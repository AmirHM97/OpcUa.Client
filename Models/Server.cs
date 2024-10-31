namespace OpcUa.Client.Models;

public class Server
{
    public string Url{ get; set; }
    public string FolderName{ get; set; }
}

public class ServerConfiguration
{
    public List<Server> Servers { get; set; }
}
