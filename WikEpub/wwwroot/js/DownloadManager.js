export class DownloadPageManager {
    constructor(inputManager, inputValidator) {
        this.inputManager = inputManager;
        this.inputValidator = inputValidator;
        this.AddFirstInputNode();
        this.SetUpButtons();
        this.SetUpBookTitleInput();
    }
    SetUpButtons() {
        let addButton = document.getElementById("add-button");
        let removeButton = document.getElementById("remove-button");
        this.submitButton = document.getElementById("submit-button");
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
    SetUpBookTitleInput() {
        this.bookTitleInput = document.getElementById("book-title");
        this.bookTitleInput.addEventListener('change', () => {
            this.CheckSubmitStatus();
            this.DisplayTitleStatus();
        });
    }
    AddNewInputNode() {
        let newNode = this.inputManager.insertInput('p'); // side-effect on DOM
        if (newNode !== null) {
            this.inputValidator.AddNode(newNode);
            newNode.addEventListener('change', async () => {
                await this.inputValidator.CheckNodeOnChange(newNode.querySelector('input'))
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
        let firstNode = document.getElementById("input-frame-1");
        this.inputValidator.AddNode(firstNode);
        let inputNode = firstNode.querySelector('input');
        inputNode.addEventListener('change', async () => {
            await this.inputValidator.CheckNodeOnChange(inputNode)
                .then(() => this.CheckSubmitStatus())
                .then(() => this.DisplayUrlStatus());
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