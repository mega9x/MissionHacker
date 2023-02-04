namespace Events.EventArgs;

public class ThrowEventArgs
{
    public string? SimpleMessage { get; init; }
    public string? FullMessage { get; init; }
    public Exception? Exception { get; set; }
}