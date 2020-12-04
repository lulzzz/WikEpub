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
    private validNodeMap: Map<Node, boolean>;
    private submitButton: HTMLInputElement;
   
    constructor(inputManager: IManageInputs, inputValidator: IValidateUrls) {
        this.nodes = [];
        this.validNodeMap = new Map();
        this.inputManager = inputManager;
        this.urlValidator = inputValidator;
        this.submitButton = <HTMLInputElement>document.getElementById("submit-button");

        let firstInput = document.getElementById("input1");
        this.AddNode(firstInput, this.validNodeMap, this.nodes);
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
            this.validNodeMap.delete(removedNode);
            this.CheckSubmitStatus();
        }
    }

    private addNewInputNode() {
        let newNode = this.inputManager.insertInput('p'); // side-effect on DOM
        if (newNode !== null) {
            let inputElement = newNode.childNodes[1]; // get actual input element
            this.AddNode(inputElement, this.validNodeMap, this.nodes)
        }
    }

    private AddNode(inputElement: Node, validNodeMap: Map<Node, boolean>, nodes: Node[]) {
        validNodeMap.set(inputElement, false);
        nodes.push(inputElement);
        console.log(inputElement);
        inputElement.addEventListener('change', () => this.ValidateNode(inputElement));
        this.submitButton.disabled = true;
    }

    private async ValidateNode(node: Node): Promise<void>{
        if (await this.urlValidator.UrlIsValidInInput(node)) {
            this.validNodeMap.set(node, true);
        } else {
            this.validNodeMap.set(node, false);
        }
        this.CheckSubmitStatus();
    }

    private CheckSubmitStatus() {
        if (this.AllNodesAreValidIn(this.validNodeMap)) {
            this.submitButton.disabled = false;
        } else {
            this.submitButton.disabled = true;
        }
    }

    private AllNodesAreValidIn(nodeMap: Map<Node, boolean>): boolean {
        for (let [node, valid] of nodeMap)
            if (!valid) return false;
        return true;
    }
}

let inputChangeManager: InputManager = new InputManager(document.getElementById("main-form"), 3);
let linkRequestValidator = new LinkRequestValidator();
let validateUrls = new ValidateUrls(linkRequestValidator);
let pageManager = new DownloadPageManager(inputChangeManager, validateUrls);