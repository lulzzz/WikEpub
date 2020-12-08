export class ValidateUrls {
    constructor() {
    }
    async UrlIsValidInInput(node) {
        let urlString = node.value;
        if (this.UrlStringIsValid(urlString)) {
            return await this.CheckUrlResponse(urlString);
        }
        return false;
    }
    UrlStringIsValid(url) {
        const regex = new RegExp('(https:\\/\\/)?(en\\.)?wikipedia\\.org\\/(wiki\\/\\b(([-a-zA-Z0-9()@:%_\\+.~#?&\/\/=,]*){1}))');
        return regex.test(url.trim());
    }
    async CheckUrlResponse(url) {
        let apiUrl = 'https://en.wikipedia.org/api/rest_v1/page/title/' + url.split("/").slice(-1)[0];
        return await this.ValidateLink(apiUrl);
    }
    async ValidateLink(url) {
        let response = await fetch(url, {
            mode: 'cors',
            headers: { 'origin': 'include' }
        });
        if (response.status === 200) {
            return true;
        }
        return false;
    }
}
//# sourceMappingURL=ValidateUrls.js.map