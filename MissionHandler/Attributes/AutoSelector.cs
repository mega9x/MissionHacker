namespace MissionHandler.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class AutoSelector: Attribute
{
    public object?[] Args { get; }
    public AutoSelector() {}
    public AutoSelector(object?[] args) => Args = args;
}