export class InputValidator {
    constructor(urlValidator) {
        this.validNodeMap = new Map();
        this.nodeUrlCount = new Map();
        this.nodeUrlMap = new Map();
        this.inputNodes = [];
        this.urlValidator = urlValidator;
    }
    AddNode(node) {
        // push node to inputNodes
        // add node to validNodeMap: false, empty
        // add node url to nodeUrlCount
        // add node url to nodeUrlMap
        throw new Error("Method not implemented.");
    }
    RemoveNode() {
        // pop node from inputNodes and remove from maps(update maps)
        throw new Error("Method not implemented.");
    }
    CheckNode(node) {
        // check node is valid, update valid nodeMap regarding status 
        throw new Error("Method not implemented.");
    }
    AllNodesAreValid() {
        // check valid node map All bool true
        throw new Error("Method not implemented.");
    }
    GetValidNodeReason(node) {
        throw new Error("Method not implemented.");
    }
    GetNodeInputText(inputNode) {
        return inputNode.value;
    }
    GetInputNodeFrom(containingNode) {
        return containingNode.querySelector('input');
    }
}
//# sourceMappingURL=InputValidator.js.map