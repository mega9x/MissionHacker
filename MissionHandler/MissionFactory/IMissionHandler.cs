using Crawler;
using Models;

namespace MissionHandler.MissionFactory;

public interface IMissionHandler
{
    public IMissionHandler SetBrowser(IBrowser browser);
    public Task<IMissionHandler> RunAsync();
    public IMissionHandler SetInfo(MissionInfo info);
}