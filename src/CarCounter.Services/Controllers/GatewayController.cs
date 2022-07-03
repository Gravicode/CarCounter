using CarCounter.Models.Custom.DTO;
using CarCounter.Services.IServices;

namespace CarCounter.Services.Controllers
{
    public static partial class RoutingExtensions
    {
        public static void GatewayApiMapping(this WebApplication app)
        {
            app.MapGet("/gateway/getall", async (IWorkspace service) =>
            {
                app.Logger.LogInformation("Commencing get gateway");
                var gateways = await service.Gateways.GetAll();
                app.Logger.LogInformation("Get gateway succeed");
                return gateways;
            })
            .WithName("GetGateways")
            .Produces(StatusCodes.Status200OK);

            app.MapGet("/gateway/get/{id}", async (IWorkspace service,
                long id) =>
            {
                app.Logger.LogInformation("Commencing get gateway");
                var gateway = await service.Gateways.Get(x => x.Id == Convert.ToInt64(id));
                app.Logger.LogInformation("Get gateway succeed");

                return gateway;
            })
            .WithName("GetGateway")
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);

            app.MapPost("/gateway/add", async (IWorkspace service,
                CreateGatewayDTO data) =>
            {
                return Results.Ok("test");
            })
                .WithName("AddGateway")
                .Produces(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status500InternalServerError);
        }
    }
}
