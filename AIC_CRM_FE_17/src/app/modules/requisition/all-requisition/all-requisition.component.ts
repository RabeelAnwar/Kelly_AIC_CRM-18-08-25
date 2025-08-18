import { Component } from '@angular/core';
import { RequisitionModel } from '../../../models/requisition/requisition-model';
import { CustomToastService } from '../../../services/custom-toast.service';
import { Router } from '@angular/router';
import { ApiService } from '../../../services/api.service';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-all-requisition',
  templateUrl: './all-requisition.component.html',
  styleUrl: './all-requisition.component.css'
})
export class AllRequisitionComponent {

  requisitionList: RequisitionModel[] = [];
  searchFields: string[] = [];

  constructor(
    private apiService: ApiService,
    private router: Router,
    private toastr: CustomToastService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.getRequisitions();
  }

  getRequisitions() {
    this.apiService.getData('Client/ClientRequisitionsListGet').subscribe({
      next: (response) => {
        this.requisitionList = response?.data || [];

        this.searchFields = Object.keys(this.requisitionList[0]);
        console.log(this.requisitionList);
      },
      error: (err) => {
        this.toastr.error(err || 'Error loading requisition list');
      }
    });
  }

  requisitionDashboard(requisition: RequisitionModel) {
    this.router.navigate(['/RequisitionDashboard', requisition.id], {
      state: { requisition }
    });
  }

  editRequisition(requisition: RequisitionModel) {
    this.router.navigate(['/ClientITRequisition'], {
      state: { requisition }
    });
  }

  deleteRequisition(id: number) {
    if (confirm('Are you sure you want to delete this requisition?')) {
      this.apiService.deleteData('Client/ClientRequisitionDelete', { id }).subscribe({
        next: () => {
          this.toastr.success('Requisition deleted successfully');
          this.getRequisitions();
        },
        error: (err) => {
          this.toastr.error(err || 'Error deleting requisition');
        }
      });
    }
  }


  goToManagerDashboard(requisition: RequisitionModel) {
    this.router.navigate(['/ManagerDashboard', requisition?.managerId]);
  }

  goToclientDashboard(requisition: RequisitionModel) {
    this.router.navigate(['/client-dashboard', requisition?.clientId]);
  }


  addRequisition(data: RequisitionModel) {
    const requisition = new RequisitionModel();
    debugger;
    requisition.clientId = data?.clientId;
    requisition.salesRepId = this.authService.getUserId();
    requisition.internalReqCoordinatorId = this.authService.getUserId();
    this.router.navigate(['/ClientITRequisition'], {
      state: { requisition: requisition }
    });
  }


  onStatusChange(event: any, rowData: RequisitionModel) {
    rowData.status = event.target.checked;
    
    this.apiService.saveData('Client/ClientRequisitionStatusUpdate', rowData).subscribe({
      next: (res) => {
        if (res.succeeded) {
          this.toastr.success('saved successfully');
        } else {
          this.toastr.error(res.message || 'Failed to save');
        }
      },
      error: (err) => {
        this.toastr.error(err?.message || 'Error saving');
      }
    });
  }

}
