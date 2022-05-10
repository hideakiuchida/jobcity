using System.Threading.Tasks;

namespace Jobcity.Chat.InfraLayer.Contracts
{
    public interface IChatApiProxy
    {
        Task<string> DownloadCsv(string stockCode);
    }
}
