export class ConsultantInterviewProcessModal {

    id!: number;
    consultantActivityId?: number;

    date!: Date;
    notes?: string;
    expectedStartDate?: Date;
    endDate?: Date;
    employmentType?: string;
    salary: number = 0;
    hourlyRate: number = 0;
    expenses: number = 0;
    loadedRate: number = 0;
    billRate: number = 0;
    vop: number = 0;
    markup: number = 0;
    recruiterAssigned?: string;
    salesRep?: string;
    notesDetail?: string;
    startCandidate?: boolean;
}