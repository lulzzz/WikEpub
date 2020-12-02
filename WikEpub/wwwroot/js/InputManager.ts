
import { IManageInputs } from "./Interfaces/IManageInputs";

export class InputManager implements IManageInputs {
    private nodeNum = 1;
    private inputChangeEvent: Event;

    constructor(
        private parentNode: Node,
        private nodeIndex: number
    ) {
        // this will be extracted into the class using this one
        let addButton = document.getElementById("add-button");
        let removeButton = document.getElementById("remove-button");
        addButton.addEventListener("click", () => this.insertInput("p"));
        removeButton.addEventListener("click", () => this.removeInput());
        this.inputChangeEvent = new Event('inputChange');
    }

    public insertInput(enclosingNodeType: string): void {
        if (this.nodeNum > 9) return;
        this.nodeIndex++;
        this.nodeNum++;
        let insertNode = this.createInputNode(enclosingNodeType);
        this.insertAfter(this.parentNode.childNodes[this.nodeIndex], insertNode);
        document.dispatchEvent(this.inputChangeEvent);
    }

    public removeInput() : void {
        if (this.nodeNum === 1) return;
        this.parentNode.childNodes[this.nodeIndex].remove();
        this.nodeIndex--;
        this.nodeNum--;
        document.dispatchEvent(this.inputChangeEvent);
    }

    private insertAfter(newSiblingNode: Node, newNode: Node): Node {
        newSiblingNode.parentNode.insertBefore(newNode, newSiblingNode);
        return newSiblingNode;
    }

    private createInputNode(enclosingNodeType: string): Node {
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
