using PlatformService.DTOs;
using System.Threading.Tasks;

namespace PlatformService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        Task PublishNewPlatformAsync(PlatformPublishedDTO platformPublishedDTO);
    }
}
