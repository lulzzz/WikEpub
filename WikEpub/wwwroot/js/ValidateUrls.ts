import { ILinkRequestValidator } from "./Interfaces/ILinkRequestValidator"
import { IValidateUrls } from "./Interfaces/IValidateUrls";

export class ValidateUrls implements IValidateUrls {
    private requestValidator: ILinkRequestValidator;

    constructor(requestValidator: ILinkRequestValidator) {
        this.requestValidator = requestValidator;
    }

    public async UrlIsValidInInput(node: Node): Promise<boolean> {
        let urlString = node.nodeValue;
        if (this.urlStringIsValid(urlString)) {
            return await this.CheckUrlResponse(urlString)
        }
        return false;
    }

    private urlStringIsValid(url: string): boolean {
        return true
    }

    private async CheckUrlResponse(url: string): Promise<boolean> {
        return await this.requestValidator.ValidateLink(url);
    }
}