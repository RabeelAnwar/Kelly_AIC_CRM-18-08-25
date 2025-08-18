export class OfficeMasterModel {
    fromDate?: Date;
    toDate?: Date;
    requisitionActivities?: RequisitionActivity[];
    requisitionInterviews?: RequisitionInterview[];
}

export class RequisitionActivity {
    createdDate!: Date;
    coordinator!: string;
    clientName!: string;
    clientId!: number;
    reqName!: string;
    reqId!: number;
    managerName!: string;
    managerId!: number;
    submittals!: number;
    interviews!: number;
    starts!: number;
    noOfPosition!: number;
    billRate!: string;
    duration!: string;
}

export class RequisitionInterview {
    enteredDate!: Date;
    salesRep!: string;
    recruiter!: string;
    consultantName!: string;
    consultantId!: number;
    clientName!: string;
    clientId!: number;
    requisition!: string;
    requisitionId!: number;
    startDate?: Date | null;       // Nullable date
    billRate!: number;             // Stored as string in backend
    hourlyRate!: number;              // Stored as string in backend
    markup!: number;
}
