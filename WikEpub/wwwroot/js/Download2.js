import { DownloadPageManager } from "./DownloadManager.js";
import { InputValidator } from "./InputValidator.js";
import { InputManager } from "./InputManager.js";
import { ValidateUrls } from "./ValidateUrls.js";
let downloadPageManager = new DownloadPageManager(new InputManager(document.getElementById("main-form")), new InputValidator(new ValidateUrls()));
//# sourceMappingURL=Download2.js.map