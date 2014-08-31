using System.Linq;
using NM.Domain.Entities;

namespace NM.Domain.Abstract {
    public interface INodeRepository {
        IQueryable<Node> Nodes { get; }

        Node SaveNode(Node node);
        Node DeleteNode(int id);
    }
}