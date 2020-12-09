import { IInputValidator } from "./Interfaces/IInputValidator";
import { ValidNodeReason } from "./ValidNodeReasonEnum";
import { IValidateUrls } from "./Interfaces/IValidateUrls";

export class InputValidator implements IInputValidator {
    private nodeUrlCount: Map<string, number>;
    private validNodeMap: Map<Node, [isValid: boolean, reason: ValidNodeReason]>;
    private nodeUrlMap: Map<Node, string>;
    private inputNodes: Node[];
    private urlValidator: IValidateUrls;

    constructor(urlValidator: IValidateUrls) {
        this.validNodeMap = new Map<Node, [isValid: boolean, reason: ValidNodeReason]>();
        this.nodeUrlCount = new Map<string, number>();
        this.nodeUrlMap = new Map<Node, string>();
        this.inputNodes = [];
        this.urlValidator = urlValidator;
    }
    AddNode(node: Node): void {
        // push node to inputNodes
        // add node to validNodeMap: false, empty
        // add node url to nodeUrlCount
        // add node url to nodeUrlMap
        throw new Error("Method not implemented.");
    }
    RemoveNode(): void {
        // pop node from inputNodes and remove from maps(update maps)
        throw new Error("Method not implemented.");
    }
    CheckNode(node: Node): boolean {
        // check node is valid, update valid nodeMap regarding status 
        throw new Error("Method not implemented.");
    }
    AllNodesAreValid(): boolean {
        // check valid node map All bool true
        throw new Error("Method not implemented.");
    }
    GetValidNodeReason(node: Node): [boolean, string] {
        throw new Error("Method not implemented.");
    }
    private GetNodeInputText(inputNode: Node): string {
        return (inputNode as HTMLInputElement).value;
    }
    private GetInputNodeFrom(containingNode: Node): Node {
        return (containingNode as HTMLElement).querySelector('input');
    }
    

}

