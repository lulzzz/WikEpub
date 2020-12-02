class ValidateUrls {
    private requestValidator: ILinkRequestValidator;
    private nodesAreValid: Event;
    private nodesAreInvalid: Event;

    constructor(requestValidator: ILinkRequestValidator) {
        this.requestValidator = requestValidator;
        this.nodesAreValid = new Event('nodesAreValid');
        this.nodesAreInvalid = new Event('nodesAreInvalid')
        document.addEventListener('inputChange', () => this.CheckNodesUponChange());
    }


    private CheckNodesUponChange() {
        let nodes = document.getElementsByClassName("url-input");
        if (this.allNodesAreValid(nodes)) document.dispatchEvent(this.nodesAreValid);
        else document.dispatchEvent(this.nodesAreInvalid);
    } 


    private NodeIsValid(node: Node): boolean {
        // check regex
        // make http request and check response
        return true;

    }



    private allNodesAreValid(nodes : HTMLCollectionOf<Element>) {
        let numberOfNodes = nodes.length;
        let numOfValidatedNodes = 0;
        for (var i = 0; i < numberOfNodes; i++) {
            if (this.NodeIsValid(nodes[i]))
                numberOfNodes++;
        }
        return numOfValidatedNodes === numberOfNodes;
        
    }

    

}

