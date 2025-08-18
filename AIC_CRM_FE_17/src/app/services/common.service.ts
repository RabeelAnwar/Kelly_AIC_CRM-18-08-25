import { Injectable } from '@angular/core';
import { environment } from '../../environment/environmemt';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
// import { ReportModalComponent } from '../features/report-modal/report-modal.component';
// import { LoaderComponent } from '../common/loader/loader.component';
import * as XLSX from 'xlsx';
import { ApiService } from './api.service';
declare const $: any;

@Injectable({
  providedIn: 'root',
})
export class CommonService {
  
  basePath = environment.basePath;

  isRound: any = localStorage.getItem('roundVal');
  cmpName: any = localStorage.getItem('CmpName');
  cmpId: any = localStorage.getItem('cmpId');
  cmpImage: any = localStorage.getItem('Logo');
  reportFormat: any = localStorage.getItem('reportFormat');
  costCenter = localStorage.getItem('costCenter');

  counter: any = 0;
  timeout: any;
  timer_on: any = 0;

  // modalRef: BsModalRef;
  constructor(private modalService: BsModalService, private apiService: ApiService) {}

  roundVal(amount: any) {
    if (this.isRound == "true") {
      return Math.round(amount);
    } else {
      return amount;
    }
  }

  getLogo() {
    return `${this.basePath}/Companies/${this.cmpName}/CompanyLogo/${this.cmpImage}`;
  }

  async getJobList(status: any) {
    if(this.costCenter == 'true')
      return await this.apiService.getDataById('Sale/GetJobNumber', {isFinished: status}).toPromise();
    else 
    return [];
  }

  async getLocation() {
    return await this.apiService.getDataById('Admin/GetLocationById', { companyId: this.cmpId }).toPromise();
  }


  viewReport(url: any) {

    url = this.reportFormates(url);

    url += `&Logo=${this.getLogo()}`;
    const modalConfig: ModalOptions = {
      class: 'custom-modal-width',
      initialState: {
        reportUrl: url,
      },
    };
    // this.modalRef = this.modalService.show(ReportModalComponent, modalConfig);
  }

  reportFormates(url: any){
    if(url.includes('SaleInvoice')){
      if(this.reportFormat == "1"){
        url = url.replace('SaleInvoice', 'SaleInvoice1')
      }
    }

    if(url.includes('SaleReturn')){
      if(this.reportFormat == "1"){
        url = url.replace('SaleReturn', 'SaleReturn1')
      }
    }

    if(url.includes('PurchaseInvoice')){
      if(this.reportFormat == "1"){
        url = url.replace('PurchaseInvoice', 'PurchaseInvoice1')
      }
    }
    
    if(url.includes('PurchaseReturn')){
      if(this.reportFormat == "1"){
        url = url.replace('PurchaseReturn', 'PurchaseReturn1')
      }
    }

    return url;
  }

  ExportFiles(fileName: any, fileFormate: any) {
    
    let cloneTable = $('#export').clone();
    cloneTable.find('.d-none, .noPrint').remove();
    
    if (fileFormate == 'xlxs') {
      const ws: XLSX.WorkSheet = XLSX.utils.table_to_sheet(cloneTable[0]);

      /* new format */
      var fmt = '0.00';
      /* change cell format of range B2:D4 */
      var range = { s: { r: 1, c: 1 }, e: { r: 2, c: 100000 } };
      for (var R = range.s.r; R <= range.e.r; ++R) {
        for (var C = range.s.c; C <= range.e.c; ++C) {
          var cell = ws[XLSX.utils.encode_cell({ r: R, c: C })];
          if (!cell || cell.t != 'n') continue; // only format numeric cells
          cell.z = fmt;
        }
      }
      const wb: XLSX.WorkBook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
      var fmt = '@';
      wb.Sheets['Sheet1']['F'] = fmt;

      /* save to file */
      XLSX.writeFile(wb, `${fileName}.xlsx`);
    }

    if(fileFormate == 'pdf'){
      $(cloneTable[0]).printThis()
    }
  }

  // showLoader() {
  //   this.modalRef = this.modalService.show(LoaderComponent, {
  //     class: 'modal-dialog-centered custom-loader',
  //     backdrop: true,
  //     ignoreBackdropClick: true,
  //     keyboard: false,
  //   });

  //   this.startCount();
  // }

  // hideLoader() {
  //   this.modalRef.hide();
  //   this.stopCount();
  // }

  startCount() {
    return new Promise((resolve, reject) => {
      try {
        if (!this.timer_on) {
          this.timer_on = 1;
          // resolve(this.timedCount());
        }
      } catch (error) {
        reject(error);
      }
    });
  }

  // timedCount() {
  //   return new Promise((resolve, reject) => {
  //     try {
  //       document.getElementById('count-second').innerHTML = this.counter;
  //       this.counter++;
  //       this.timeout = setTimeout(() => {
  //         resolve(this.timedCount());
  //       }, 1000);
  //     } catch (error) {
  //       reject(error);
  //     }
  //   });
  // }

  // stopCount() {
  //   return new Promise((resolve) => {
  //     try {
  //       clearTimeout(this.timeout);
  //       this.timer_on = 0;
  //       this.counter = 0;
  //       resolve;
  //     } catch (err) {
  //       console.log(err);
  //       return false;
  //     }
  //   });
  // }
}
