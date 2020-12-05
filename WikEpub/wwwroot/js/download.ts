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
    private bookTitleInput: HTMLInputElement;
   
    constructor(inputManager: IManageInputs, inputValidator: IValidateUrls) {
        this.nodes = [];
        this.validNodeMap = new Map();
        this.inputManager = inputManager;
        this.urlValidator = inputValidator;
        this.submitButton = <HTMLInputElement>document.getElementById("submit-button");
        this.bookTitleInput = <HTMLInputElement>document.getElementById("book-title");
        this.bookTitleInput.addEventListener('change', () => this.CheckSubmitStatus());

        let firstInput = document.getElementById("input1");
        this.AddNode(firstInput, this.validNodeMap, this.nodes);
        this.SetUpButtons();
    }

    private SetUpButtons(): void {
        let addButton = document.getElementById("add-button");
        let removeButton = document.getElementById("remove-button");
        addButton.addEventListener('click', () => {
            this.addNewInputNode();
            this.CheckSubmitStatus();
        });
        removeButton.addEventListener('click', () => {
            this.removeInputNode();
            this.CheckSubmitStatus();
        });
    }

    private removeInputNode() {
        if (this.inputManager.removeInput()) {
            let removedNode = this.nodes.pop(); // side-effect on DOM
            this.validNodeMap.delete(removedNode);
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
        inputElement.addEventListener('change', () => {
            this.ValidateNode(inputElement)
                .then(() => this.CheckSubmitStatus());
        });
        this.submitButton.disabled = true;
    }

    private async ValidateNode(node: Node): Promise<void>{
        if (await this.urlValidator.UrlIsValidInInput(node)) {
            this.validNodeMap.set(node, true);
        } else {
            this.validNodeMap.set(node, false);
        }
    }

    private CheckSubmitStatus() {
        console.log(this.bookTitleInput.value.length);
        if (this.AllNodesAreValid(this.validNodeMap)
            && this.DoesNotContainDuplicates(this.nodes) && this.bookTitleInput.value.length !== 0) {
            this.submitButton.disabled = false;
        } else {
            this.submitButton.disabled = true;
        }
    }

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

let inputChangeManager: InputManager = new InputManager(document.getElementById("main-form"), 3);
let linkRequestValidator = new LinkRequestValidator();
let validateUrls = new ValidateUrls(linkRequestValidator);
let pageManager = new DownloadPageManager(inputChangeManager, validateUrls);