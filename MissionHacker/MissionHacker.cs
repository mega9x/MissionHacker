using Crawler;
using MissionHacker.ConfigHelper;
using MissionHandler;
using Models;
using Events;
using Events.EventArgs;
using MissionHacker.ConfigHelper.Logger;
using MissionHandler.Enums;


namespace MissionHacker
{
    /// <summary>
    /// 入口点
    /// </summary>

    public class MissionHacker
    {
        private int _progressBarNum = 0;
        private MailChrome _mailBrowser;
        private readonly IBrowser _mainBrowser = new Chrome();
        public bool CanChangeIp = true;
        public Config.Config Config { get; private set; } = global::Config.Config.Instance;
        public MissionLoader MissionLoader { get; private set; } = new MissionLoader();
        public MailProvider.MailProvider Mail { get; private set; } = MailProvider.MailProvider.Instance;
        public async Task LoadMission()
        {
            await MissionLoader.LoadMission();
        }
        public int GetMissionListCount()
        {
            return MissionLoader.MissionList.Count;
        }
        public async Task Run()
        {
            _ = global::Config.Config.Instance;
            _mailBrowser = new MailChrome();
            if (MissionLoader.MissionList.Count <= 0)
            {
                return;
            }
            while (true)
            {
                MissionEvents.OnMissionLoaded(this, new MissionLoadedArgs()
                {
                    Max = MissionLoader.MissionList.Count,
                });
                // 这轮处理是否刚开始
                var first = true;
                // 获取邮箱池内的邮箱
                Mail mail;
                try
                {
                    mail = new(await Mail.GetMail());
                }
                catch (Exception)
                {
                    throw new("邮箱已用完或无法获取");
                }
                var area = "";
                var allHandled = true;
                foreach (var l in MissionLoader.MissionList)
                {
                    // 计算这个任务组是不是已经做完了. 如果所有任务组都是 True, 那么 allHandled 也是 true. 则软件停止
                    allHandled = allHandled && l.IsAllHandled();
                    if (l.IsAllHandled())
                    {
                        continue;
                    }
                    Console.WriteLine(l.Keyword);
                    // 获取链接. 没链接就获取代码.
                    var code = l.GetCode();
                    // 查看这个邮箱是否已经做了这个任务
                    if (code is null || Mail.IsMailUsed(l.Keyword)) continue;
                    var link = new Links
                    {
                        Link = code.Code,
                        Domain = code.Domain,
                    };
                    // 如果当前的 ip 地区不为任务要求的地区或者这是某一轮的第一个任务组, 则执行换 ip 的操作
                    if (l.Area != area || first)
                    {
                        if (CanChangeIp)
                        {
                            await _mainBrowser.ChangeIp(l.Area);
                        }
                        area = l.Area;
                        first = false;
                    }
                    // 把当前邮箱和链接塞进一个新的待处理任务信息
                    var info = new MissionInfo()
                    {
                        Mail = mail,
                        Links = link.Link.Length <= 0 ? null : link,
                    };
                    try
                    {
                        // 检测支持的任务里面是否有该任务组的关键词. 如果没有则锁住该任务组.
                        if (!Missions.AllMissionsKeywordSupported.ContainsKey(l.Keyword))
                        {
                            l.Lock();
                            continue;
                        }
                        // 根据关键词获取任务处理器
                        var handler = Missions.AllMissionsKeywordSupported[l.Keyword].SetInfo(info);
                        // 启动任务处理器
                        // await handler.SetBrowser(mainBrowser);
                        _mailBrowser.Mail = info.Mail;
                        await (await handler
                                .SetBrowser(_mainBrowser))
                            .SetMailBrowser(_mailBrowser);
                        await handler.Init();
                        await handler.RunAsync();
                        await MissionLogger.Instance.Success(code.Code);
                    }
                    catch (Exception e)
                    {
                        MissionEvents.ThrowException(this, e, $"任务 {l.Keyword} ({link.Link} | {link.Domain}) 运行失败");
                        await MissionLogger.Instance.Failed(code.Code);
                    }
                    finally
                    {
                        _progressBarNum++;
                        MissionEvents.SetMissionDoneNum(_progressBarNum);
                        await l.NextCode();
                        Mail.Record(l.Keyword);
                        await _mailBrowser.Quit();
                    }
                }
                _progressBarNum = 0;
                MissionEvents.SetMissionDoneNum(_progressBarNum);
                if (allHandled)
                {
                    break;
                }
            }
            await MissionLogger.Instance.Log();
        }
    }
}
