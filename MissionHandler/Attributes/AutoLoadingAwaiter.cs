namespace MissionHandler.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class AutoLoadingAwaiter: Attribute
{
    public object?[] Args { get; }
    public AutoLoadingAwaiter() {}
    public AutoLoadingAwaiter(object?[] args) => Args = args;
}