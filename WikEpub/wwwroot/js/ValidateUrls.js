export class ValidateUrls {
    constructor(requestValidator) {
        this.requestValidator = requestValidator;
    }
    async UrlIsValidInInput(node) {
        let urlString = node.nodeValue;
        if (this.urlStringIsValid(urlString)) {
            return await this.CheckUrlResponse(urlString);
        }
        return false;
    }
    urlStringIsValid(url) {
        return true;
    }
    async CheckUrlResponse(url) {
        return await this.requestValidator.ValidateLink(url);
    }
}
//# sourceMappingURL=ValidateUrls.js.map