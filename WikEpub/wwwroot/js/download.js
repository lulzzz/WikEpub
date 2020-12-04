import { InputManager } from "./InputManager.js";
import { ValidateUrls } from "./ValidateUrls.js";
import { LinkRequestValidator } from "./LinkRequestValidator.js";
class DownloadPageManager {
    constructor(inputManager, inputValidator) {
        this.nodes = [];
        this.nodeMap = new Map();
        this.inputManager = inputManager;
        this.urlValidator = inputValidator;
        this.submitButton = document.getElementById("submit-button");
        let firstInput = document.getElementById("input1");
        this.nodes.push(firstInput); // first node
        firstInput.addEventListener('change', () => this.ValidateNode(firstInput));
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
            if (this.AllNodesAreValid(this.nodeMap))
                this.submitButton.disabled = false;
        }
    }
    addNewInputNode() {
        let newNode = this.inputManager.insertInput('p'); // side-effect on DOM
        if (newNode !== null) {
            let inputNode = newNode.childNodes[1];
            this.nodeMap.set(inputNode, false);
            inputNode.addEventListener('change', () => this.ValidateNode(inputNode));
            this.nodes.push(inputNode);
            this.submitButton.disabled = true;
        }
    }
    async ValidateNode(node) {
        if (await this.urlValidator.UrlIsValidInInput(node)) {
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
    // this can be changed: the method will return false if any node is not valid, otherwise true
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