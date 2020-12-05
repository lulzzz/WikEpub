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
    CreateInputNode() {
        let inputNode = document.createElement("input");
        inputNode.setAttribute("name", "WikiPages");
        inputNode.setAttribute("id", "input" + this.nodeNum.toString());
        inputNode.className = "url-input";
        return inputNode;
    }
    CreateEnclosingNode(enclosingNodeType) {
        let enclosingNode = document.createElement(enclosingNodeType);
        enclosingNode.id = "input-frame-" + this.nodeNum.toString();
        enclosingNode.textContent = "Wikipedia url: ";
        return enclosingNode;
    }
    CreateCrossElement() {
        let span = document.createElement("span");
        span.textContent = '\u2718';
        span.id = "url-cross-" + this.nodeNum.toString();
        return span;
    }
    createInputNode(enclosingNodeType) {
        let enclosingNode = this.CreateEnclosingNode(enclosingNodeType);
        enclosingNode.appendChild(this.CreateInputNode());
        enclosingNode.appendChild(this.CreateCrossElement());
        return enclosingNode;
    }
}
//# sourceMappingURL=InputManager.js.map