export class ConsultantActivityModal {
    id!: number;
    clientId!: number;
    managerId!: number;
    consultantId!: number;
    requisitionId!: number;
    assignedToId!: string;
    
    consultantName?: string;
    billRate?: number;
    payRate?: number;
    lastContact?: Date;
    createDate?: Date;
    disabled?: boolean = false;

    interviewStatus?: string[];
    interviewDateTime?: Date[];
    processId?: number[];
}