using static System.Int32;

namespace Models.Log;

public class MissionDone
{
    public string Fullname { get; set; }
    public string Link { get; set; }
    public string Price { get; set; }
    public int MaxTimes { get; set; }
    public int FailedTimes { get; set; }
    public int SuccessTimes { get; set; }
    public override string ToString()
    {
        var price = 0;
        var tryParse = TryParse(Price, out price);
        price = price * SuccessTimes;

        return $"<======================== {Environment.NewLine}任务全名: {Fullname} {Environment.NewLine}链接: {Link} {Environment.NewLine}应做次数: {MaxTimes} {Environment.NewLine}成功次数: {SuccessTimes} {Environment.NewLine}失败次数: {FailedTimes} {Environment.NewLine}预测金额: {price} {Environment.NewLine}========================>";
    }
}