﻿import { IManageInputs } from "./Interfaces/IManageInputs";

export class InputManager implements IManageInputs {
    private nodeNum = 1;
    private inputNodes : Node[];
    private currentNode: Node;
    

    constructor(
        private parentNode: Node,
    )
    {
        this.inputNodes = [];
        this.currentNode = document.getElementById('input-frame-1');
        this.inputNodes.push(this.currentNode);
    }

    public insertInput(enclosingNodeType: string): Node {
        if (this.nodeNum > 9) return null;
        this.nodeNum++;
        let insertNode = this.createInputNode(enclosingNodeType);
        this.insertAfter(this.currentNode, insertNode)
        this.currentNode = insertNode;
        this.inputNodes.push(insertNode);
        return insertNode;
    }
   
    private insertAfter(sibling: Node, newNode: Node): Node {
        (sibling as HTMLElement).after(newNode);
        return sibling;
    }

    public removeInput(): boolean {
        if (this.nodeNum === 1) return false;
        this.parentNode.removeChild(this.inputNodes.pop());
        this.currentNode = this.inputNodes[this.inputNodes.length - 1];
        this.nodeNum--;
        return true;
    }

    public CreateInputNode(): Node {
        let inputNode = document.createElement("input");
        inputNode.setAttribute("name", "WikiPages");
        inputNode.setAttribute("id", "input" + this.nodeNum.toString());
        inputNode.className = "url-input";
        return inputNode;
    }

    public CreateEnclosingNode(enclosingNodeType: string): Node {
        let enclosingNode = document.createElement(enclosingNodeType);
        enclosingNode.id = "input-frame-" + this.nodeNum.toString();
        return enclosingNode;
    }
    public CreateCrossElement(): Node {
        let span = document.createElement("span");
        span.textContent = '\u2718';
        span.id = "url-cross-" + this.nodeNum.toString();
        return span;
    }

    private createInputNode(enclosingNodeType: string): Node {
        let enclosingNode = this.CreateEnclosingNode(enclosingNodeType);
        enclosingNode.appendChild(this.CreateInputNode());
        enclosingNode.appendChild(this.CreateCrossElement())
        return enclosingNode;
    }
}