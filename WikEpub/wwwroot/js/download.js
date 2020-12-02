import { InputManager } from "./InputManager.js";
class DownloadPageManager {
    constructor(inputManager) {
        this.nodes = [];
        this.nodeMap = new Map();
        this.inputManager = inputManager;
        this.inputValidator = this.inputValidator;
        this.nodes.push(document.getElementById("input-frame-1")); // first node
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
        console.log(this.nodes.length.toString());
    }
    addNewInputNode() {
        let newNode = this.inputManager.insertInput('p'); // side-effect on DOM
        if (newNode !== null) {
            this.nodes.push(newNode);
        }
        console.log(this.nodes.length.toString());
    }
}
let urlInputEventHandler = new InputManager(document.getElementById("main-form"), 3);
let pageManager = new DownloadPageManager(urlInputEventHandler);
//# sourceMappingURL=Download.js.map