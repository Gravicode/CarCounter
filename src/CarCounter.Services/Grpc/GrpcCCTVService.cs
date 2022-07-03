using CarCounter.Data;
using CarCounter.Models;
using CarCounter.Services.IServices;
using ProtoBuf.Grpc;

namespace CarCounter.Services.Grpc
{
    public class GrpcCCTVService : ICCTV, ICrudGrpc<CCTV>
    {
        private readonly ILogger<GrpcCCTVService> _logger;
        private readonly IWorkspace _workspace;

        public GrpcCCTVService(ILogger<GrpcCCTVService> logger, IWorkspace workspace)
        {
            _logger = logger;
            _workspace = workspace;
        }

        public async Task<List<CCTV>?> FindByKeyword(string Keyword, CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(FindByKeyword)}");
            var res = await _workspace.Cctvs.GetAll(x => x.Nama.ToLower().Trim()
                .Contains(Keyword.ToLower().Trim()));
            _logger.LogInformation($"Executing {nameof(FindByKeyword)} succeed");
            return (List<CCTV>?)res;
        }

        public async Task<List<CCTV>?> GetAllData(CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(GetAllData)}");
            var res = await _workspace.Cctvs.GetAll();
            _logger.LogInformation($"Executing {nameof(GetAllData)} succeed");

            return (List<CCTV>?)res;
        }

        public async Task<CCTV?> GetDataById(InputCls Id, CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(GetDataById)}");
            var res = await _workspace.Cctvs.Get(x => x.Id == long.Parse(Id.Param[0]));
            _logger.LogInformation($"Executing {nameof(GetDataById)} succeed");

            return res;
        }

        public async Task<OutputCls> GetLastId(CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(GetLastId)}");
            var res = await _workspace.Cctvs.GetMax(x => x.Id);
            _logger.LogInformation($"Executing {nameof(GetLastId)} succeed");

            return new OutputCls() { Data = res.ToString() };
        }

        public async Task<OutputCls> DeleteData(InputCls Id, CallContext context)
        {
            _logger.LogInformation($"Commencing {nameof(DeleteData)}");
            await _workspace.Cctvs.Delete(long.Parse(Id.Param[0]));
            await _workspace.Save();
            _logger.LogInformation($"Executing {nameof(DeleteData)} succeed");

            return new OutputCls() { Result = true };
        }

        public async Task<OutputCls> InsertData(CCTV data, CallContext context)
        {
            _logger.LogInformation($"Commencing {nameof(InsertData)}");
            await _workspace.Cctvs.Create(data);
            await _workspace.Save();
            _logger.LogInformation($"Executing {nameof(InsertData)} succeed");

            return new OutputCls() { Result = true };
        }

        public async Task<OutputCls> UpdateData(CCTV data, CallContext context)
        {
            _logger.LogInformation($"Commencing {nameof(UpdateData)}");
            await _workspace.Cctvs.Update(data);
            await _workspace.Save();
            _logger.LogInformation($"Executing {nameof(UpdateData)} succeed");

            return new OutputCls() { Result = true };
        }
    }
}
