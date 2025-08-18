export class LeadModel {
  id!: number;
  clientId!: number;
  category!: string;
  leadType!: string;
  statusOfLead!: string;
  departmentId!: number;
  managerId!: number;
  managerName?: string;
  managerTitle?: string;
  assignedToId!: string;
  assignedTo?: string;
  generatedById?: string;
  generatedBy?: string;
  called?: boolean;
  isStaffConsultant?: boolean;
  isClientManager?: boolean;
  referredBy?: string;
  approximateAmount?: number;
  source?: string;
  leadInformation?: string;
  result?: string;
  reminderDateTime?: Date = new Date();
}



export class LeadListModel {
  id!: number;
  clientId!: number;
  category!: string;
  leadType!: string;
  statusOfLead!: string;
  departmentId!: number;
  managerId!: number;
  assignedToId!: string;
  generatedById?: string;
  called?: boolean;
  isStaffConsultant?: boolean;
  isClientManager?: boolean;
  referredBy?: string;
  approximateAmount?: number;
  source?: string;
  leadInformation?: string;
  result?: string;
  reminderDateTime?: Date = new Date();

  // Extended properties from joined tables
  clientName?: string;
  managerName?: string;
  managerTitle?: string;
  managerContact?: string;
  managerEmail?: string;
  assignedTo?: string;
  generatedBy?: string;
  departmentName?: string;
}