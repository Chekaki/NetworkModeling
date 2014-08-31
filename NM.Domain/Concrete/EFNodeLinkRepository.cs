using System.Linq;
using NM.Domain.Abstract;
using NM.Domain.Entities;

namespace NM.Domain.Concrete {
    public class EFNodeLinkRepository : EFRepository, INodeLinkRepository {
        public IQueryable<NodeLink> NodeLinks {
            get { return Context.NodeLinks; }
        }

        public void SaveNodeLink(NodeLink nodeLink)
        {
            if (nodeLink.Id == 0)
            {
                Context.NodeLinks.Add(nodeLink);
            }
            else
            {
                var dbEntry = Context.NodeLinks.Find(nodeLink.Id);
                if (dbEntry != null)
                {
                    dbEntry.FirstNodeId = nodeLink.FirstNodeId;
                    dbEntry.SecondNodeId = nodeLink.SecondNodeId;
                    dbEntry.Weight = nodeLink.Weight;
                }
            }
            Context.SaveChanges();
        }

        public NodeLink DeleteNodeLink(int id) {
            var dbEntry = Context.NodeLinks.Find(id);
            if (dbEntry != null) {
                Context.NodeLinks.Remove(dbEntry);
                Context.SaveChanges();
            }
            return dbEntry;
        }
    }
}