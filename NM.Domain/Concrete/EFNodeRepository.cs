using System.Linq;
using NM.Domain.Abstract;
using NM.Domain.Entities;

namespace NM.Domain.Concrete {
    public class EFNodeRepository : EFRepository, INodeRepository {
        public IQueryable<Node> Nodes {
            get { return Context.Nodes; }
        }

        public void SaveNode(Node node)
        {
            if (node.Id == 0)
            {
                Context.Nodes.Add(node);
            }
            else
            {
                var dbEntity = Context.Nodes.Find(node.Id);
                if (dbEntity != null)
                {
                    dbEntity.Name = node.Name;
                }
            }
            Context.SaveChanges();
        }

        public Node DeleteNode(int id) {
            var dbEntry = Context.Nodes.Find(id);
            if (dbEntry != null) {
                Context.Nodes.Remove(dbEntry);
                Context.SaveChanges();
            }
            return dbEntry;
        }
    }
}