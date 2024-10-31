namespace OpcUa.Client.Infrastructiure
{
    public interface ICircuitBreaker
    {
        bool CanAttempt();
        void RecordFailure();
        void Reset();
    }
}