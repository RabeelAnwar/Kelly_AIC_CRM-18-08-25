export class ClientPipelineModel {
    id!: number;
    clientId!: number;
    managerId?: number;
    winningProbabilityAmount?: string;
    pipelineAmount?: string;
    pipelineSource?: string;
    pipelineTypes!: string;
    pipelineAssociatedWithReqs?: string;
    winningProbabilities?: number;
}