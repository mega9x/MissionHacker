namespace Models.Config.MissionHacker;

public class MissionConfigModel
{
    public string Keyword { get; set; } = "";
    public string Area { get; set; } = "";
    public int MaxTimes { get; set; } = 0;
    public int MinTimes { get; set; } = 0;
}