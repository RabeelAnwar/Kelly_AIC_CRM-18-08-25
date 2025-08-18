export class ConsultantModel {
  id!: number;
  assignedToId?: string;

  firstName!: string;
  middleName?: string;
  lastName!: string;

  currentPosition?: string;
  visaStatus?: string;
  currentRate?: string;
  resumeFile?: File;
  resume?: string;
  resumeSearchText?: string;
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
  skypeId?: string;
  linkedInUrl?: string;
  linkedInImage?: string;

  notes?: string;

  callRecords?: string;

  creationTime?: Date | string;
  requisition?: ConsultantRequisitionsDto[];
}

export class ConsultantRequisitionsDto {
  clientName!: string;
  requisitionId!: number;
}