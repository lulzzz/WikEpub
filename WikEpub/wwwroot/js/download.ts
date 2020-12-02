import { InputManager } from "./InputManager.js";
import { ValidateUrls } from "./ValidateUrls.js";
import { IManageInputs } from "./Interfaces/IManageInputs"
import { ILinkRequestValidator } from "./Interfaces/ILinkRequestValidator";
import { IValidateUrls } from "./Interfaces/IValidateUrls.js";

class DownloadPageManager {
    inputManager: IManageInputs;
    inputValidator: IValidateUrls; 
    nodes: Node[];
    nodeMap: Map<Node, boolean>

    constructor(inputManager: IManageInputs, inputValidator: IValidateUrls) {
        this.nodes = [];
        this.nodeMap = new Map();
        this.inputManager = inputManager;
        this.inputValidator = inputValidator;
        this.inputValidator = inputValidator;
        let firstInput = document.getElementById("input1");
        this.nodes.push(firstInput); // first node
        firstInput.addEventListener('change', () => this.Validate(firstInput))
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
            // enable submit
            } else {
            //disable submit
            }

        } else {
            this.nodeMap.set(node, false);
            //disable submit
        }
        // validate input
        // if valid, check all others with false in map for validation + change ui
        // if all are valid -> enable accept button
    }

    private AllNodesAreValid(nodeMap: Map<Node, boolean>): boolean{
        return true;

    }
}

let inputChangeManager: InputManager = new InputManager(document.getElementById("main-form"), 3);

//let pageManager = new DownloadPageManager(inputChangeManager);


 





