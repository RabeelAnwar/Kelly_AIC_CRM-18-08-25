export class ClientManagerModel {
    id!: number;
    clientId!: number;
    clientName?: string;
    firstName!: string;
    middleName?: string;
    lastName!: string;
    title?: string;
    departmentId?: number;
    worksUnderId?: number;
    isAssignedToId?: string;

    completeAddress?: string;
    address1?: string;
    address2?: string;
    country?: string;
    state?: string;
    city?: string;
    zipCode?: string;

    officePhone?: string;
    officePhoneExt?: string;
    cellPhone?: string;
    cellPhoneExt?: string;
    homePhone?: string;
    homePhoneExt?: string;

    workEmail?: string;
    personalEmail?: string;
    linkedInUrl?: string;
    linkedInImageUrl?: string;

    skillNeeded?: string;
    notes?: string;
    isManager?: boolean;
    stillWithCompany?: boolean;
    isActive?: boolean;

    callRecords?: string;
}
