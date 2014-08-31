using System.Collections.Generic;
using NM.Domain.Entities;

namespace NM.WebUI.Models {
    public class NodeEditViewModel {
        public Node Node { get; set; }
        public IEnumerable<Node> Nodes { get; set; } 
        public IEnumerable<NodeLink> NodeLinks { get; set; }
    }
}