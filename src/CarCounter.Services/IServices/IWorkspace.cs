using CarCounter.Models;

namespace CarCounter.Services.IServices
{
    public interface IWorkspace : IDisposable
    {
        IGenericCrud<Gateway> Gateways { get; }
        IGenericCrud<CCTV> Cctvs { get; }
        IGenericCrud<DataCounter> DataCounters { get; }
        IGenericCrud<UserProfile> UserProfiles { get; }

        Task<bool> Save();
    }
}
