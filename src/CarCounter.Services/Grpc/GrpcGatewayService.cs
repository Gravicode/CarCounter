using CarCounter.Data;
using CarCounter.Models;
using CarCounter.Services.IServices;
using ProtoBuf.Grpc;

namespace CarCounter.Services.Grpc
{
    public class GrpcGatewayService : IGateway, ICrudGrpc<Gateway>
    {
        private readonly ILogger<GrpcGatewayService> _logger;
        private readonly IWorkspace _workspace;

        public GrpcGatewayService(ILogger<GrpcGatewayService> logger, IWorkspace workspace)
        {
            _logger = logger;
            _workspace = workspace;
        }

        public async Task<List<Gateway>?> FindByKeyword(string Keyword, CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(FindByKeyword)}");
            var res = await _workspace.Gateways.GetAll(x => x.Nama.ToLower().Trim()
                .Contains(Keyword.ToLower().Trim()));
            _logger.LogInformation($"Executing {nameof(FindByKeyword)} succeed");
            return (List<Gateway>?)res;
        }

        public async Task<List<Gateway>?> GetAllData(CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(GetAllData)}");
            var res = await _workspace.Gateways.GetAll();
            _logger.LogInformation($"Executing {nameof(GetAllData)} succeed");

            return (List<Gateway>?)res;
        }

        public async Task<Gateway?> GetDataById(InputCls Id, CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(GetDataById)}");
            var res = await _workspace.Gateways.Get(x => x.Id == long.Parse(Id.Param[0]));
            _logger.LogInformation($"Executing {nameof(GetDataById)} succeed");

            return res;
        }

        public async Task<OutputCls> GetLastId(CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(GetLastId)}");
            var res = await _workspace.Gateways.GetMax(x => x.Id);
            _logger.LogInformation($"Executing {nameof(GetLastId)} succeed");

            return new OutputCls() { Data = res.ToString() };
        }

        public async Task<OutputCls> DeleteData(InputCls Id, CallContext context)
        {
            _logger.LogInformation($"Commencing {nameof(DeleteData)}");
            await _workspace.Gateways.Delete(long.Parse(Id.Param[0]));
            await _workspace.Save();
            _logger.LogInformation($"Executing {nameof(DeleteData)} succeed");

            return new OutputCls() { Result = true };
        }

        public async Task<OutputCls> InsertData(Gateway data, CallContext context)
        {
            _logger.LogInformation($"Commencing {nameof(InsertData)}");
            await _workspace.Gateways.Create(data);
            await _workspace.Save();
            _logger.LogInformation($"Executing {nameof(InsertData)} succeed");

            return new OutputCls() { Result = true };
        }

        public async Task<OutputCls> UpdateData(Gateway data, CallContext context)
        {
            _logger.LogInformation($"Commencing {nameof(UpdateData)}");
            await _workspace.Gateways.Update(data);
            await _workspace.Save();
            _logger.LogInformation($"Executing {nameof(UpdateData)} succeed");

            return new OutputCls() { Result = true };
        }
    }
}
