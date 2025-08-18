export class UserModel {
    id?: string;
    firstName!: string;
    middleName?: string;
    lastName!: string;
    userName!: string;
    password!: string;
    contactTypeId!: number;
    contactTypeName?: string;
  
    address1?: string;
    address2?: string;
    country?: string;
    state!: string;
    city!: string;
    zipCode?: string;
  
    phone1?: string;
    phone1Ext?: string;
    phone2?: string;
    phone2Ext?: string;
    alternatePhone?: string;
    alternatePhoneExt?: string;
  
    workEmail?: string;
    personalEmail?: string;
    skypeId?: string;
  
    activeStatus?: boolean;
  }
  