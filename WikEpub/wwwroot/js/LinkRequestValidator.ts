import { ILinkRequestValidator } from "./Interfaces/ILinkRequestValidator";
export class LinkRequestValidator implements ILinkRequestValidator {
    public async ValidateLink(url: string): Promise<boolean> {
        throw new Error("Method not implemented.");
    }
    
}
