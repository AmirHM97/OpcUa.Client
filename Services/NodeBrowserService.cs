namespace OpcUa.Client.Services;

public class NodeBrowserService : INodeBrowserService
{
    public async Task<NodeId> FindFolderNodeIdAsync(Session session, NodeId parentNodeId, string folderName)
    {
        return await RecursiveFindFolderNodeIdAsync(session, parentNodeId, folderName);
    }

    public async Task<List<NodeId>> GetVariableNodeIdsAsync(Session session, NodeId folderNodeId)
    {
        var variableNodeIds = new List<NodeId>();
        var references = await BrowseAsync(session, folderNodeId, NodeClass.Variable);

        foreach (var reference in references)
        {
            if (reference.NodeClass == NodeClass.Variable)
            {
                variableNodeIds.Add((NodeId)reference.NodeId);
            }
        }

        return variableNodeIds;
    }

    private async Task<ReferenceDescriptionCollection> BrowseAsync(Session session, NodeId nodeId, NodeClass nodeClass)
    {
        ReferenceDescriptionCollection references;
        byte[] continuationPoint;

        session.Browse(
            null,
            null,
            nodeId,
            0u,
            BrowseDirection.Forward,
            ReferenceTypeIds.HierarchicalReferences,
            true,
            (uint)nodeClass,
            out continuationPoint,
            out references);

        return references;
    }

    public async Task<NodeId> RecursiveFindFolderNodeIdAsync(Session session, NodeId parentNodeId, string targetFolderName)
    {
        var references = await BrowseAsync(session, parentNodeId, NodeClass.Object);

        foreach (var reference in references)
        {
            if (reference.DisplayName.Text == targetFolderName)
            {
                return (NodeId)reference.NodeId;
            }

            var subFolderNodeId = (NodeId)reference.NodeId;
            var resultNodeId = await RecursiveFindFolderNodeIdAsync(session, subFolderNodeId, targetFolderName);
            if (resultNodeId != null)
            {
                return resultNodeId;
            }
        }

        return null;
    }
}
