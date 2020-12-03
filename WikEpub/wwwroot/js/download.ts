import { InputManager } from "./InputManager.js";
import { IManageInputs } from "./Interfaces/IManageInputs"
import { ValidateUrls } from "./ValidateUrls.js";
import { IValidateUrls } from "./Interfaces/IValidateUrls.js";
import { LinkRequestValidator } from "./LinkRequestValidator.js";
import { ILinkRequestValidator } from "./Interfaces/ILinkRequestValidator";

class DownloadPageManager {
    private inputManager: IManageInputs;
    private inputValidator: IValidateUrls;
    private nodes: Node[];
    private nodeMap: Map<Node, boolean>;
    private submitButton: HTMLInputElement;

    constructor(inputManager: IManageInputs, inputValidator: IValidateUrls) {
        this.nodes = [];
        this.nodeMap = new Map();
        this.inputManager = inputManager;
        this.inputValidator = inputValidator;
        this.inputValidator = inputValidator;
        this.submitButton = <HTMLInputElement>document.getElementById("submit-button");
        let firstInput = document.getElementById("input1");
        this.nodes.push(firstInput); // first node
        firstInput.addEventListener('change', () => this.Validate(firstInput))
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
        }
    }

    private addNewInputNode() {
        let newNode = this.inputManager.insertInput('p'); // side-effect on DOM
        if (newNode !== null) {
            this.nodeMap.set(newNode, false);
            let inputNode = newNode.childNodes[1];
            inputNode.addEventListener('change', () => this.Validate(inputNode));
            this.nodes.push(inputNode);
        }
    }

    private async Validate(node: Node) {
        if (await this.inputValidator.UrlIsValidInInput(node)) {
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