using Crawler;
using Models;

namespace MissionHandler.MissionFactory;

public interface IMissionHandler
{
    public IMissionHandler Init();
    public Task<IMissionHandler> SetMailBrowser(MailChrome browser);
    public Task<IMissionHandler> SetBrowser(IBrowser browser);
    public Task<IMissionHandler> RunAsync();
    public IMissionHandler SetInfo(MissionInfo info);

}