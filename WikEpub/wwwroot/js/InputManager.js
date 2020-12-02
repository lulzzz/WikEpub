export class InputManager {
    constructor(parentNode, nodeIndex) {
        this.parentNode = parentNode;
        this.nodeIndex = nodeIndex;
        this.nodeNum = 1;
        // this will be extracted into the class using this one
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
        document.dispatchEvent(this.inputChangeEvent);
    }
    removeInput() {
        if (this.nodeNum === 1)
            return;
        this.parentNode.childNodes[this.nodeIndex].remove();
        this.nodeIndex--;
        this.nodeNum--;
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
}
//# sourceMappingURL=InputManager.js.map