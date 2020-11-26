class AddRemoveInput implements IAddRemoveInput{
    private nodeNum = 1;
    private inputChangeEvent: Event;
    constructor(
        private parentNode: Node,
        private nodeIndex: number
    ) {
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
        this.removeButtons(this.parentNode.childNodes[this.nodeIndex - 1]);
        this.addButtons(this.parentNode.childNodes[this.nodeIndex]);
        document.dispatchEvent(this.inputChangeEvent);
    }

    public removeInput() : void {
        if (this.nodeNum === 1) return;
        this.parentNode.childNodes[this.nodeIndex].remove();
        this.nodeIndex--;
        this.nodeNum--;
        this.addButtons(this.parentNode.childNodes[this.nodeIndex]);
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

    private addButtons(node: Node): void {
        let addButton = this.createButton("add-button", "button", "Add wikipage");
        let removeButton = this.createButton("remove-button", "button", "Remove wikipage");
        node.appendChild(addButton);
        node.appendChild(removeButton);
        addButton.addEventListener("click", () => this.insertInput("p"));
        removeButton.addEventListener("click", () => this.removeInput());
    }

    private removeButtons(node: Node): void {
        
        node.removeChild(document.getElementById("add-button"));
        node.removeChild(document.getElementById("remove-button"));
    }

    private createButton(id: string, type: string, text: string): Node {
        let button = document.createElement("button");
        button.textContent = text;
        button.id = id;
        button.type = type;
        return button;
    }
}
