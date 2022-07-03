using CarCounter.Data;
using CarCounter.Models;
using CarCounter.Services.IServices;
using ProtoBuf.Grpc;

namespace CarCounter.Services.Grpc
{
    public class GrpcDataCounterService : IDataCounter, ICrudGrpc<DataCounter>
    {
        private readonly ILogger<GrpcDataCounterService> _logger;
        private readonly IWorkspace _workspace;

        public GrpcDataCounterService(ILogger<GrpcDataCounterService> logger, IWorkspace workspace)
        {
            _logger = logger;
            _workspace = workspace;
        }

        public async Task<List<DataCounter>?> FindByKeyword(string Keyword, CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(FindByKeyword)}");
            var res = await _workspace.DataCounters.GetAll(x => x.Jenis.ToLower().Trim()
                .Contains(Keyword.ToLower().Trim()));
            _logger.LogInformation($"Executing {nameof(FindByKeyword)} succeed");
            return (List<DataCounter>?)res;
        }

        public async Task<List<DataCounter>?> GetAllData(CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(GetAllData)}");
            var res = await _workspace.DataCounters.GetAll();
            _logger.LogInformation($"Executing {nameof(GetAllData)} succeed");

            return (List<DataCounter>?)res;
        }

        public async Task<DataCounter?> GetDataById(InputCls Id, CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(GetDataById)}");
            var res = await _workspace.DataCounters.Get(x => x.Id == long.Parse(Id.Param[0]));
            _logger.LogInformation($"Executing {nameof(GetDataById)} succeed");

            return res;
        }

        public async Task<OutputCls> GetLastId(CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(GetLastId)}");
            var res = await _workspace.DataCounters.GetMax(x => x.Id);
            _logger.LogInformation($"Executing {nameof(GetLastId)} succeed");

            return new OutputCls() { Data = res.ToString() };
        }

        public async Task<OutputCls> DeleteData(InputCls Id, CallContext context)
        {
            _logger.LogInformation($"Commencing {nameof(DeleteData)}");
            await _workspace.DataCounters.Delete(long.Parse(Id.Param[0]));
            await _workspace.Save();
            _logger.LogInformation($"Executing {nameof(DeleteData)} succeed");

            return new OutputCls() { Result = true };
        }

        public async Task<OutputCls> InsertData(DataCounter data, CallContext context)
        {
            _logger.LogInformation($"Commencing {nameof(InsertData)}");
            await _workspace.DataCounters.Create(data);
            await _workspace.Save();
            _logger.LogInformation($"Executing {nameof(InsertData)} succeed");

            return new OutputCls() { Result = true };
        }

        public async Task<OutputCls> UpdateData(DataCounter data, CallContext context)
        {
            _logger.LogInformation($"Commencing {nameof(UpdateData)}");
            await _workspace.DataCounters.Update(data);
            await _workspace.Save();
            _logger.LogInformation($"Executing {nameof(UpdateData)} succeed");

            return new OutputCls() { Result = true };
        }
    }
}
