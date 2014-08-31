using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using NM.Domain.Entities;

namespace NM.WebUI {
    public static class Commons {
        public static string GetBaseUrl() {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (string.IsNullOrWhiteSpace(appUrl))
                appUrl = "/";

            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            if (!baseUrl.EndsWith("/"))
                baseUrl += "/";

            return baseUrl;
        }

        public static string GetLinkWeidthFromNodeIds(IEnumerable<NodeLink> links, int nodeId, int searchId) {
            var result = String.Empty;
            var localLinks = links.ToList();
            if (nodeId == searchId)
                result = "0";
            else {
                foreach (var link in localLinks.Where(l => ContainsNodeLinkId(l, nodeId))) {
                    var linkWeidht = SearchNodeLinkWeidht(localLinks, null, nodeId, searchId);
                    if (linkWeidht != null) {
                        result = linkWeidht.ToString();
                        break;
                    }
                }
            }
            return result;
        }

        public static bool ContainsNodeLinkIds(NodeLink link, int idF, int idS) {
            return (link.FirstNodeId == idF && link.SecondNodeId == idS) || (link.FirstNodeId == idS && link.SecondNodeId == idF);
        }

        public static int? SearchNodeLinkWeidht(IEnumerable<NodeLink> links, NodeLink link, int nodeId, int searchId) {
            var result = new int?();
            if (link!= null && ContainsNodeLinkIds(link, nodeId, searchId))
                return link.Weight;

            var localLinks = links.ToList();
            foreach (var localLink in localLinks.Where(l => ((link==null)||(link.Id!=l.Id)) && ContainsNodeLinkId(l, nodeId)))
            {
                if (ContainsNodeLinkId(localLink, searchId))
                    return localLink.Weight;

                var locResult = SearchNodeLinkWeidht(localLinks, localLink, TakeAnotherNodeId(localLink, nodeId), searchId);
                if (locResult != null) {
                    if (result == null)
                        result = 0;
                    result += locResult + localLink.Weight;
                    break;
                }
            }
            return result;
        }

        public static int TakeAnotherNodeId(NodeLink link, int nodeId) {
            if (link.FirstNodeId == nodeId)
                return link.SecondNodeId;
            else if (link.SecondNodeId == nodeId)
                return link.FirstNodeId;
            else {
                throw new Exception();
            }
        }

        public static bool ContainsNodeLinkId(NodeLink link, int id) {
            return (link.FirstNodeId == id) || (link.SecondNodeId == id);
        }
    }
}