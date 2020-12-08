import { InputManager } from "./InputManager.js";
import { ValidateUrls } from "./ValidateUrls.js";
import { LinkRequestValidator } from "./LinkRequestValidator.js";
class DownloadPageManager {
    constructor(inputManager, inputValidator) {
        this.inputNodes = [];
        this.validNodeMap = new Map();
        this.inputManager = inputManager;
        this.urlValidator = inputValidator;
        this.submitButton = document.getElementById("submit-button");
        this.bookTitleInput = document.getElementById("book-title");
        this.bookTitleInput.addEventListener('change', () => {
            this.CheckSubmitStatus();
            this.DisplayTitleStatus();
        });
        let firstInput = document.getElementById("input1");
        this.AddNode(firstInput, this.validNodeMap, this.inputNodes);
        this.SetUpButtons();
    }
    SetUpButtons() {
        let addButton = document.getElementById("add-button");
        let removeButton = document.getElementById("remove-button");
        addButton.addEventListener('click', () => {
            this.AddNewInputNode();
            this.CheckSubmitStatus();
            this.DisplayStatus();
        });
        removeButton.addEventListener('click', () => {
            this.RemoveInputNode();
            this.CheckSubmitStatus();
        });
    }
    RemoveInputNode() {
        if (this.inputManager.removeInput()) {
            let removedNode = this.inputNodes.pop(); // side-effect on DOM
            this.validNodeMap.delete(removedNode);
        }
    }
    AddNewInputNode() {
        let newNode = this.inputManager.insertInput('p'); // side-effect on DOM
        if (newNode !== null) {
            let inputElement = newNode.childNodes[1]; // get actual input element
            this.AddNode(inputElement, this.validNodeMap, this.inputNodes);
        }
    }
    AddNode(inputElement, validNodeMap, inputNodes) {
        validNodeMap.set(inputElement, false);
        inputNodes.push(inputElement);
        inputElement.addEventListener('change', () => {
            this.ValidateNode(inputElement)
                .then(() => this.CheckSubmitStatus())
                .then(() => this.DisplayStatus());
        });
        this.submitButton.disabled = true;
    }
    async ValidateNode(node) {
        if (await this.urlValidator.UrlIsValidInInput(node)) {
            this.validNodeMap.set(node, true);
        }
        else {
            this.validNodeMap.set(node, false);
        }
    }
    CheckSubmitStatus() {
        if (this.AllNodesAreValid(this.validNodeMap)
            && this.DoesNotContainDuplicates(this.inputNodes)
            && this.bookTitleInput.value.length !== 0) {
            this.submitButton.disabled = false;
        }
        else {
            this.submitButton.disabled = true;
        }
    }
    DisplayStatus() {
        this.validNodeMap.forEach((nodeIsValid, node) => {
            let spanElement = node.parentNode.querySelector("span");
            if (nodeIsValid) {
                spanElement.textContent = '\u2714';
            }
            else {
                spanElement.textContent = '\u2718';
            }
        });
    }
    DisplayTitleStatus() {
        let titleCross = document.getElementById("title-cross");
        if (this.bookTitleInput.value.length !== 0) {
            titleCross.textContent = '\u2714';
        }
        else {
            titleCross.textContent = '\u2718';
        }
    }
    DoesNotContainDuplicates(nodes) {
        let values = nodes.map(x => x.value);
        let setValues = new Set(values);
        return values.length === setValues.size;
    }
    AllNodesAreValid(nodeMap) {
        for (let [node, valid] of nodeMap)
            if (!valid)
                return false;
        return true;
    }
}
let inputChangeManager = new InputManager(document.getElementById("main-form"));
let linkRequestValidator = new LinkRequestValidator();
let validateUrls = new ValidateUrls(linkRequestValidator);
let pageManager = new DownloadPageManager(inputChangeManager, validateUrls);
//# sourceMappingURL=download.js.map