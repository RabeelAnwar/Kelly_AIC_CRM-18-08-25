export class RequisitionModel {
    // IT Requisition Section
    id!: number;
    clientId!: number;
    clientName?: string;
    managerName?: string;
    internalReqCoordinatorId!: string; // Required
    internalReqCoordinatorName?: string;
    requisitionType!: string;          // Required
    priority!: number;                 // Required
    jobTitle!: string;                // Required
    clientReqNumber?: string;
    managerId!: number;               // Required
    salesRepId!: string;              // Required
    salesRepName?: string;
    recruiterAssignedId!: string[];   // Required
    recruiterAssignedName?: string[];
    location?: string;
    duration?: string;
    durationTypes?: string;
    startDate!: Date;                 // Required
    createdDate?: Date;                 // Required
    numberOfPositions!: number;       // Required
    comments?: string;
    projectDepartmentOverview?: string;
    jobDescription?: string;
    payRate?: string;
    billRate?: string;
    hours?: string;
    overtime?: string;
    interviewProcesses?: string;
    phoneHireIfOutOfArea?: string;
    clientMarkup?: string;
    billRateHighestBeforeResumeNotSent?: string;
    secondaryContact?: string;
    hiringManagerVop?: string;
    otherWaysToFillPosition?: string;
    notes?: string;
    responsibilities?: string;
    qualifications?: string;
    searchString1?: string;
    searchString2?: string;
    searchString3?: string;
    codingValue?: number;
    analysisValue?: number;
    testingValue?: number;
    otherValue?: number;

    // Technical Skills Section
    hardware?: string;
    os?: string;
    languages?: string;
    databases?: string;
    protocols?: string;
    softwareStandards?: string;
    others?: string;
    status?: boolean;
}
