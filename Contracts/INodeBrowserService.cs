namespace OpcUa.Client.Contracts;

public interface INodeBrowserService
{
    Task<NodeId> FindFolderNodeIdAsync(Session session, NodeId parentNodeId, string folderName);
    Task<List<NodeId>> GetVariableNodeIdsAsync(Session session, NodeId folderNodeId);
    Task<NodeId> RecursiveFindFolderNodeIdAsync(Session session, NodeId parentNodeId, string targetFolderName);
}