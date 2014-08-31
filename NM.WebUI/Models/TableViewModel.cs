using System.Collections;
using System.Collections.Generic;
using NM.Domain.Entities;

namespace NM.WebUI.Models {
    public class TableViewModel {
        public IEnumerable<Node> Nodes { get; set; }
        public IEnumerable<NodeLink> NodeLinks { get; set; }
    }
}