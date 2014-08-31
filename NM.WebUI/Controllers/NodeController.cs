using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using NM.Domain.Abstract;
using NM.Domain.Entities;
using NM.WebUI.Models;

namespace NM.WebUI.Controllers {
    public class NodeController : Controller {
        private readonly INodeLinkRepository _nodeLinkRepository;
        private readonly INodeRepository _nodeRepository;

        public NodeController(INodeRepository nodeRepository, INodeLinkRepository nodeLinkRepository) {
            _nodeRepository = nodeRepository;
            _nodeLinkRepository = nodeLinkRepository;
        }

        // GET: Node
        public ViewResult NodeList() {
            return View(_nodeRepository.Nodes);
        }

        public ViewResult Edit(int id) {
            var node = _nodeRepository.Nodes.FirstOrDefault(n => n.Id == id) ?? new Node();
            var nodes = _nodeRepository.Nodes;
            var nodelLinks = _nodeLinkRepository.NodeLinks;

            return View(new NodeEditViewModel {Node = node, Nodes = nodes, NodeLinks = nodelLinks});
        }

        [HttpPost]
        public ActionResult Edit(NodeEditViewModel editViewModel) {
            if (ModelState.IsValid) {
                _nodeRepository.SaveNode(editViewModel.Node);
                TempData["message"] = string.Format("Узел {0} был сохранен", editViewModel.Node.Name);
                return RedirectToAction("NodeList");
            }
            return View(editViewModel);
        }

        [HttpPost]
        public ActionResult SaveNode(Node node, IEnumerable<NodeLink> links, IEnumerable<int> removeIdLinks) {
            var savedNode = _nodeRepository.SaveNode(node);
            if (removeIdLinks != null)
                foreach (var removeIdLink in removeIdLinks) {
                    _nodeLinkRepository.DeleteNodeLink(removeIdLink);
                }
            if (links != null)
                foreach (var nodeLink in links.Where(l => l.IsEdit)) {
                    if (nodeLink.FirstNodeId == 0) nodeLink.FirstNodeId = savedNode.Id;
                    _nodeLinkRepository.SaveNodeLink(nodeLink);
                }

            TempData["message"] = string.Format("Узел {0} был сохранен", node.Name);
            return Json(new { redirectTo = Url.Action("NodeList") });
        }

        public ViewResult Create() {
            var nodes = _nodeRepository.Nodes;
            var nodeLinks = _nodeLinkRepository.NodeLinks;
            return View("Edit", new NodeEditViewModel { Node = new Node(), Nodes = nodes, NodeLinks = nodeLinks });
        }

        public ActionResult Delete(int id) {
            var deletedNode = _nodeRepository.DeleteNode(id);
            if (deletedNode != null) {
                TempData["message"] = string.Format("Узел {0} и все его связи были удалены",
                                                    deletedNode.Name);
            }
            return RedirectToAction("NodeList");
        }

        public ActionResult Table() {
            var nodes = _nodeRepository.Nodes;
            var nodeLinks = _nodeLinkRepository.NodeLinks;
            return View(new TableViewModel { Nodes = nodes, NodeLinks = nodeLinks});
        }
    }
}