using PlatformService.DTOs;
using System.Threading.Tasks;

namespace PlatformService.SyncDataServices.Http.Interfaces
{
    public interface ICommandDataClient
    {
        Task SendPlatformToCommand(PlatformReadDTO platformReadDTO);
    }
}
