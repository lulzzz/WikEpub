export class InputManager {
    constructor(parentNode, nodeIndex) {
        this.parentNode = parentNode;
        this.nodeIndex = nodeIndex;
        this.nodeNum = 1;
    }
    insertInput(enclosingNodeType) {
        if (this.nodeNum > 9)
            return null;
        this.nodeIndex++;
        this.nodeNum++;
        let insertNode = this.createInputNode(enclosingNodeType);
        this.insertAfter(this.parentNode.childNodes[this.nodeIndex], insertNode);
        return insertNode;
    }
    removeInput() {
        if (this.nodeNum === 1)
            return false;
        this.parentNode.childNodes[this.nodeIndex].remove();
        this.nodeIndex--;
        this.nodeNum--;
        return true;
    }
    insertAfter(newSiblingNode, newNode) {
        newSiblingNode.parentNode.insertBefore(newNode, newSiblingNode);
        return newSiblingNode;
    }
    createInputNode(enclosingNodeType) {
        let enclosingNode = document.createElement(enclosingNodeType);
        enclosingNode.id = "input-frame-" + (this.nodeNum).toString();
        let inputNode = document.createElement("input");
        inputNode.setAttribute("name", "WikiPages");
        inputNode.setAttribute("id", "input" + (this.nodeNum).toString());
        inputNode.className = "url-input";
        enclosingNode.textContent = "Wikipedia url: ";
        enclosingNode.appendChild(inputNode);
        return enclosingNode;
    }
}
//# sourceMappingURL=InputManager.js.map