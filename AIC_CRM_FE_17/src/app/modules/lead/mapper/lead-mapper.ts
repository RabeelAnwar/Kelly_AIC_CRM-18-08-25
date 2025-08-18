import { Injectable } from '@angular/core';
import { LeadListModel, LeadModel } from '../../../models/lead/lead-model';

@Injectable({
    providedIn: 'root'
})
export class LeadMappingService {

    constructor() { }

    mapToLeadModel(leadList: LeadListModel): LeadModel {
        return {
            id: leadList.id,
            clientId: leadList.clientId,
            category: leadList.category,
            leadType: leadList.leadType,
            statusOfLead: leadList.statusOfLead,
            departmentId: leadList.departmentId,
            managerId: leadList.managerId,
            managerName: leadList.managerName,
            managerTitle: leadList.managerTitle,
            assignedToId: leadList.assignedToId,
            assignedTo: leadList.assignedTo,
            generatedById: leadList.generatedById,
            generatedBy: leadList.generatedBy,
            called: leadList.called,
            isStaffConsultant: leadList.isStaffConsultant,
            isClientManager: leadList.isClientManager,
            referredBy: leadList.referredBy,
            approximateAmount: leadList.approximateAmount,
            source: leadList.source,
            leadInformation: leadList.leadInformation,
            result: leadList.result,
            reminderDateTime: leadList.reminderDateTime
        };
    }
}
