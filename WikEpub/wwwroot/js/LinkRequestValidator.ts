import { ILinkRequestValidator } from "./Interfaces/ILinkRequestValidator";
export class LinkRequestValidator implements ILinkRequestValidator {
    public async ValidateLink(url: string): Promise<boolean> {
        let response: Response = await fetch(url, {
            mode: 'cors',
            headers: {'origin': 'include'}
                }
            );
        if (response.status === 200) {
            return true;
        }
        return false;
    }
}