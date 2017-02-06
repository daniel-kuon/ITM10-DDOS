namespace DDoSDetector.Models
{
    public enum RequestState
    {
        Analyzing,
        Blocked,
        WaitingForResponse,
        Completed,
        TimeOut
    }
}