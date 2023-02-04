using ConstStr;
using Models.Log;

namespace MissionHacker.ConfigHelper.Logger;

public class MissionLogger
{
    private List<MissionDone> missions = new();
    public static MissionLogger Instance = new Lazy<MissionLogger>(() => new MissionLogger()).Value;
    private MissionLogger()
    { }
    public MissionLogger Push(MissionDone missionDone)
    {
        missions.Add(missionDone);
        return this;
    }
    public async Task<MissionLogger> Failed(string code)
    {
        missions.First(x => x.Link == code).FailedTimes++;
        return this;
    }
    public async Task<MissionLogger> Success(string code)
    {
        missions.First(x => x.Link == code).SuccessTimes++;
        return this;
    }
    public async Task<MissionLogger> Log()
    {
        var texts = missions.Select(x => x.ToString());
        try
        {
            File.Delete(ConfigPath.HANDLED_MISSION_LOG.Replace("date", DateTime.Now.ToString("yyyy-M-d dddd")));
        }
        catch
        {
            // ignore
        }
        await File.WriteAllLinesAsync(ConfigPath.HANDLED_MISSION_LOG.Replace("date", DateTime.Now.ToString("yyyy-M-d dddd")), texts);
        return this;
    }
    public MissionLogger Clear()
    {
        missions.Clear();
        return this;
    }
}