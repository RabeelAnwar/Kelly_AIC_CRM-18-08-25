export class DocumentUploadModel {
    id!: number;
    documentName?: string;
    documentTypeId!: number;
    clientId?: number;
    clientManagerId?: number;
    consultantId?: number;
    requisitionId?: number;
    documentTypeName?: string;
    source!: string;
    documentFileName?: string;
    documentFile?: File; // Corresponds to IFormFile
}
