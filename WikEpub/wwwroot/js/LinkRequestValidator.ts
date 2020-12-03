import { ILinkRequestValidator } from "./Interfaces/ILinkRequestValidator";
export class LinkRequestValidator implements ILinkRequestValidator {
    public async ValidateLink(url: string): Promise<boolean> {
        return await true;
    }
}