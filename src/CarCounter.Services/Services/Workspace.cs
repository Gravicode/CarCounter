using CarCounter.Models;
using CarCounter.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CarCounter.Services.Services
{
    public class Workspace : IWorkspace
    {

        private readonly DbContext _db;

        private IGenericCrud<Gateway>? _gateways { get; }
        private IGenericCrud<CCTV>? _cctvs { get; }
        private IGenericCrud<DataCounter>? _dataCounters { get; }

        public IGenericCrud<Gateway> Gateways => _gateways ?? new GenericCrud<Gateway>(_db);

        public IGenericCrud<CCTV> Cctvs => throw new NotImplementedException();

        public IGenericCrud<DataCounter> DataCounters => throw new NotImplementedException();

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }

        public Task<bool> Save()
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
