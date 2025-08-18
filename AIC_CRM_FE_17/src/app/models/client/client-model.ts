export class ClientModel {
    id!: number;
    clientName!: string;
    parentClientId?: number;
    completeAddress?: string;
    address1?: string;
    address2?: string;
    country?: string;
    state?: string;
    city?: string;
    zipCode?: string;
    officeContact?: string;
    officeContactExt?: string;
    division?: string;
    website?: string;
    purchaseContactName?: string;
    purchaseContact?: string;
    purchaseContactExt?: string;
    diversityContactName?: string;
    diversityContact?: string;
    diversityContactExt?: string;
    registered?: boolean;
    registrationPortal?: string;
    companySize?: string;
    notes?: string;
  }
  