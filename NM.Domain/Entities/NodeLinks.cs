using System.ComponentModel.DataAnnotations.Schema;

namespace NM.Domain.Entities {
    public class NodeLink {
        public int Id { get; set; }
        public int FirstNodeId { get; set; }
        public int SecondNodeId { get; set; }
        public int Weight { get; set; }
        [NotMapped]
        public bool IsEdit { get; set; }

    }
}