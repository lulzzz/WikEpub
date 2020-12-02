import { InputManager } from "./InputManager.js";
import { ValidateUrls } from "./ValidateUrls.js";
import { IManageInputs } from "./Interfaces/IManageInputs"
import { ILinkRequestValidator } from "./Interfaces/ILinkRequestValidator";

class DownloadPageManager {
    inputManager: IManageInputs;
    inputValidator: ILinkRequestValidator; 
    nodes: Node[];
    nodeMap: Map<Node, boolean>

    constructor(inputManager: IManageInputs) {
        this.nodes = [];
        this.nodeMap = new Map();
        this.inputManager = inputManager;
        this.inputValidator = this.inputValidator;
        this.nodes.push(document.getElementById("input-frame-1")); // first node
        this.SetUpButtons();
    }

    private SetUpButtons(): void{
        let addButton = document.getElementById("add-button");
        let removeButton = document.getElementById("remove-button");
        addButton.addEventListener('click',() => this.addNewInputNode());
        removeButton.addEventListener('click', () => this.removeInputNode());

    }

    private removeInputNode() {
        if (this.inputManager.removeInput())
            this.nodes.pop(); // side-effect on DOM
        console.log(this.nodes.length.toString());
    }

    private addNewInputNode() {
        let newNode = this.inputManager.insertInput('p'); // side-effect on DOM
        if (newNode !== null) {
            this.nodes.push(newNode);
        }
        console.log(this.nodes.length.toString());
    }
}

let urlInputEventHandler: InputManager = new InputManager(document.getElementById("main-form"), 3);

let pageManager = new DownloadPageManager(urlInputEventHandler);


 





