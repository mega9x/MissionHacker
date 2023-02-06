using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using ConstStr;
using DataLibs;
using MissionHacker.ConfigHelper;
using MissionHacker.ConfigHelper.Logger;
using MissionHandler.Enums;
using Models;
using Models.Config.MissionHacker;
using Models.Log;

namespace MissionHandler;

public class MissionLoader
{
    private List<BackendModel> _backendModels = new();
    private LoaderOptions _options = new();
    public List<MissionModel> MissionList { get; private set; } = new();
    public MissionLoader()
    { }
    public MissionLoader(LoaderOptions options)
    {
        _options = options;
    }
    public void Clear()
    {
        _backendModels.Clear();
        MissionList.Clear();
        MissionLogger.Instance.Clear();
    }
    public async Task LoadMission()
    {
        Clear();
        await RefreshBackend();
        var supportedMissions = (JsonSerializer
                .Deserialize<IEnumerable<MissionConfigModel>>(await File.ReadAllTextAsync(ConfigPath.ConstMission)) ?? Array.Empty<MissionConfigModel>())
            .ToList();
        // 今天做过的任务
        var blockList = new List<string>();
        // 处理获取的任务列表并去除不做的任务
        foreach (var l in _backendModels)
        {
            // 如果这个任务关键词是匹配的
            var keyword = Missions.AllMissionsKeywordSupported.Keys.ToList().Find(i => l.Name.ToLower().Contains(i));
            if (keyword is null)
            {
                continue;
            }
            // 如果已启用
            if (l.Launched == "false")
            {
                continue;
            }
            // 如果识别码匹配
            if ((l.MId != Config.Config.Instance.MissionHackerConfig.General.Id) && _options.MatchMId)
            {
                continue;
            }
            // 如果 MissionModel 列表内可以找到
            var missionConfig = supportedMissions.Find(i => i.Keyword == keyword);
            if (missionConfig is null && _options.UseLocalWhiteLsit)
            {
                continue;
            }
            var area = l.Area;
            var times = int.Parse(l.Times);
            if (_options.UseLocalWhiteLsit)
            {
                area = missionConfig.Area;
                times = RandomNumberGenerator.GetInt32(missionConfig.MinTimes, missionConfig.MaxTimes + 1);
            }
            // 如果已经是最大次数
            var finalTimes = times - Libs.Instance.GetCountByKeyword(keyword);
            if(finalTimes <= 0) continue;
            var missionModel = MissionList.Find(m => m.Keyword == keyword);
            if (missionModel is null)
            {
                missionModel = new MissionModel()
                {
                    Fullname = l.Name,
                    Keyword = keyword,
                    Area =  area,
                    Platform = l.Platform,
                    MaxTimes = times,
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
            MissionList.Sort((left, right) =>
                string.Compare(right.Area, left.Area, StringComparison.Ordinal)
            );
            MissionLogger.Instance.Push(new () {
                Fullname = l.Name,
                Price = l.Price,
                Link = l.GetLink(),
                MaxTimes = times,
            });
        }
    }
    private async Task RefreshBackend()
    {
        await (await GetMissionFromBackend()).DistinctBackendList();
    }
    private async Task<MissionLoader> GetMissionFromBackend()
    {
        var client = new HttpClient();
        var response = await client.GetStreamAsync(Api.GET_MISSION);
        _backendModels = JsonSerializer.Deserialize<IEnumerable<BackendModel>>(response).ToList();
        return this;
    }
    private async Task<MissionLoader> DistinctBackendList()
    {
        // code link 去重
        var missionHasLink = _backendModels.Where(i => i.Link is not null && i.Link.Length > 0);
        var missionHasCode = _backendModels.Where(i => i.Code is not null && i.Code.Length > 0 && i.Link.Length <= 0);
        var distinctCode = missionHasCode.Select(i => i.Code).Distinct().ToList();
        var distinctLink = missionHasLink.Select(i => i.Link).Distinct().ToList();
        var finalCodeMission = missionHasCode.Where(i =>
        {
            var contains =  distinctCode.Contains(i.Code);
            if (contains)
            {
                distinctCode.RemoveAll(j => i.Code == j);
            }
            return contains;
        }).ToList();
        var finalLinkMission = missionHasLink.Where(i =>
        {
            var contains =  distinctLink.Contains(i.Link);
            if (contains)
            {
                distinctLink.RemoveAll(j => i.Link == j);
            }
            return contains;
        }).ToList();
        finalCodeMission.AddRange(finalLinkMission);
        _backendModels = finalCodeMission;
        return this;
    }
    
    
    private MissionModel? PushMissionModel(MissionModel missionModel, BackendModel backendModel)
    {
        missionModel.PushCode(backendModel.GetLink(), backendModel.Domain.Trim());
        return missionModel;
    }
}