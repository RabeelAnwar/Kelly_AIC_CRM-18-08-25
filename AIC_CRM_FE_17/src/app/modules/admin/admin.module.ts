import { NgModule } from '@angular/core';
import { AsyncPipe, CommonModule, DatePipe } from '@angular/common';
import { UserMasterComponent } from './user-master/user-master.component';
import { RouterModule } from '@angular/router';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule, RequiredValidator } from '@angular/forms';
import { DropdownModule } from 'primeng/dropdown';
import { RoleBasedPermissionComponent } from './role-based-permission/role-based-permission.component';
import { CountryComponent } from './country/country.component';
import { StateComponent } from './state/state.component';
import { CityComponent } from './city/city.component';
import { TableModule } from 'primeng/table';
import { TenantMasterComponent } from './tenant-master/tenant-master.component';
import { DepartmentComponent } from './department/department.component';
import { CallTypeComponent } from './call-type/call-type.component';
import { ContactTypeComponent } from './contact-type/contact-type.component';
import { SkillMasterComponent } from './skill-master/skill-master.component';
import { DocumentTypeComponent } from './document-type/document-type.component';
import { CalendarModule } from 'primeng/calendar';
import { EditorModule } from 'primeng/editor';
import { SupportTicketComponent } from './support-ticket/support-ticket.component';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { AddConsultantComponent } from '../consultant/add-consultant/add-consultant.component';
import { ConsultantDirectoryComponent } from '../consultant/consultant-directory/consultant-directory.component';
import { SearchConsultantComponent } from '../consultant/search-consultant/search-consultant.component';
import { SharedModule } from '../../shared.module';
import { provideNgxMask } from 'ngx-mask';
import { ConsultantDashboardComponent } from '../consultant/consultant-dashboard/consultant-dashboard.component';
import { ConsultantTopNavComponent } from '../consultant/consultant-top-nav/consultant-top-nav.component';
import { ConsultantReferenceComponent } from '../consultant/consultant-reference/consultant-reference.component';
import { TechnicalInterviewComponent } from '../consultant/technical-interview/technical-interview.component';
import { LeftConsultantSearchNavComponent } from '../consultant/left-consultant-search-nav/left-consultant-search-nav.component';
import { NgxSliderModule } from '@angular-slider/ngx-slider';
import { LeftClientSearchNavComponent } from '../client/left-client-search-nav/left-client-search-nav.component';
import { AddClientComponent } from '../client/add-client/add-client.component';
import { ClientDirectoryComponent } from '../client/client-directory/client-directory.component';
import { SearchClientsComponent } from '../client/search-clients/search-clients.component';
import { ClientDashboardComponent } from '../client/client-dashboard/client-dashboard.component';
import { ClientTopNavComponent } from '../client/client-top-nav/client-top-nav.component';
import { SearchManagersComponent } from '../client/manager/search-managers/search-managers.component';
import { AddPipelineComponent } from '../client/add-pipeline/add-pipeline.component';
import { AddManagerComponent } from '../client/manager/add-manager/add-manager.component';
import { AllClientRequestsComponent } from '../client/all-client-requests/all-client-requests.component';
import { AllClientPipelineComponent } from '../client/all-client-pipeline/all-client-pipeline.component';
import { LeftManagerSearchNavComponent } from '../client/manager/left-manager-search-nav/left-manager-search-nav.component';
import { ClientManagerDashboardComponent } from '../client/manager/client-manager-dashboard/client-manager-dashboard.component';
import { MyLeadsComponent } from '../lead/my-leads/my-leads.component';
import { AllLeadsComponent } from '../lead/all-leads/all-leads.component';
import { AddLeadComponent } from '../lead/add-lead/add-lead.component';
import { LeadDashboardComponent } from '../lead/lead-dashboard/lead-dashboard.component';
import { RightManagerSearchNavComponent } from '../client/manager/right-manager-search-nav/right-manager-search-nav.component';
import { RequisitionDashboardComponent } from '../requisition/requisition-dashboard/requisition-dashboard.component';
import { AllRequisitionComponent } from '../requisition/all-requisition/all-requisition.component';
import { OpenRequisitionComponent } from '../requisition/open-requisition/open-requisition.component';
import { LeftRequisitionSearchNavComponent } from '../requisition/left-requisition-search-nav/left-requisition-search-nav.component';
import { ClientITRequisitionComponent } from '../requisition/client-itrequisition/client-itrequisition.component';
import { RightRequisitionSearchNavComponent } from '../requisition/right-requisition-search-nav/right-requisition-search-nav.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RPTIndividualComponent } from '../reports/rptindividual/rptindividual.component';
import { AllRecruitersComponent } from '../reports/all-recruiters/all-recruiters.component';
import { RPTOfficeMasterComponent } from '../reports/rptoffice-master/rptoffice-master.component';
import { AuditRptComponent } from '../reports/audit-rpt/audit-rpt.component';
import { ReqConsultantInterviewProcessComponent } from '../requisition/req-consultant-interview-process/req-consultant-interview-process.component';
import { ConsultantActivityComponent } from '../requisition/consultant-activity/consultant-activity.component';
import { NgxMatSelectSearchModule } from 'ngx-mat-select-search';

import { TopNavbarComponent } from '../navbar/top-navbar/top-navbar.component';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { CdkAriaLive } from '@angular/cdk/a11y';

@NgModule({
  declarations: [
    //TopNavbarComponent,
    UserMasterComponent,
    RoleBasedPermissionComponent,
    CountryComponent,
    StateComponent,
    CityComponent,
    LeftClientSearchNavComponent,
    TenantMasterComponent,
    MyLeadsComponent,
    AllLeadsComponent,
    AddLeadComponent,
    DepartmentComponent,
    DocumentTypeComponent,
    CallTypeComponent,
    ContactTypeComponent,
    SkillMasterComponent,
    AddClientComponent,
    ClientDirectoryComponent,
    SupportTicketComponent,
    SearchClientsComponent,
    ClientDashboardComponent,
    ClientTopNavComponent,
    SearchManagersComponent,
    AddConsultantComponent,
    ConsultantDirectoryComponent,
    SearchConsultantComponent,
    ConsultantDashboardComponent,
    AddPipelineComponent,
    AddManagerComponent,
    ConsultantTopNavComponent,
    ConsultantReferenceComponent,
    TechnicalInterviewComponent,
    AllClientRequestsComponent,
    AllClientPipelineComponent,
    LeftConsultantSearchNavComponent,
    LeftRequisitionSearchNavComponent,
    LeftManagerSearchNavComponent,
    ClientManagerDashboardComponent,
    LeadDashboardComponent,
    RequisitionDashboardComponent,
    ClientITRequisitionComponent,
    RightManagerSearchNavComponent,
    AllRequisitionComponent,
    LeftRequisitionSearchNavComponent,
    OpenRequisitionComponent,
    RightRequisitionSearchNavComponent,
    DashboardComponent,
    RPTIndividualComponent,
    AllRecruitersComponent,
    RPTOfficeMasterComponent,
    AuditRptComponent,
    ReqConsultantInterviewProcessComponent,
    ConsultantActivityComponent,
  ],
  imports: [
    CommonModule,
    NgSelectModule,
    NgxMatSelectSearchModule,
    MatSelectModule,
    MatInputModule,
    MatFormFieldModule,
    MatAutocompleteModule,
    AsyncPipe,
    DropdownModule,
    FormsModule,
    TableModule,
    CalendarModule,
    EditorModule,
    ToastModule,
    SharedModule,
    NgxSliderModule,
    CKEditorModule,
    RouterModule.forChild([
      { path: 'home', component: DashboardComponent },
      { path: 'user-master', component: UserMasterComponent },
      {
        path: 'role-based-permission',
        component: RoleBasedPermissionComponent,
      },
      { path: 'country-master', component: CountryComponent },
      { path: 'state-master', component: StateComponent },
      { path: 'city-master', component: CityComponent },
      { path: 'tenant-master', component: TenantMasterComponent },
      { path: 'my-leads', component: MyLeadsComponent },
      { path: 'all-leads', component: AllLeadsComponent },
      { path: 'add-leads', component: AddLeadComponent },
      { path: 'department', component: DepartmentComponent },
      { path: 'document-type', component: DocumentTypeComponent },
      { path: 'call-type', component: CallTypeComponent },
      { path: 'contact-type', component: ContactTypeComponent },
      { path: 'skill-master', component: SkillMasterComponent },
      { path: 'add-client', component: AddClientComponent },
      { path: 'client-directory', component: ClientDirectoryComponent },
      { path: 'search-client', component: SearchClientsComponent },
      { path: 'client-dashboard/:id', component: ClientDashboardComponent },
      { path: 'support-ticket', component: SupportTicketComponent },
      { path: 'add-consultant', component: AddConsultantComponent },
      { path: 'consultant-directory', component: ConsultantDirectoryComponent },
      { path: 'search-consultant', component: SearchConsultantComponent },
      {
        path: 'consultant-dashboard/:id',
        component: ConsultantDashboardComponent,
      },
      { path: 'AddPipeline', component: AddPipelineComponent },
      { path: 'AddManager', component: AddManagerComponent },
      { path: 'ConsultantReference', component: ConsultantReferenceComponent },
      { path: 'TechnicalInterview', component: TechnicalInterviewComponent },
      { path: 'SearchManagers', component: SearchManagersComponent },
      { path: 'AllClientRequests', component: AllClientRequestsComponent },
      { path: 'AllClientPipeline', component: AllClientPipelineComponent },
      {
        path: 'ManagerDashboard/:id',
        component: ClientManagerDashboardComponent,
      },
      { path: 'LeadDashboard/:id', component: LeadDashboardComponent },
      {
        path: 'RequisitionDashboard/:id',
        component: RequisitionDashboardComponent,
      },
      { path: 'AllRequisition', component: AllRequisitionComponent },
      { path: 'OpenReqs', component: OpenRequisitionComponent },
      { path: 'ClientITRequisition', component: ClientITRequisitionComponent },
      { path: 'ClientITRequisition', component: ClientITRequisitionComponent },
      { path: 'RPTIndividual', component: RPTIndividualComponent },
      { path: 'AllRecruiters', component: AllRecruitersComponent },
      { path: 'RPTOfficeMaster', component: RPTOfficeMasterComponent },
      { path: 'AllActivities', component: AuditRptComponent },
      {
        path: 'ReqConsultantInterviewProcess',
        component: ReqConsultantInterviewProcessComponent,
      },
      { path: 'ConsultantActivity', component: ConsultantActivityComponent },
    ]),
    CdkAriaLive,
  ],
  exports: [
    RoleBasedPermissionComponent,
    UserMasterComponent,
    CountryComponent,
    StateComponent,
    CityComponent,
    LeftClientSearchNavComponent,
    TenantMasterComponent,
    MyLeadsComponent,
    AllLeadsComponent,
    AddLeadComponent,
    DepartmentComponent,
    DocumentTypeComponent,
    CallTypeComponent,
    ContactTypeComponent,
    SkillMasterComponent,
    AddClientComponent,
    ClientDirectoryComponent,
    SearchClientsComponent,
    SupportTicketComponent,
    AddConsultantComponent,
    ConsultantDirectoryComponent,
    SearchConsultantComponent,
    ClientDashboardComponent,
    ClientTopNavComponent,
    ConsultantDashboardComponent,
    AddPipelineComponent,
    AddManagerComponent,
    ConsultantTopNavComponent,
    ConsultantReferenceComponent,
    TechnicalInterviewComponent,
    SearchManagersComponent,
    AllClientRequestsComponent,
    AllClientPipelineComponent,
    LeftConsultantSearchNavComponent,
    LeftManagerSearchNavComponent,
    RightManagerSearchNavComponent,
    LeftRequisitionSearchNavComponent,
    ClientManagerDashboardComponent,
    LeadDashboardComponent,
    RequisitionDashboardComponent,
    ClientITRequisitionComponent,
    LeftRequisitionSearchNavComponent,
    OpenRequisitionComponent,
    AllRequisitionComponent,
    RightRequisitionSearchNavComponent,
    RPTIndividualComponent,
    AllRecruitersComponent,
    RPTOfficeMasterComponent,
    AuditRptComponent,
    ReqConsultantInterviewProcessComponent,
    ConsultantActivityComponent,
  ],
  providers: [DatePipe, MessageService, provideNgxMask()],
})
export class AdminModule {}
