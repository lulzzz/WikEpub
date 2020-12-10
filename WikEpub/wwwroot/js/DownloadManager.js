export class DownloadPageManager {
    constructor(inputManager, inputValidator) {
        this.inputManager = inputManager;
        this.inputValidator = inputValidator;
        this.submitButton = document.getElementById("submit-button");
        this.bookTitleInput = document.getElementById("book-title");
        this.bookTitleInput.addEventListener('change', () => {
            this.CheckSubmitStatus();
            this.DisplayTitleStatus();
        });
        this.AddFirstInputNode();
        this.SetUpButtons();
    }
    SetUpButtons() {
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
    AddNewInputNode() {
        let newNode = this.inputManager.insertInput('p'); // side-effect on DOM
        if (newNode !== null) {
            this.inputValidator.AddNode(newNode);
            newNode.addEventListener('change', async () => {
                await this.inputValidator.CheckNodeOnChange(newNode)
                    .then(() => this.CheckSubmitStatus())
                    .then(() => this.DisplayUrlStatus());
            });
        }
    }
    RemoveInputNode() {
        if (this.inputManager.removeInput())
            this.inputValidator.RemoveNode();
    }
    AddFirstInputNode() {
        let firstNode = document.getElementById("input1");
        this.inputValidator.AddNode(firstNode);
        firstNode.addEventListener('change', () => {
            this.inputValidator.CheckNodeOnChange(firstNode);
        });
    }
    CheckSubmitStatus() {
        if (this.inputValidator.AllNodesAreValid() && this.bookTitleInput.value.length > 0) {
            this.submitButton.disabled = false;
        }
        else
            this.submitButton.disabled = true;
    }
    DisplayUrlStatus() {
        let validNodeReasons = this.inputValidator.GetValidNodeReasons();
        for (let [node, isValid, reason] of validNodeReasons) {
            let spanElement = node.parentNode.querySelector("span");
            if (isValid)
                spanElement.textContent = '\u2714';
            else
                spanElement.textContent = '\u2718';
        }
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
}
//# sourceMappingURL=DownloadManager.js.map