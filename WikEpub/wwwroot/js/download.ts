import { InputManager } from "./InputManager.js";
import { IManageInputs } from "./Interfaces/IManageInputs"
import { ValidateUrls } from "./ValidateUrls.js";
import { IValidateUrls } from "./Interfaces/IValidateUrls.js";

class DownloadPageManager {
    private inputManager: IManageInputs;
    private urlValidator: IValidateUrls;
    private inputNodes: Node[];
    private validNodeMap: Map<Node, boolean>;
    private submitButton: HTMLInputElement;
    private bookTitleInput: HTMLInputElement;
   
    constructor(inputManager: IManageInputs, inputValidator: IValidateUrls) {
        this.inputNodes = [];
        this.validNodeMap = new Map();
        this.inputManager = inputManager;
        this.urlValidator = inputValidator;
        this.submitButton = <HTMLInputElement>document.getElementById("submit-button");
        this.bookTitleInput = <HTMLInputElement>document.getElementById("book-title");
        this.bookTitleInput.addEventListener('change', () => {
            this.ChangeSubmitStatus();
            this.DisplayTitleStatus();
        });

        let firstInput = document.getElementById("input1");
        this.AddNode(firstInput, this.validNodeMap, this.inputNodes);
        this.SetUpButtons();
    }

    private SetUpButtons(): void {
        let addButton = document.getElementById("add-button");
        let removeButton = document.getElementById("remove-button");
        addButton.addEventListener('click', () => {
            this.AddNewInputNode();
            this.ChangeSubmitStatus();
            this.DisplayUrlStatus();
        });
        removeButton.addEventListener('click', () => {
            this.RemoveInputNode();
            this.ChangeSubmitStatus();
        });
    }

    private RemoveInputNode() {
        if (this.inputManager.removeInput()) {
            let removedNode = this.inputNodes.pop(); // side-effect on DOM
            this.validNodeMap.delete(removedNode);
        }
    }

    private AddNewInputNode() {
        let newNode = this.inputManager.insertInput('p'); // side-effect on DOM
        if (newNode !== null) {
            let inputElement = (newNode as HTMLElement).querySelector('input'); // get actual input element
            this.AddNode(inputElement, this.validNodeMap, this.inputNodes)
        }
    }

    private AddNode(inputElement: Node, validNodeMap: Map<Node, boolean>, inputNodes: Node[]) {
        validNodeMap.set(inputElement, false);
        inputNodes.push(inputElement);
        inputElement.addEventListener('change', async () => {
            await this.CheckIfNodeIsValid(inputElement)
                .then(() => this.ChangeSubmitStatus())
                .then(() => this.DisplayUrlStatus());
        });
        this.submitButton.disabled = true;
    }

    // extract
    // Add additional check: & is not duplicate
    // Store result (invalid url | duplicate | all good)
    private async CheckIfNodeIsValid(node: Node): Promise<void>{
        if (await this.urlValidator.UrlIsValidInInput(node)) {
            this.validNodeMap.set(node, true);
        } else {
            this.validNodeMap.set(node, false);
        }
    }

    private ChangeSubmitStatus() {
        if (this.AllNodesAreValid(this.validNodeMap)
            && this.DoesNotContainDuplicates(this.inputNodes)
            && this.bookTitleInput.value.length !== 0) {
            this.submitButton.disabled = false;
        } else {
            this.submitButton.disabled = true;
        }
    }

    private DisplayUrlStatus(): void {
        this.validNodeMap.forEach((nodeIsValid: boolean, node: Node) => {
            let spanElement = node.parentNode.querySelector("span"); 
            if (nodeIsValid) {
                spanElement.textContent = '\u2714';
            }
            else {
                spanElement.textContent = '\u2718';
            }
        });
    }

    private DisplayTitleStatus() {
        let titleCross = document.getElementById("title-cross");
        if (this.bookTitleInput.value.length !== 0) {
            titleCross.textContent = '\u2714';
        }
        else {
            titleCross.textContent = '\u2718';
        }
    }

    // extract 
    private DoesNotContainDuplicates(nodes: Node[]): boolean {
        let values = nodes.map(x => (x as HTMLInputElement).value);
        let setValues = new Set(values);
        return values.length === setValues.size
    }


    private AllNodesAreValid(nodeMap: Map<Node, boolean>): boolean {
        for (let [node, valid] of nodeMap)
            if (!valid) return false;
        return true;
    }
}

let inputChangeManager = new InputManager(document.getElementById("main-form") );
let validateUrls = new ValidateUrls();
let pageManager = new DownloadPageManager(inputChangeManager, validateUrls);