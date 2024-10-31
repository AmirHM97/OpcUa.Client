namespace OpcUa.Client.Contracts;

public interface IServerConnectionService
{
    Task<Session> ConnectAsync(string serverUrl);
}