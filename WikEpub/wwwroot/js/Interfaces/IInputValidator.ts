export interface IInputValidator {
    AddNode(node: Node): void;
    RemoveNode(): void;
    CheckNodeOnChange(node: Node): Promise<void>;
    AllNodesAreValid(): boolean;
    GetValidNodeReason(node: Node): [boolean, string];
}