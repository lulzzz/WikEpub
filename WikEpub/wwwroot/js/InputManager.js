export class InputManager {
    constructor(parentNode, nodeIndex) {
        this.parentNode = parentNode;
        this.nodeIndex = nodeIndex;
        this.nodeNum = 1;
        let addButton = document.getElementById("add-button");
        let removeButton = document.getElementById("remove-button");
        addButton.addEventListener("click", () => this.insertInput("p"));
        removeButton.addEventListener("click", () => this.removeInput());
        this.inputChangeEvent = new Event('inputChange');
    }
    insertInput(enclosingNodeType) {
        if (this.nodeNum > 9)
            return;
        this.nodeIndex++;
        this.nodeNum++;
        let insertNode = this.createInputNode(enclosingNodeType);
        this.insertAfter(this.parentNode.childNodes[this.nodeIndex], insertNode);
        this.removeButtons(this.parentNode.childNodes[this.nodeIndex - 1]);
        this.addButtons(this.parentNode.childNodes[this.nodeIndex]);
        document.dispatchEvent(this.inputChangeEvent);
    }
    removeInput() {
        if (this.nodeNum === 1)
            return;
        this.parentNode.childNodes[this.nodeIndex].remove();
        this.nodeIndex--;
        this.nodeNum--;
        this.addButtons(this.parentNode.childNodes[this.nodeIndex]);
        document.dispatchEvent(this.inputChangeEvent);
    }
    insertAfter(newSiblingNode, newNode) {
        newSiblingNode.parentNode.insertBefore(newNode, newSiblingNode);
        return newSiblingNode;
    }
    createInputNode(enclosingNodeType) {
        let enclosingNode = document.createElement(enclosingNodeType);
        let inputNode = document.createElement("input");
        inputNode.setAttribute("name", "WikiPages");
        inputNode.setAttribute("id", "input" + (this.nodeIndex).toString());
        inputNode.className = "url-input";
        enclosingNode.textContent = "Wikipedia url: ";
        enclosingNode.appendChild(inputNode);
        return enclosingNode;
    }
    addButtons(node) {
        let addButton = this.createButton("add-button", "button", "Add wikipage");
        let removeButton = this.createButton("remove-button", "button", "Remove wikipage");
        node.appendChild(addButton);
        node.appendChild(removeButton);
        addButton.addEventListener("click", () => this.insertInput("p"));
        removeButton.addEventListener("click", () => this.removeInput());
    }
    removeButtons(node) {
        node.removeChild(document.getElementById("add-button"));
        node.removeChild(document.getElementById("remove-button"));
    }
    createButton(id, type, text) {
        let button = document.createElement("button");
        button.textContent = text;
        button.id = id;
        button.type = type;
        return button;
    }
}
//# sourceMappingURL=InputManager.js.map