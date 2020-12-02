export class ValidateUrls {
    constructor(requestValidator) {
        this.requestValidator = requestValidator;
    }
    async NodeIsValid(node) {
        let urlString = node.nodeValue;
        if (this.urlIsValid(urlString)) {
            return await this.CheckUrlResponse(urlString);
        }
        return false;
    }
    urlIsValid(url) {
        return true;
    }
    async CheckUrlResponse(url) {
        return true;
    }
}
//# sourceMappingURL=ValidateUrls.js.map