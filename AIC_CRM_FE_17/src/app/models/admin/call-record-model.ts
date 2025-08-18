export class CallRecordModel {
  id!: number;
  clientId?: number;
  leadId?: number;
  managerId?: number;
  consultantId?: number;
  date!: Date;
  typeId?: number;
  record?: string;
  remindStatus?: boolean;
  reminderDate?: Date;
  creatorUserName?: string
}
