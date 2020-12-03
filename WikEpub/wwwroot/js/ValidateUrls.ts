﻿import { ILinkRequestValidator } from "./Interfaces/ILinkRequestValidator"
import { IValidateUrls } from "./Interfaces/IValidateUrls";

export class ValidateUrls implements IValidateUrls {
    private requestValidator: ILinkRequestValidator;

    constructor(requestValidator: ILinkRequestValidator) {
        this.requestValidator = requestValidator;
    }

    public async UrlIsValidInInput(node: Node): Promise<boolean> {
        let urlString = (node as HTMLInputElement).value;
        if (this.UrlStringIsValid(urlString)) {
            return await this.CheckUrlResponse(urlString)
        }
        return false;
    }

    private UrlStringIsValid(url: string): boolean {
        const regex =
            new RegExp('(https:\\/\\/)?(en\\.)?wikipedia\\.org\\/[a-zA-Z0-9()]{1,6}\\b([-a-zA-Z0-9()@:%_\\+.~#?&\\/\\/=,]*)');
        console.log(regex.test(url.trim()));
        return regex.test(url.trim());
    }

    private async CheckUrlResponse(url: string): Promise<boolean> {
        let apiUrl = 'https://en.wikipedia.org/api/rest_v1/page/title/' + url.split("/").slice(-1)[0];
        return await this.requestValidator.ValidateLink(apiUrl);
    }
}