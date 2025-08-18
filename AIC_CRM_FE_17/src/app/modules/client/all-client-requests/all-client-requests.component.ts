import { Component } from '@angular/core';

@Component({
  selector: 'app-all-client-requests',
  templateUrl: './all-client-requests.component.html',
  styleUrl: './all-client-requests.component.css'
})
export class AllClientRequestsComponent {

    clientsList: any[] = [];
    searchFields: string[] = [];

}
