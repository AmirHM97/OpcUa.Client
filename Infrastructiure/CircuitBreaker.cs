namespace OpcUa.Client.Infrastructiure;

public class CircuitBreaker : ICircuitBreaker
{
    private int _failureCount = 0;
    private readonly int _failureThreshold;
    private readonly TimeSpan _openTimeout;
    private DateTime _lastFailureTime;

    public CircuitBreaker(int failureThreshold, TimeSpan openTimeout)
    {
        _failureThreshold = failureThreshold;
        _openTimeout = openTimeout;
    }

    public bool CanAttempt()
    {
        if (_failureCount < _failureThreshold)
            return true;

        if (DateTime.UtcNow - _lastFailureTime > _openTimeout)
        {
            _failureCount = 0;
            return true;
        }

        return false;
    }

    public void RecordFailure()
    {
        _failureCount++;
        _lastFailureTime = DateTime.UtcNow;
    }

    public void Reset()
    {
        _failureCount = 0;
    }
}

