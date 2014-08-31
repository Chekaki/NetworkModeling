function LoadNodeLinks() {
    var model = this;
    var nodes = model.nodes;
    var links = model.links;
    var editNode = model.node;
    var nodesWay = [];
    var removeIdLinks = model.removeIdLinks;

    function refreshLinks() {
        $('#nodeLinks input[type=checkbox]').unbind("click");
        $('#nodeLinks input[type=text]').bind("change");
        $("#nodeLinks").empty();
        model.nodes.forEach(function (node) {
            nodesWay = [];
            if (node.Id != editNode.Id) {
                var linkNode;
                for (var i = 0; i < links.length; i++) {
                    var link = links[i];
                    if (containsNodeLinkIds(link, node.Id, editNode.Id)) {
                        linkNode = link;
                        break;
                    }
                }

                var linkId = model.linkId + node.Id;
                if (linkNode !== undefined) {                 
                    var templateSelect = _.template($('#nm-nodeLinks-select').html().trim(), { node: node, link: linkNode, linkId: linkId });
                    $('#nodeLinks').append(templateSelect);

                } else if (searchNodeLinks(editNode.Id, node.Id)) {                  
                    var templateHasLink = _.template($('#nm-nodeLinks-hasLink').html().trim(), { node: node, nodeWay: nodesWay[nodesWay.length - 1], linkId: linkId });
                    $('#nodeLinks').append(templateHasLink);

                } else {                   
                    var templateClear = _.template($('#nm-nodeLinks').html().trim(), { node: node, linkId: linkId });
                    $('#nodeLinks').append(templateClear);
                }
            }
        });
        $('#nodeLinks input[type=checkbox]').bind("click", linkCheckboxClick);
        $('#nodeLinks input[type=text]').bind("change", linkWeightChange);
    } 

    refreshLinks();
    function searchNodeLinks(nodeId, searchNodeId) {
        for (var i = 0; i < links.length; i++) {
            var link = links[i];
            if (containsNodeLinkId(link, nodeId)) {
                if (searchNodeLink("", nodeId, searchNodeId)) {

                    return true;
                }
            }
        }
        return false;
    }

    function searchNodeLink(link, nodeId, searchNodeId) {
        if (link !== "" && containsNodeLinkIds(link, nodeId, searchNodeId)) return true;
        for (var i = 0; i < links.length; i++) {
            var localLink = links[i];
            if (localLink === link) continue;
            if (containsNodeLinkId(localLink, nodeId)) {
                if (containsNodeLinkId(localLink, searchNodeId)) return true;
                else {
                    if (searchNodeLink(localLink, takeAnotherNodeId(localLink, nodeId), searchNodeId)) {
                        nodesWay.push(getNodeNameFromId(takeAnotherNodeId(localLink, nodeId)));
                        return true;
                    }
                }
            }
        }
        return false;
    }

    function takeAnotherNodeId(link, idNode) {
        if (link.FirstNodeId == idNode) return link.SecondNodeId;
        else return link.FirstNodeId;
    }

    function containsNodeLinkIds(link, idF, idS) {
        if (link.FirstNodeId == idF && link.SecondNodeId == idS) 
            return true;
        else if (link.FirstNodeId == idS && link.SecondNodeId == idF)
            return true;
        return false;
    }

    function containsNodeLinkId(link, id) {
        if (link.FirstNodeId == id || link.SecondNodeId == id)
            return true;
        else
            return false;
    }
    
    function constainsNodeLinkIdsFromLinks(idF, idS) {
        for (var i = 0; i < model.links.length; i++) {
            var link = model.links[i];
            if (containsNodeLinkIds(link, idF, idS)) {
                return true;
            }
        }
        return false;
    }

    function getNodeNameFromId(id) {
        var name;
        for (var i = 0; i < model.nodes.length; i++) {
            var nodeLocal = model.nodes[i];
            if (nodeLocal !== null && nodeLocal !== undefined && nodeLocal.Id == id) {
                name = nodeLocal.Name;
                break;
            }
        }
        return name;
    }

    function getLinkObjIndIdFromLinkIds(idF, idS) {
        var linkIndexAndId = [];
        for (var i = 0; i < model.links.length; i++) {
            var linkLocal = model.links[i];
            if (containsNodeLinkIds(linkLocal, idF, idS)) {
                linkIndexAndId.push(i);
                linkIndexAndId.push(linkLocal.Id);
                break;
            }
        };
        return linkIndexAndId;
    }

    function getLinkFromLinkIds(idF, idS) {
        var link;
        for (var i = 0; i < model.links.length; i++) {
            var linkLocal = model.links[i];
            if (containsNodeLinkIds(linkLocal, idF, idS)) {
                link = linkLocal;
                break;
            }
        };
        return link;
    }

    function linkCheckboxClick (event) {
        var isChecked = event.target.checked;

        var linkUiId = event.target.parentElement.parentElement.id;
        var re = /nf(\d*)ns(\d*)/g;
        var ids = re.exec(linkUiId);

        if (isChecked) {
            if (!constainsNodeLinkIdsFromLinks(ids[1], ids[2])) {
                var newLink = { Id: "0", FirstNodeId: ids[1], SecondNodeId: ids[2], Weight: "10", IsEdit: true };
                links.push(newLink);
            }
        } else {
            if (constainsNodeLinkIdsFromLinks(ids[1], ids[2])) {
                var linkRemoveObjLink = getLinkObjIndIdFromLinkIds(ids[1], ids[2]);
                links.splice(linkRemoveObjLink[0], 1);
                if (linkRemoveObjLink[1] != 0)
                    removeIdLinks.push(+linkRemoveObjLink[1]);
            }
        }

        refreshLinks();
    };

    function linkWeightChange(event) {
        var reWight = /^\d+$/;
        var value = event.target.value;
        var validateValue = reWight.test(value) && value != 0;
        if (validateValue) {
            var linkUiId = event.target.parentElement.parentElement.id;
            var re = /nf(\d*)ns(\d*)/g;
            var ids = re.exec(linkUiId);
            var link = getLinkFromLinkIds(ids[1], ids[2]);
            if (link === undefined) return;

            link.Weight = value;
            link.IsEdit = true;
        } else {
        
        }
    }

    $("#nodename input[type=text]").change(function(event) {
        var reName = RegExp("^([A-Za-zА-Я-а-я0-9_ ]*)$", "g");
        var value = event.target.value;
        var validateValue = reName.test(value) && value != "";
        if (validateValue) {
            var isDuplicate = false;
            for (var i = 0; i < nodes.length; i++) {
                var node = nodes[i];
                if (node.Id != editNode.Id && node.Name == value) {
                    isDuplicate = true;
                    break;
                }
            };
            if (isDuplicate) {

            } else {
                editNode.Name = value;
            }
        } else {

        }
    });

    $("#formNodeEdit").validate({
        rules: {
            name: {
                legalName: true,
                required: true,
                maxlength: 10
            },
            weight: {
                legalWeight: true,
                required: true,
                maxlength: 10
            }
        },
    });
    $.validator.addMethod("legalName",
        function (value, element) {
            var reName = RegExp("^([A-Za-zА-Я-а-я0-9_ ]*)$", "g");
            var validateValue = reName.test(value) && value != "";
            var isDuplicate = false;
            if (validateValue) {
                for (var i = 0; i < nodes.length; i++) {
                    var node = nodes[i];
                    if (node.Id != editNode.Id && node.Name == value) {
                        isDuplicate = true;
                        break;
                    }
                };
            }
            return validateValue && !isDuplicate;
        },
        "Введите коректное имя");
    $.validator.addMethod("legalWeight",
    function (value, element) {
        var reWight = /^\d+$/;
        var validateValue = reWight.test(value) && value != 0;
        return validateValue && value !==0;
    },
    "Введите коректную длину");
}

function save() {

    if ($("#formNodeEdit input.error").length > 0) return;

    var model = this;
    if (model.Name == null) return;
    $.ajax({
        url: model.GlobalAppPath + "Node/SaveNode",
        type: "POST",
        data: {
            node: model.node,
            links: model.links,
            removeIdLinks: model.removeIdLinks
        },
        success: function (data) {
            window.location.href = data.redirectTo;
        }
    });
}