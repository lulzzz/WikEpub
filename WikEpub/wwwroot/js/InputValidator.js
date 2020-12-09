import { ValidNodeReason } from "./ValidNodeReasonEnum.js";
export class InputValidator {
    constructor(urlValidator) {
        this.validNodeMap = new Map();
        this.urlCount = new Map();
        this.nodeUrlMap = new Map();
        this.inputNodes = [];
        this.urlValidator = urlValidator;
    }
    AddNode(node) {
        let addedInputNode = this.GetInputNodeFrom(node);
        this.inputNodes.push(addedInputNode);
        this.validNodeMap.set(addedInputNode, [false, ValidNodeReason.Empty]);
        this.nodeUrlMap.set(addedInputNode, this.GetNodeInputText(addedInputNode));
        this.AddNodeToUrlCountMap(addedInputNode, this.urlCount);
    }
    RemoveNode() {
        let removedNode = this.inputNodes.pop();
        this.validNodeMap.delete(removedNode);
        this.nodeUrlMap.delete(removedNode);
        this.RemoveUrlFromCountMap(this.GetNodeInputText(removedNode), this.urlCount);
    }
    async CheckNodeOnChange(node) {
        this.UpdateNodeMaps(node, this.urlCount, this.nodeUrlMap);
        let containsDuplicateUrl = this.urlCount.get(this.GetNodeInputText(node)) > 1;
        let isValidUrl = await this.urlValidator.UrlIsValidInInput;
        let inputText = this.GetNodeInputText(node);
        if (inputText.length === 0 || inputText === null)
            this.validNodeMap.set(node, [false, ValidNodeReason.Empty]);
        else if (containsDuplicateUrl && isValidUrl)
            this.validNodeMap.set(node, [false, ValidNodeReason.Duplicate]);
        else if (!isValidUrl)
            this.validNodeMap.set(node, [false, ValidNodeReason.InvalidUrl]);
        else
            this.validNodeMap.set(node, [true, ValidNodeReason.Valid]);
    }
    AllNodesAreValid() {
        for (let [node, [valid, reason]] of this.validNodeMap)
            if (!valid)
                return false;
        return true;
    }
    GetValidNodeReasons() {
        return this.inputNodes.map(node => {
            let [isValid, reason] = this.GetValidNodeReason(node);
            return [node, isValid, reason];
        });
    }
    GetValidNodeReason(node) {
        let [isValid, reason] = this.validNodeMap.get(node);
        let reasonString;
        switch (reason) {
            case ValidNodeReason.Empty:
                reasonString = "Enter a wikipedia link";
                break;
            case ValidNodeReason.Duplicate:
                reasonString = "Duplicate link";
                break;
            case ValidNodeReason.InvalidUrl:
                reasonString = "Url is Invalid";
                break;
            case ValidNodeReason.Valid:
                reasonString = "All good";
                break;
        }
        return [isValid, reasonString];
    }
    GetNodeInputText(inputNode) {
        return inputNode.value;
    }
    GetInputNodeFrom(containingNode) {
        return containingNode.querySelector('input');
    }
    AddNodeToUrlCountMap(node, urlCount) {
        let inputText = this.GetNodeInputText(node);
        if (urlCount.has(inputText)) {
            let previousValue = urlCount.get(inputText);
            urlCount.set(inputText, previousValue += 1);
        }
        else
            urlCount.set(inputText, 1);
    }
    RemoveUrlFromCountMap(nodeInputValue, urlCount) {
        if (!urlCount.has(nodeInputValue))
            return;
        let inputTextValue = urlCount.get(nodeInputValue);
        if (inputTextValue == 1) {
            urlCount.delete(nodeInputValue);
        }
        else {
            urlCount.set(nodeInputValue, inputTextValue -= 1);
        }
    }
    UpdateNodeMaps(node, urlCountMap, nodeUrlMap) {
        let inputText = this.GetNodeInputText(node);
        if (nodeUrlMap.get(node) === inputText)
            return; // if there is no change do nothing
        // otherwise: 
        // add new url to counter 
        this.AddNodeToUrlCountMap(node, urlCountMap);
        // remove old value from counter
        this.RemoveUrlFromCountMap(nodeUrlMap.get(node), urlCountMap);
        // update urlMap with new value
        nodeUrlMap.set(node, inputText);
    }
}
//# sourceMappingURL=InputValidator.js.map