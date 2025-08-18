export class TenantModel {
    id: number = 0;

    companyName: string = ''; // required
    tenantId? : number;

    activeStatus?: boolean;

    address1?: string;
    address2?: string;

    country?: string;
    state?: string;
    city?: string;

    zipCode?: string;

    phoneNo1?: string;
    phoneNo1Ext?: string;
    phoneNo2?: string;

    alternatePhoneNo?: string;

    workEmailId?: string;
    personalEmailId?: string;

    skypeId?: string;

    firstName?: string;
    middleName?: string;
    lastName?: string;
}


