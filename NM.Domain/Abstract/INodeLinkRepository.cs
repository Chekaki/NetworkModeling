using System.Linq;
using NM.Domain.Entities;

namespace NM.Domain.Abstract {
    public interface INodeLinkRepository {
        IQueryable<NodeLink> NodeLinks { get; }

        void SaveNodeLink(NodeLink nodeLink);
        NodeLink DeleteNodeLink(int id);
    }
}