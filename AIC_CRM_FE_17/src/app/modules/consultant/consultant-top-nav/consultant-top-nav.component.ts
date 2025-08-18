import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { ConsultantModel } from '../../../models/consultant/consultant-model';

@Component({
  selector: 'app-consultant-top-nav',
  templateUrl: './consultant-top-nav.component.html',
  styleUrl: './consultant-top-nav.component.css'
})
export class ConsultantTopNavComponent {

  constructor(
    private router: Router,
  ) { }


  @Input() consultantData: ConsultantModel = new ConsultantModel();

  editConsultant() {
    this.router.navigate(['/add-consultant'], {
      state: { consultant: this.consultantData }
    });
  }

  addConsultantReference() {

    this.router.navigate(['/ConsultantReference'], {
      state: { consultant: this.consultantData }
    });

  }

  addTechnicalInterview() {
    this.router.navigate(['/TechnicalInterview'], {
      state: { consultant: this.consultantData }
    });

  }



}
