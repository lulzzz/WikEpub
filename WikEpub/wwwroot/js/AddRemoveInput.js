var AddRemoveInput = /** @class */ (function () {
    function AddRemoveInput(parentNode, nodeIndex) {
        var _this = this;
        this.parentNode = parentNode;
        this.nodeIndex = nodeIndex;
        this.nodeNum = 1;
        var addButton = document.getElementById("add-button");
        var removeButton = document.getElementById("remove-button");
        addButton.addEventListener("click", function () { return _this.insertInput("p"); });
        removeButton.addEventListener("click", function () { return _this.removeInput(); });
        this.inputChangeEvent = new Event('inputChange');
    }
    AddRemoveInput.prototype.insertInput = function (enclosingNodeType) {
        if (this.nodeNum > 9)
            return;
        this.nodeIndex++;
        this.nodeNum++;
        var insertNode = this.createInputNode(enclosingNodeType);
        this.insertAfter(this.parentNode.childNodes[this.nodeIndex], insertNode);
        this.removeButtons(this.parentNode.childNodes[this.nodeIndex - 1]);
        this.addButtons(this.parentNode.childNodes[this.nodeIndex]);
        document.dispatchEvent(this.inputChangeEvent);
    };
    AddRemoveInput.prototype.removeInput = function () {
        if (this.nodeNum === 1)
            return;
        this.parentNode.childNodes[this.nodeIndex].remove();
        this.nodeIndex--;
        this.nodeNum--;
        this.addButtons(this.parentNode.childNodes[this.nodeIndex]);
        document.dispatchEvent(this.inputChangeEvent);
    };
    AddRemoveInput.prototype.insertAfter = function (newSiblingNode, newNode) {
        newSiblingNode.parentNode.insertBefore(newNode, newSiblingNode);
        return newSiblingNode;
    };
    AddRemoveInput.prototype.createInputNode = function (enclosingNodeType) {
        var enclosingNode = document.createElement(enclosingNodeType);
        var inputNode = document.createElement("input");
        inputNode.setAttribute("name", "WikiPages");
        inputNode.setAttribute("id", "input" + (this.nodeIndex).toString());
        inputNode.className = "url-input";
        enclosingNode.textContent = "Wikipedia url: ";
        enclosingNode.appendChild(inputNode);
        return enclosingNode;
    };
    AddRemoveInput.prototype.addButtons = function (node) {
        var _this = this;
        var addButton = this.createButton("add-button", "button", "Add wikipage");
        var removeButton = this.createButton("remove-button", "button", "Remove wikipage");
        node.appendChild(addButton);
        node.appendChild(removeButton);
        addButton.addEventListener("click", function () { return _this.insertInput("p"); });
        removeButton.addEventListener("click", function () { return _this.removeInput(); });
    };
    AddRemoveInput.prototype.removeButtons = function (node) {
        node.removeChild(document.getElementById("add-button"));
        node.removeChild(document.getElementById("remove-button"));
    };
    AddRemoveInput.prototype.createButton = function (id, type, text) {
        var button = document.createElement("button");
        button.textContent = text;
        button.id = id;
        button.type = type;
        return button;
    };
    return AddRemoveInput;
}());
//# sourceMappingURL=AddRemoveInput.js.map