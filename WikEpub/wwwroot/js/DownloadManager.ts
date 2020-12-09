import { IManageInputs } from "./Interfaces/IManageInputs";
import { IInputValidator } from "./Interfaces/IInputValidator";

export class DownloadPageManager {
    private inputManager: IManageInputs;
    private inputValidator: IInputValidator
    private submitButton: HTMLInputElement;
    private bookTitleInput: HTMLInputElement;

    constructor(inputManager: IManageInputs, inputValidator: IInputValidator) {
        this.inputManager = inputManager;
        this.inputValidator = inputValidator;
        this.submitButton = <HTMLInputElement>document.getElementById("submit-button");
        this.bookTitleInput = <HTMLInputElement>document.getElementById("book-title");
        this.bookTitleInput.addEventListener('change', () => {
            this.CheckSubmitStatus();
            this.DisplayTitleStatus();
        });
        this.AddFirstInputNode();
        this.SetUpButtons();
    }

    private SetUpButtons(): void {
        let addButton = document.getElementById("add-button");
        let removeButton = document.getElementById("remove-button");
        addButton.addEventListener('click', () => {
            this.AddNewInputNode();
            this.CheckSubmitStatus();
            this.DisplayUrlStatus();
        });
        removeButton.addEventListener('click', () => {
            this.RemoveInputNode();
            this.CheckSubmitStatus();
        });
    }

    private AddNewInputNode() : void {
        let newNode = this.inputManager.insertInput('p'); // side-effect on DOM
        if (newNode !== null) {
            this.inputValidator.AddNode(newNode);
            newNode.addEventListener('change', async () => {
                await this.inputValidator.CheckNodeOnChange(newNode)
                    .then(() => this.CheckSubmitStatus())
                    .then(() => this.DisplayUrlStatus())
            });
        }
    }

    private RemoveInputNode(): void {
        if (this.inputManager.removeInput())
            this.inputValidator.RemoveNode();
    }

    private AddFirstInputNode() {
        let firstNode = document.getElementById("input1");
        this.inputValidator.AddNode(firstNode);
        firstNode.addEventListener('change', () => {
            this.inputValidator.CheckNodeOnChange(firstNode);
        });
    }

    private CheckSubmitStatus(): void {
        if (this.inputValidator.AllNodesAreValid() && this.bookTitleInput.value.length > 0) {
            this.submitButton.disabled = false;
        }
        else this.submitButton.disabled = true;
    }

    private DisplayUrlStatus(): void {
        let validNodeReasons = this.inputValidator.GetValidNodeReasons();
        for (let [node, isValid, reason] of validNodeReasons) {
            let spanElement = node.parentNode.querySelector("span");
            if (isValid) 
                spanElement.textContent = '\u2714';
            else 
                spanElement.textContent = '\u2718';
            }
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


}