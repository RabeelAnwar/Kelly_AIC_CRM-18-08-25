export class SupportTicketModel {
    id!: number;
    type!: string;
    status!: string;
    priority!: string;
    subject!: string;
    description?: string;
    documentFile?: File;
}