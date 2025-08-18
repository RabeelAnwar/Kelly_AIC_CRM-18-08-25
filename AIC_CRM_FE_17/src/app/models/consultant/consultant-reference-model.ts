export class ConsultantReferenceModal {
    id!: number;
    consultantId!: number;
    referenceName?: string;
    company?: string;
    managerPhone?: string;
    managerEmail?: string;
    referenceDate?: Date = new Date();
    employementDates?: string;
    hiringManagerComments?: string;
}