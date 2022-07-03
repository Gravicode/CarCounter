using CarCounter.Data;
using CarCounter.Models;
using CarCounter.Services.IServices;
using ProtoBuf.Grpc;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace CarCounter.Services.Grpc
{
    public class GrpcUserProfileService : IUserProfile, ICrudGrpc<UserProfile>
    {
        private readonly ILogger<GrpcUserProfileService> _logger;
        private readonly IWorkspace _workspace;

        public GrpcUserProfileService(ILogger<GrpcUserProfileService> logger, IWorkspace workspace)
        {
            _logger = logger;
            _workspace = workspace;
        }

        public async Task<List<UserProfile>?> FindByKeyword(string Keyword, CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(FindByKeyword)}");
            var res = await _workspace.UserProfiles.GetAll(x => x.Email.ToLower().Trim()
                .Contains(Keyword.ToLower().Trim()));
            _logger.LogInformation($"Executing {nameof(FindByKeyword)} succeed");
            return (List<UserProfile>?)res;
        }

        public async Task<List<UserProfile>?> GetAllData(CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(GetAllData)}");
            var res = await _workspace.UserProfiles.GetAll();
            _logger.LogInformation($"Executing {nameof(GetAllData)} succeed");

            return (List<UserProfile>?)res;
        }

        public async Task<UserProfile?> GetDataById(InputCls Id, CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(GetDataById)}");
            var res = await _workspace.UserProfiles.Get(x => x.Id == long.Parse(Id.Param[0]));
            _logger.LogInformation($"Executing {nameof(GetDataById)} succeed");

            return res;
        }

        public async Task<OutputCls> GetLastId(CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(GetLastId)}");
            var res = await _workspace.UserProfiles.GetMax(x => x.Id);
            _logger.LogInformation($"Executing {nameof(GetLastId)} succeed");

            return new OutputCls() { Data = res.ToString() };
        }

        public async Task<OutputCls> DeleteData(InputCls Id, CallContext context)
        {
            _logger.LogInformation($"Commencing {nameof(DeleteData)}");
            await _workspace.UserProfiles.Delete(long.Parse(Id.Param[0]));
            await _workspace.Save();
            _logger.LogInformation($"Executing {nameof(DeleteData)} succeed");

            return new OutputCls() { Result = true };
        }

        public async Task<OutputCls> InsertData(UserProfile data, CallContext context)
        {
            _logger.LogInformation($"Commencing {nameof(InsertData)}");
            await _workspace.UserProfiles.Create(data);
            await _workspace.Save();
            _logger.LogInformation($"Executing {nameof(InsertData)} succeed");

            return new OutputCls() { Result = true };
        }

        public async Task<OutputCls> UpdateData(UserProfile data, CallContext context)
        {
            _logger.LogInformation($"Commencing {nameof(UpdateData)}");
            await _workspace.UserProfiles.Update(data);
            await _workspace.Save();
            _logger.LogInformation($"Executing {nameof(UpdateData)} succeed");

            return new OutputCls() { Result = true };
        }

        public async Task<UserProfile> GetItemByEmail(InputCls input, CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(GetItemByEmail)}");
            var res = await _workspace.UserProfiles.Get(x => x.Email.ToLower().Trim()
                .Contains(input.Param[0].ToLower().Trim()));
            _logger.LogInformation($"Executing {nameof(GetItemByEmail)} succeed");

            return res;
        }

        public async Task<UserProfile> GetItemByPhone(InputCls input, CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(GetItemByPhone)}");
            var res = await _workspace.UserProfiles.Get(x => x.Phone.ToLower().Trim()
                .Contains(input.Param[0].ToLower().Trim()));
            _logger.LogInformation($"Executing {nameof(GetItemByPhone)} succeed");

            return res;
        }

        public async Task<OutputCls> IsUserExists(InputCls input, CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(IsUserExists)}");
            var res = await _workspace.UserProfiles.DataExist(x => x.Username.ToLower().Trim()
                .Equals(input.Param[0].ToLower().Trim()));
            _logger.LogInformation($"Executing {nameof(IsUserExists)} succeed");

            return new OutputCls() { Result = res } ;
        }

        public async Task<OutputCls> GetUserRole(InputCls input, CallContext context = default)
        {
            _logger.LogInformation($"Commencing {nameof(FindByKeyword)}");
            var res = await _workspace.UserProfiles.Get(x => x.Username.ToLower().Trim()
                .Contains(input.Param[0].ToLower().Trim()));
            _logger.LogInformation($"Executing {nameof(FindByKeyword)} succeed");

            return new OutputCls() { Data = res.Role.ToString(), Result = true };
        }
    }
}
