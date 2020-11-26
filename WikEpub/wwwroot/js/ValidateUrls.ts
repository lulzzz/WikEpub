class ValidateUrls {
    private inputNodes: HTMLCollectionOf<Element>;
    private requestValidator: ILinkRequestValidator;
    constructor(inputNodes: HTMLCollectionOf<Element>, requestValidator: ILinkRequestValidator) {
        this.inputNodes = inputNodes;
        this.requestValidator = requestValidator;
        document.addEventListener('inputChange', () => { this.inputNodes = document.getElementsByClassName("url-input") });
        document.addEventListener('inputChange', () => {
            for (var i = 0; i < inputNodes.length; i++) {
                if (inputNodes[i].)

            }
        });
         
    }

    private validateRequest : 
    

}
