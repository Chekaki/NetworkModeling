using System.Linq;
using NM.Domain.Entities;

namespace NM.Domain.Abstract {
    public interface INodeRepository {
        IQueryable<Node> Nodes { get; }

        void SaveNode(Node node);
        Node DeleteNode(int id);
    }
}