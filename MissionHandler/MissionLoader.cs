using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text.Json;
using Models;
using Models.ConstStr;
using Models.Enums;

namespace MissionHandler;

public class MissionLoader
{
    public List<MissionModel> MissionList { get; private set; } = new();

    public async Task LoadMission()
    {
        var client = new HttpClient();
        var response = await client.GetStreamAsync(Api.GET_MISSION);
        var list = JsonSerializer.Deserialize<IEnumerable<BackendModel>>(response);
        var missionList = (JsonSerializer
                .Deserialize<IEnumerable<MissionConfigModel>>(await File.ReadAllTextAsync(ConfigPath.CONST_MISSION)) ?? Array.Empty<MissionConfigModel>())
            .ToList();
        var blockList = new List<MissionConfigModel>();
        var blockListPath = ConfigPath.BLOCK_LSIT.Replace("date", DateTime.Now.ToString("yyyy-M-d"));
        if (!string.IsNullOrEmpty(blockListPath))
        {
            if (!File.Exists(blockListPath))
            {
                File.Create(blockListPath).Close();
            }
            else
            {
                try
                {
                    blockList =
                        (JsonSerializer.Deserialize<IEnumerable<MissionConfigModel>>(File.ReadAllText(blockListPath)) ??
                         Array.Empty<MissionConfigModel>())
                        .ToList();
                    blockList = blockList.Where(s =>
                        Missions.AllMissionsKeywordSupported.Contains(s.Keyword)
                    ).ToList();
                }
                catch(Exception e)
                {
                    blockList = new();
                }

            }
        }
        // 处理获取的任务列表并去除不做的任务
        foreach (var l in list)
        {
            // 如果黑名单
            if (blockList.Find(i => l.Name.ToLower().Contains(i.Keyword)) is not null)
            {
                continue;
            }
            // 如果这个任务关键词是匹配的
            var keyword = Missions.AllMissionsKeywordSupported.Find(i => l.Name.ToLower().Contains(i));
            if (keyword is null)
            {
                continue;
            }
            var missionConfig = missionList.Find(i => i.Keyword == keyword);
            if (missionConfig is null)
            {
                continue;
            }
            // 如果 MissionModel 列表内可以找到
            var missionModel = MissionList.Find(m => m.Keyword == keyword);
            if (missionModel is null)
            {
                missionModel = new MissionModel()
                {
                    Keyword = keyword,
                    Area = missionConfig.Area,
                    Platform = l.Platform,
                    Times = RandomNumberGenerator.GetInt32(missionConfig.MinTimes, missionConfig.MaxTimes + 1),
                };
                var mis = PushMissionModel(missionModel, l);
                if (mis is null)
                {
                    continue;
                }
                MissionList.Add(mis);
            }
            else
            {
                var mis = PushMissionModel(missionModel, l);
                if (mis is null)
                {
                    continue;
                }
                var index = MissionList.FindIndex(m => m.Keyword == keyword);
                MissionList[index] = mis;
            }
        }
    }
    
    private MissionModel? PushMissionModel(MissionModel missionModel, BackendModel backendModel)
    {
        if (backendModel.Code is not null && backendModel.Code.Length > 0)
        {
            missionModel.PushCode(backendModel.Code);
            return missionModel;
        }
        if (backendModel.Link is not null && backendModel.Link.Length > 0)
        {
            missionModel.PushLink(backendModel.Link);
            return missionModel;
        }

        return null;
    }
}