using CarCounter.Models;
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
                Gateway data) =>
            {
                app.Logger.LogInformation("Commencing add gateway");
                await service.Gateways.Create(data);
                var res = await service.Save();
                app.Logger.LogInformation("Add gateway succeed");
                return data;
            })
                .WithName("AddGateway")
                .Produces(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status500InternalServerError);

            app.MapDelete("/gaeteway/delete/{id}", async (IWorkspace service,
                long id) =>
            {
                app.Logger.LogInformation("Commencing delete gateway");
                await service.Gateways.Delete(id);
                var res = await service.Save();
                app.Logger.LogInformation("Delete gateway succeed");
                return res;
            })
                .WithName("DeleteGateway")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status500InternalServerError);

            app.MapPut("/gateway/update", async (IWorkspace service,
                Gateway data) =>
            {
                app.Logger.LogInformation("Commencing update gateway");
                await service.Gateways.Update(data);
                var res = await service.Save();
                app.Logger.LogInformation("Delete update succeed");
            })
                .WithName("UpdateGateway")
                .Produces(StatusCodes.Status204NoContent)
                .ProducesProblem(StatusCodes.Status500InternalServerError);
        }
    }
}
