class ValidateUrls {
    constructor(inputNodes, requestValidator) {
        this.inputNodes = inputNodes;
        this.requestValidator = requestValidator;
        document.addEventListener('inputChange', () => { this.inputNodes = document.getElementsByClassName("url-input"); });
    }
}
//# sourceMappingURL=ValidateUrls.js.map