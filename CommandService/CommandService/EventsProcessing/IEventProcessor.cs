using System.Threading.Tasks;

namespace CommandService.EventsProcessing
{
    public interface IEventProcessor
    {
        Task ProcessEvent(string message);
    }
}
