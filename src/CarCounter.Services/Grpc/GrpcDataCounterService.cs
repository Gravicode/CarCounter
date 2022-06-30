using CarCounter.Data;
using CarCounter.Models;
using ProtoBuf.Grpc;

namespace CarCounter.Services.Grpc
{
    public class GrpcDataCounterService : IDataCounter
    {
        DataCounterService svc;
        public GrpcDataCounterService()
        {
            svc = new DataCounterService();
        }


        public Task<List<DataCounter>> FindByKeyword(string Keyword, CallContext context = default)
        {
            var res = svc.FindByKeyword(Keyword);
            return Task.FromResult(res);
        }

        public Task<List<DataCounter>> GetAllData(CallContext context = default)
        {
            try
            {
                var res = svc.GetAllData();
                return Task.FromResult(res);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Task.FromResult(default(List<DataCounter>));
            }
        }

        public Task<DataCounter> GetDataById(InputCls Id, CallContext context = default)
        {
            var res = svc.GetDataById(long.Parse(Id.Param[0]));
            return Task.FromResult(res);
        }

        public Task<OutputCls> GetLastId(CallContext context = default)
        {

            var res = svc.GetLastId();
            return Task.FromResult(new OutputCls() { Data = res.ToString() });
        }



        Task<OutputCls> ICrudGrpc<DataCounter>.DeleteData(InputCls Id, CallContext context)
        {
            var res = svc.DeleteData(long.Parse(Id.Param[0]));
            return Task.FromResult(new OutputCls() { Result = res });
        }



        Task<OutputCls> ICrudGrpc<DataCounter>.InsertData(DataCounter data, CallContext context)
        {
            var res = svc.InsertData(data);
            return Task.FromResult(new OutputCls() { Result = res });
        }


        Task<OutputCls> ICrudGrpc<DataCounter>.UpdateData(DataCounter data, CallContext context)
        {
            var res = svc.UpdateData(data);
            return Task.FromResult(new OutputCls() { Result = res });
        }

    }
}
