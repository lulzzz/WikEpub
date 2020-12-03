import { IManageInputs } from "./Interfaces/IManageInputs";

export class InputManager implements IManageInputs {
    private nodeNum = 1;

    constructor(
        private parentNode: Node,
        private nodeIndex: number
    ) { }

    public insertInput(enclosingNodeType: string): Node {
        if (this.nodeNum > 9) return null;
        this.nodeIndex++;
        this.nodeNum++;
        let insertNode = this.createInputNode(enclosingNodeType);
        this.insertAfter(this.parentNode.childNodes[this.nodeIndex], insertNode);
        return insertNode;
    }

    public removeInput(): boolean {
        if (this.nodeNum === 1) return false;
        this.parentNode.childNodes[this.nodeIndex].remove();
        this.nodeIndex--;
        this.nodeNum--;
        return true;
    }

    private insertAfter(newSiblingNode: Node, newNode: Node): Node {
        newSiblingNode.parentNode.insertBefore(newNode, newSiblingNode);
        return newSiblingNode;
    }

    private createInputNode(enclosingNodeType: string): Node {
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