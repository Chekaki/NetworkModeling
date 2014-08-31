using System.Data.Entity;
using NM.Domain.Entities;

namespace NM.Domain.Concrete {
    internal class EFDbContext : DbContext {
        public DbSet<Node> Nodes { get; set; }
        public DbSet<NodeLink> NodeLinks { get; set; }
    }
}