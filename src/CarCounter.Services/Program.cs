using CarCounter.Tools;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using CarCounter.Data;
using CarCounter.Services.Grpc;
using ProtoBuf.Grpc.Server;
using System.Text;
using CarCounter.Services.Helpers;
using Microsoft.EntityFrameworkCore;
using Serilog;
using CarCounter.Services.Extension;
using CarCounter.Services.Controllers;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// logger with serilog
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// implement DI
builder.Services.InjectDbWorkspace();

// db first
//builder.Services.AddDbContext<CarCounterDB>(option =>
//    option.UseMySql(builder.Configuration.GetConnectionString("SqlConn"), 
//    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("SqlConn")))
//);

ConfigureServices(builder.Services, builder.Configuration);
// Configure Kestrel to listen on a specific HTTP port 
/*
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
    options.ListenAnyIP(8585, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});
*/
var app = builder.Build();
Configure(app, app.Environment);
ConfigureRouting(app);

// global error handler
app.CustomErrorHandler();

// start logging
try
{
    logger.Information("Starting CarContainer.Service API");
}
catch (Exception ex)
{
    logger.Fatal(ex, "Fatal error when starting CarContainer.Service API");
}

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration Configuration)
{
    //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    //services.AddCors(options =>
    //{
    //    options.AddPolicy("AllowAllOrigins",
    //        builder => builder.AllowAnyOrigin().AllowAnyHeader().WithMethods("GET, PATCH, DELETE, PUT, POST, OPTIONS"));
    //});

    AppConstants.SQLConn = Configuration["ConnectionStrings:SqlConn"];
    AppConstants.RedisCon = Configuration["RedisCon"];
    AppConstants.BlobConn = Configuration["BlobConn"];

    MailService.MailUser = Configuration["MailSettings:MailUser"];
    MailService.MailPassword = Configuration["MailSettings:MailPassword"];
    MailService.MailServer = Configuration["MailSettings:MailServer"];
    MailService.MailPort = int.Parse(Configuration["MailSettings:MailPort"]);
    MailService.SetTemplate(Configuration["MailSettings:TemplatePath"]);
    MailService.SendGridKey = Configuration["MailSettings:SendGridKey"];
    MailService.UseSendGrid = true;


    SmsService.UserKey = Configuration["SmsSettings:ZenzivaUserKey"];
    SmsService.PassKey = Configuration["SmsSettings:ZenzivaPassKey"];
    SmsService.TokenKey = Configuration["SmsSettings:TokenKey"];


    /*
    //ML
    services.AddPredictionEnginePool<ImageInputData, TinyYoloPrediction>().
        FromFile(_mlnetModelFilePath);

    //services.AddTransient<IImageFileWriter, ImageFileWriter>();
    services.AddTransient<IObjectDetectionService, ObjectDetectionService>();
    */
    //services.AddSignalR(hubOptions =>
    //{
    //    hubOptions.MaximumReceiveMessageSize = 128 * 1024; // 1MB
    //});

    CarCounterDB db = new CarCounterDB();
    db.Database.EnsureCreated();

    //GRPC
    services.AddCodeFirstGrpc(options =>
    {
        options.EnableDetailedErrors = true;
        options.MaxReceiveMessageSize = 8 * 1024 * 1024; // 2 MB
        options.MaxSendMessageSize = 8 * 1024 * 1024; // 5 MB
    });
    services.AddGrpcHealthChecks()
                    .AddCheck("Sample", () => HealthCheckResult.Healthy());
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    //only for GRPC WEB
    services.AddCors(o => o.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
    }));
    services.Configure<ForwardedHeadersOptions>(options =>
    {
        options.KnownProxies.Add(IPAddress.Parse("103.189.234.36"));
    });

}
void Configure(WebApplication app, IWebHostEnvironment env)
{
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    //if use GRPC Native
    // app.UseCors(x => x
    //.AllowAnyMethod()
    //.AllowAnyHeader()
    //.SetIsOriginAllowed(origin => true) // allow any origin  
    //.AllowCredentials());               // allow credentials 
    // Configure the HTTP request pipeline.
    //if (env.IsDevelopment())
    //{
    app.UseSwagger();
    app.UseSwaggerUI();
    //}
    if (!env.IsDevelopment())
    {
        //app.UseHttpsRedirection();
    }

    app.UseStaticFiles();

    // grpc web
    // Configure the HTTP request pipeline.
    app.UseRouting();
    app.UseGrpcWeb();
    app.UseCors();

    //GRPC Native
    //app.MapGrpcService<GrpcNgajiService>();
    //app.MapGrpcService<GrpcAuthService>();
    //app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
    //app.MapGrpcHealthChecksService();

    //GRPC WEB
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapGrpcService<GrpcGatewayService>().EnableGrpcWeb().RequireCors("AllowAll");
        endpoints.MapGrpcService<GrpcCCTVService>().EnableGrpcWeb().RequireCors("AllowAll");
        endpoints.MapGrpcService<GrpcDataCounterService>().EnableGrpcWeb().RequireCors("AllowAll");
        endpoints.MapGrpcService<GrpcUserProfileService>().EnableGrpcWeb().RequireCors("AllowAll");
        endpoints.MapGrpcService<GrpcAuthService>().EnableGrpcWeb().RequireCors("AllowAll");


        //    endpoints.MapGrpcHealthChecksService().EnableGrpcWeb().RequireCors("AllowAll");

        endpoints.MapGet("/", async context =>
        {
            await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
        });
    });
}

void ConfigureRouting(WebApplication app)
{
    app.WeatherForecastApiMapping();
    app.GatewayApiMapping();
}