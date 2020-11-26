var ValidateUrls = /** @class */ (function () {
    function ValidateUrls(inputNodes, requestValidator) {
        var _this = this;
        this.inputNodes = inputNodes;
        this.requestValidator = requestValidator;
        document.addEventListener('inputChange', function () { _this.inputNodes = document.getElementsByClassName("url-input"); });
    }
    return ValidateUrls;
}());
//# sourceMappingURL=ValidateUrls.js.map