import { InputManager } from "./InputManager.js";
import { ValidateUrls } from "./ValidateUrls.js";
import { LinkRequestValidator } from "./LinkRequestValidator.js";
class DownloadPageManager {
    constructor(inputManager, inputValidator) {
        this.nodes = [];
        this.nodeMap = new Map();
        this.inputManager = inputManager;
        this.inputValidator = inputValidator;
        this.inputValidator = inputValidator;
        this.submitButton = document.getElementById("submit-button");
        let firstInput = document.getElementById("input1");
        this.nodes.push(firstInput); // first node
        firstInput.addEventListener('change', () => this.Validate(firstInput));
        this.nodeMap.set(firstInput, false);
        this.SetUpButtons();
    }
    SetUpButtons() {
        let addButton = document.getElementById("add-button");
        let removeButton = document.getElementById("remove-button");
        addButton.addEventListener('click', () => this.addNewInputNode());
        removeButton.addEventListener('click', () => this.removeInputNode());
    }
    removeInputNode() {
        if (this.inputManager.removeInput()) {
            let removedNode = this.nodes.pop(); // side-effect on DOM
            this.nodeMap.delete(removedNode);
        }
    }
    addNewInputNode() {
        let newNode = this.inputManager.insertInput('p'); // side-effect on DOM
        if (newNode !== null) {
            this.nodeMap.set(newNode, false);
            let inputNode = newNode.childNodes[1];
            inputNode.addEventListener('change', () => this.Validate(inputNode));
            this.nodes.push(inputNode);
        }
    }
    async Validate(node) {
        if (await this.inputValidator.UrlIsValidInInput(node)) {
            this.nodeMap.set(node, true);
            if (this.AllNodesAreValid(this.nodeMap)) {
                this.submitButton.disabled = false;
            }
            else {
                this.submitButton.disabled = true;
            }
        }
        else {
            this.nodeMap.set(node, false);
            this.submitButton.disabled = true;
        }
    }
    AllNodesAreValid(nodeMap) {
        let numNodes = this.nodes.length;
        let numValidatedNodes = 0;
        nodeMap.forEach((nodeIsValid, node) => {
            if (nodeIsValid)
                numValidatedNodes++;
        });
        return numNodes === numValidatedNodes;
    }
}
let inputChangeManager = new InputManager(document.getElementById("main-form"), 3);
let linkRequestValidator = new LinkRequestValidator();
let validateUrls = new ValidateUrls(linkRequestValidator);
let pageManager = new DownloadPageManager(inputChangeManager, validateUrls);
//# sourceMappingURL=Download.js.map