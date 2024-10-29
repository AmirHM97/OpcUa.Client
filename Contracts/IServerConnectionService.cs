namespace OPCUA.Client.Contracts;

public interface IServerConnectionService
{
    Task<Session> ConnectAsync();
}