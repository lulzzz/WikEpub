import { ILinkRequestValidator } from "./Interfaces/ILinkRequestValidator"

export class ValidateUrls {
    private requestValidator: ILinkRequestValidator;

    constructor(requestValidator: ILinkRequestValidator) {
        this.requestValidator = requestValidator;
    }

    public async NodeIsValid(node: Node): Promise<boolean> {
        let urlString = node.nodeValue;
        if (this.urlIsValid(urlString)) {
            return await this.CheckUrlResponse(urlString)
        }
        return false;
    }

    private urlIsValid(url: string): boolean {
        return true 
    }

    private async CheckUrlResponse(url: string): Promise<boolean> {
        return true
    }



}

