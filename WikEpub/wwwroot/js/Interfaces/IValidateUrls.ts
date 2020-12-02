export interface IValidateUrls{
	UrlIsValidInInput(node: Node): Promise<boolean>; 
}