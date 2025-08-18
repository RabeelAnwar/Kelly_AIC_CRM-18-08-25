import { Component, Input } from '@angular/core';
import { ClientModel } from '../../../models/client/client-model';
import { Router } from '@angular/router';
import { ClientManagerModel } from '../../../models/client/client-manager-model';
import { LeadModel } from '../../../models/lead/lead-model';
import { RequisitionModel } from '../../../models/requisition/requisition-model';

@Component({
  selector: 'app-client-top-nav',
  templateUrl: './client-top-nav.component.html',
  styleUrl: './client-top-nav.component.css'
})
export class ClientTopNavComponent {

  constructor(
    private router: Router,
  ) { }

  @Input() clientData: ClientModel = new ClientModel();
  @Input() managerData: ClientManagerModel = new ClientManagerModel();

  addManager() {
    const manager = new ClientManagerModel();
    manager.clientId = this.clientData.id;
    manager.address1 = this.clientData?.address1;
    manager.address2 = this.clientData?.address2;

    manager.country = this.clientData?.country;
    manager.state = this.clientData?.state;
    manager.city = this.clientData?.city;
    manager.zipCode = this.clientData?.zipCode;

    this.router.navigate(['/AddManager'], {
      state: { manager: manager }
    });
  }


  addLead() {
    const lead = new LeadModel();
    lead.clientId = this.clientData.id;
    lead.managerId = this.managerData.id;

    this.router.navigate(['/add-leads'], {
      state: { lead: lead }
    });
  }

  addPipeline() {
    this.router.navigate(['/AddPipeline'], {
      state: { client: this.clientData }
    });
  }

  addRequisition() {
    const requisition = new RequisitionModel();
    requisition.clientId = this.clientData.id;
    requisition.managerId = this.managerData.id;
    this.router.navigate(['/ClientITRequisition'], {
      state: { requisition: requisition }
    });
  }

}
