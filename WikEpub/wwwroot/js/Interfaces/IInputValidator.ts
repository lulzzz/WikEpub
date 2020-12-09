export interface IInputValidator {
    AddNode(node: Node): void;
    RemoveNode(): void;
    CheckNode(node: Node): boolean;
    AllNodesAreValid(): boolean;
    GetValidNodeReason(node: Node): [boolean, string];
}