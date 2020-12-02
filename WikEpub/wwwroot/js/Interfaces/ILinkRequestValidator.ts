export interface ILinkRequestValidator{
    ValidateLink(url: string): Promise<boolean>;
}