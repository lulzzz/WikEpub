import { InputManager } from "./InputManager.js";
class DownloadPageManager {
    constructor(inputManager, inputValidator) {
        this.nodes = [];
        this.nodeMap = new Map();
        this.inputManager = inputManager;
        this.inputValidator = inputValidator;
        this.inputValidator = inputValidator;
        let firstInput = document.getElementById("input1");
        this.nodes.push(firstInput); // first node
        firstInput.addEventListener('change', () => this.Validate(firstInput));
        this.SetUpButtons();
    }
    SetUpButtons() {
        let addButton = document.getElementById("add-button");
        let removeButton = document.getElementById("remove-button");
        addButton.addEventListener('click', () => this.addNewInputNode());
        removeButton.addEventListener('click', () => this.removeInputNode());
    }
    removeInputNode() {
        if (this.inputManager.removeInput())
            this.nodes.pop(); // side-effect on DOM
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
                // enable submit
            }
            else {
                //disable submit
            }
        }
        else {
            this.nodeMap.set(node, false);
            //disable submit
        }
        // validate input
        // if valid, check all others with false in map for validation + change ui
        // if all are valid -> enable accept button
    }
    AllNodesAreValid(nodeMap) {
        return true;
    }
}
let inputChangeManager = new InputManager(document.getElementById("main-form"), 3);
//let pageManager = new DownloadPageManager(inputChangeManager);
//# sourceMappingURL=Download.js.map