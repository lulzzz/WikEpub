import { InputManager } from "./InputManager.js";
import { IManageInputs } from "./Interfaces/IManageInputs"
import { ValidateUrls } from "./ValidateUrls.js";
import { IValidateUrls } from "./Interfaces/IValidateUrls.js";
import { LinkRequestValidator } from "./LinkRequestValidator.js";
import { ILinkRequestValidator } from "./Interfaces/ILinkRequestValidator";

class DownloadPageManager {
    private inputManager: IManageInputs;
    private urlValidator: IValidateUrls;
    private nodes: Node[];
    private nodeMap: Map<Node, boolean>;
    private submitButton: HTMLInputElement;
   
    constructor(inputManager: IManageInputs, inputValidator: IValidateUrls) {
        this.nodes = [];
        this.nodeMap = new Map();
        this.inputManager = inputManager;
        this.urlValidator = inputValidator;
        this.submitButton = <HTMLInputElement>document.getElementById("submit-button");
        let firstInput = document.getElementById("input1");
        this.nodes.push(firstInput); // first node
        firstInput.addEventListener('change', () => this.ValidateNode(firstInput))
        this.nodeMap.set(firstInput, false);
        this.SetUpButtons();
    }

    private SetUpButtons(): void {
        let addButton = document.getElementById("add-button");
        let removeButton = document.getElementById("remove-button");
        addButton.addEventListener('click', () => this.addNewInputNode());
        removeButton.addEventListener('click', () => this.removeInputNode());
    }

    private removeInputNode() {
        if (this.inputManager.removeInput()) {
            let removedNode = this.nodes.pop(); // side-effect on DOM
            this.nodeMap.delete(removedNode);
            if (this.AllNodesAreValid(this.nodeMap)) this.submitButton.disabled = false;
        }
    }

    private addNewInputNode() {
        let newNode = this.inputManager.insertInput('p'); // side-effect on DOM
        if (newNode !== null) {
            let inputNode = newNode.childNodes[1];
            this.nodeMap.set(inputNode, false);
            inputNode.addEventListener('change', () => this.ValidateNode(inputNode));
            this.nodes.push(inputNode);
            this.submitButton.disabled = true;
        }
    }

    private async ValidateNode(node: Node): Promise<void>{
        if (await this.urlValidator.UrlIsValidInInput(node)) {
            this.nodeMap.set(node, true);
            if (this.AllNodesAreValid(this.nodeMap)) {
                this.submitButton.disabled = false;
            } else {
                this.submitButton.disabled = true;
            }
        } else {
            this.nodeMap.set(node, false);
            this.submitButton.disabled = true;
        }
    }

    // this can be changed: the method will return false if any node is not valid, otherwise true
    private AllNodesAreValid(nodeMap: Map<Node, boolean>): boolean {
        let numNodes = this.nodes.length;
        let numValidatedNodes = 0;
        nodeMap.forEach((nodeIsValid, node) => {
            if (nodeIsValid) numValidatedNodes++;
        });
        return numNodes === numValidatedNodes;
    }
}

let inputChangeManager: InputManager = new InputManager(document.getElementById("main-form"), 3);
let linkRequestValidator = new LinkRequestValidator();
let validateUrls = new ValidateUrls(linkRequestValidator);
let pageManager = new DownloadPageManager(inputChangeManager, validateUrls);