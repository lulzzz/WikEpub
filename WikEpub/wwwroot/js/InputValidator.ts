import { IInputValidator } from "./Interfaces/IInputValidator";
import { ValidNodeReason } from "./ValidNodeReasonEnum.js";
import { IValidateUrls } from "./Interfaces/IValidateUrls";

export class InputValidator implements IInputValidator {
    private urlCount: Map<string, number>;
    private nodeUrlMap: Map<Node, string>;
    private validNodeMap: Map<Node, [isValid: boolean, reason: ValidNodeReason]>;
    private inputNodes: Node[];
    private urlValidator: IValidateUrls;

    constructor(urlValidator: IValidateUrls) {
        this.validNodeMap = new Map<Node, [isValid: boolean, reason: ValidNodeReason]>();
        this.urlCount = new Map<string, number>();
        this.nodeUrlMap = new Map<Node, string>();
        this.inputNodes = [];
        this.urlValidator = urlValidator;
    }
    AddNode(node: Node): void {
        let addedInputNode = this.GetInputNodeFrom(node);
        this.inputNodes.push(addedInputNode);
        this.validNodeMap.set(addedInputNode, [false, ValidNodeReason.Empty])
        this.nodeUrlMap.set(addedInputNode, this.GetNodeInputText(addedInputNode));
        this.AddNodeToUrlCountMap(addedInputNode, this.urlCount);
    }
    RemoveNode(): void {
        let removedNode = this.inputNodes.pop();
        this.validNodeMap.delete(removedNode);
        this.nodeUrlMap.delete(removedNode)
        this.RemoveUrlFromCountMap(this.GetNodeInputText(removedNode), this.urlCount);
    }
    async CheckNodeOnChange(node: Node): Promise<void>{
        this.UpdateNodeMaps(node, this.urlCount, this.nodeUrlMap)
        let containsDuplicateUrl = this.urlCount.get(this.GetNodeInputText(node)) > 1;
        let isValidUrl = await this.urlValidator.UrlIsValidInInput
        let inputText = this.GetNodeInputText(node);
        if (inputText.length === 0 || inputText === null)
            this.validNodeMap.set(node, [false, ValidNodeReason.Empty])
        else if (containsDuplicateUrl && isValidUrl) 
            this.validNodeMap.set(node, [false, ValidNodeReason.Duplicate])
        else if (!isValidUrl)
            this.validNodeMap.set(node, [false, ValidNodeReason.InvalidUrl])
        else
            this.validNodeMap.set(node, [true, ValidNodeReason.Valid])
    }
    AllNodesAreValid(): boolean {
        for (let [node, [valid, reason]] of this.validNodeMap) 
            if (!valid) return false;
        return true;
    }
    GetValidNodeReasons(): [Node, boolean, string][] {
        return this.inputNodes.map(node => {
            let [isValid, reason] = this.GetValidNodeReason(node);
            return [node, isValid, reason] 
        });
    }
    private GetValidNodeReason(node: Node): [boolean, string] {
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
                reasonString = "All good"
                break;
        }
        return [isValid, reasonString];
    }
    private GetNodeInputText(inputNode: Node): string {
        return (inputNode as HTMLInputElement).value;
    }
    private GetInputNodeFrom(containingNode: Node): Node {
        return (containingNode as HTMLElement).querySelector('input');
    }
    private AddNodeToUrlCountMap(node: Node, urlCount: Map<string, number>) : void {
        let inputText = this.GetNodeInputText(node);
        if (urlCount.has(inputText)) {
            let previousValue = urlCount.get(inputText);
            urlCount.set(inputText, previousValue += 1);
        }
        else urlCount.set(inputText, 1)
    }
    private RemoveUrlFromCountMap(nodeInputValue: string, urlCount: Map<string, number>): void {
        if (!urlCount.has(nodeInputValue)) return;
        let inputTextValue = urlCount.get(nodeInputValue);
        if (inputTextValue == 1) {
            urlCount.delete(nodeInputValue);
        }
        else {
            urlCount.set(nodeInputValue, inputTextValue -= 1);
        }
    }
    private UpdateNodeMaps(node: Node, urlCountMap: Map<string, number>, nodeUrlMap: Map<Node, string>) {
        let inputText = this.GetNodeInputText(node);
        if (nodeUrlMap.get(node) === inputText) return; // if there is no change do nothing
        // otherwise: 
        // add new url to counter 
        this.AddNodeToUrlCountMap(node, urlCountMap);
        // remove old value from counter
        this.RemoveUrlFromCountMap(nodeUrlMap.get(node), urlCountMap)
        // update urlMap with new value
        nodeUrlMap.set(node, inputText);
    }

}

