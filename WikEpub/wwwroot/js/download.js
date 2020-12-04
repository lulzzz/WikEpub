import { InputManager } from "./InputManager.js";
import { ValidateUrls } from "./ValidateUrls.js";
import { LinkRequestValidator } from "./LinkRequestValidator.js";
class DownloadPageManager {
    constructor(inputManager, inputValidator) {
        this.nodes = [];
        this.validNodeMap = new Map();
        this.inputManager = inputManager;
        this.urlValidator = inputValidator;
        this.submitButton = document.getElementById("submit-button");
        let firstInput = document.getElementById("input1");
        this.AddNode(firstInput, this.validNodeMap, this.nodes);
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
            this.validNodeMap.delete(removedNode);
            this.CheckSubmitStatus();
        }
    }
    addNewInputNode() {
        let newNode = this.inputManager.insertInput('p'); // side-effect on DOM
        if (newNode !== null) {
            let inputElement = newNode.childNodes[1]; // get actual input element
            this.AddNode(inputElement, this.validNodeMap, this.nodes);
        }
    }
    AddNode(inputElement, validNodeMap, nodes) {
        validNodeMap.set(inputElement, false);
        nodes.push(inputElement);
        console.log(inputElement);
        inputElement.addEventListener('change', () => this.ValidateNode(inputElement));
        this.submitButton.disabled = true;
    }
    async ValidateNode(node) {
        if (await this.urlValidator.UrlIsValidInInput(node)) {
            this.validNodeMap.set(node, true);
        }
        else {
            this.validNodeMap.set(node, false);
        }
        this.CheckSubmitStatus();
    }
    CheckSubmitStatus() {
        if (this.AllNodesAreValidIn(this.validNodeMap)) {
            this.submitButton.disabled = false;
        }
        else {
            this.submitButton.disabled = true;
        }
    }
    AllNodesAreValidIn(nodeMap) {
        for (let [node, valid] of nodeMap)
            if (!valid)
                return false;
        return true;
    }
}
let inputChangeManager = new InputManager(document.getElementById("main-form"), 3);
let linkRequestValidator = new LinkRequestValidator();
let validateUrls = new ValidateUrls(linkRequestValidator);
let pageManager = new DownloadPageManager(inputChangeManager, validateUrls);
//# sourceMappingURL=Download.js.map