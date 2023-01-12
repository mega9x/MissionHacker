using Events.EventArgs;

namespace Events;

/// <summary>
/// 事件类
/// </summary>
public static class MissionEvents
{
    public delegate void MissionDoneDelegate(int num);
    
    /// <summary>
    /// 当任务结束后触发. 参数是当前任务在所有获取到的任务里的列表索引值.
    /// </summary>
    public static event MissionDoneDelegate? MissionDoneEvent;
    /// <summary>
    /// 抛出异常时触发.
    /// </summary>
    public static event EventHandler<ThrowEventArgs> ThrowExceptionEvent;
    /// <summary>
    /// 发送信息的事件. 仅用于接收额外的信息.
    /// </summary>
    public static event EventHandler<string> SendMsgToFormEvent;
    /// <summary>
    /// 任务列表准备好时的事件.
    /// </summary>
    public static event EventHandler<MissionLoadedArgs> MissionLoadedEvent;
    public static void SetMissionDoneNum(int num)
    {
        MissionDoneEvent(num);
    }

    public static void ThrowException(object sender, Exception e, string simpleMessage)
    {
        ThrowExceptionEvent(sender, new()
        {
            Exception = e,
            FullMessage = e.ToString(),
            SimpleMessage = simpleMessage,
        });
    }
    
    
    public static void ThrowException(object sender, ThrowEventArgs args)
    {
        ThrowExceptionEvent(sender, args);
    }

    public static void SendMsgToForm(object sender, string msg)
    {
        SendMsgToFormEvent(sender, msg);
    }

    public static void OnMissionLoaded(object sender, MissionLoadedArgs args)
    {
        MissionLoadedEvent(sender, args);
    }
}