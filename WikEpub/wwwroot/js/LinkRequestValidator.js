export class LinkRequestValidator {
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
//# sourceMappingURL=LinkRequestValidator.js.map