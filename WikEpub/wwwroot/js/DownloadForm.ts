import { DownloadPageManager } from "./DownloadManager.js";
import { InputValidator } from "./InputValidator.js";
import { InputManager } from "./InputManager.js";
import { ValidateUrls } from "./ValidateUrls.js";

let validateUrls = new ValidateUrls();
let inputManager = new InputManager(document.getElementById("main-form"));
let inputValidator = new InputValidator(validateUrls);
let downloadPageManager = new DownloadPageManager(inputManager, inputValidator);