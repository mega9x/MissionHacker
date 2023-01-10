namespace Events.EventArgs;

public class ThrowEventArgs
{
    public string SimpleMessage { get; set; }
    public string FullMessage { get; set; }
    public Exception Exception { get; set; }
}