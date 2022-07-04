using CarCounter.Models;
using CarCounter.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CarCounter.Services.Services
{
    public class Workspace : IWorkspace
    {

        private readonly DbContext _db;

        public Workspace(DbContext db)
        {
            _db = db;
        }

        private IGenericCrud<Gateway>? _gateways { get; }
        private IGenericCrud<CCTV>? _cctvs { get; }
        private IGenericCrud<DataCounter>? _dataCounters { get; }
        private IGenericCrud<UserProfile>? _userProfiles { get; }

        public IGenericCrud<Gateway> Gateways => _gateways ?? new GenericCrud<Gateway>(_db);

        public IGenericCrud<CCTV> Cctvs => _cctvs ?? new GenericCrud<CCTV>(_db);

        public IGenericCrud<DataCounter> DataCounters => _dataCounters ?? new GenericCrud<DataCounter>(_db);

        public IGenericCrud<UserProfile> UserProfiles => _userProfiles ??  new GenericCrud<UserProfile>(_db);

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<bool> Save()
        {
            try
            {
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
