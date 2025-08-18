import { Component } from '@angular/core';
import { ApiService } from '../../../services/api.service';
import { ToastrService } from 'ngx-toastr';
import { SelectItem } from 'primeng/api';
import { SkillMasterModel } from '../../../models/common/common';

@Component({
  selector: 'app-skill-master',
  templateUrl: './skill-master.component.html',
  styleUrl: './skill-master.component.css'
})
export class SkillMasterComponent {

  
  skillList: SelectItem[] = [];
  skillModel: SkillMasterModel = new SkillMasterModel();

  constructor(
    private apiService: ApiService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.getSkills();
  }

  getSkills(): void {
    this.apiService.getData('Admin/SkillMasterGet').subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.skillList = res.data;
        } else {
          this.toastr.error('Failed to load skills');
        }
      },
      error: () => this.toastr.error('Error fetching skills')
    });
  }

  saveSkill(): void {
    if (!this.skillModel.name || this.skillModel.name.trim() === '') {
      this.toastr.warning('Skill Name is required');
      return;
    }
    this.apiService.saveData('Admin/SkillMasterAddUpdate', this.skillModel).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.toastr.success('Skill saved successfully');
          this.getSkills();
          this.clearInput();
        } else {
          this.toastr.error('Save failed');
        }
      },
      error: () => this.toastr.error('Error saving skill')
    });
  }

  deleteSkill(id: number): void {
    const confirmDelete = confirm('Are you sure you want to delete this skill?');
    if (!confirmDelete) {
      return;
    }

    this.apiService.deleteData('Admin/SkillMasterDelete',  {id: id}).subscribe({
      next: (res: any) => {
        if (res.succeeded) {
          this.toastr.success('Skill deleted successfully');
          this.getSkills();
        } else {
          this.toastr.error('Delete failed');
        }
      },
      error: () => this.toastr.error('Error deleting skill')
    });
  }

  editSkill(skill: SkillMasterModel): void {
    this.skillModel = { ...skill };
  }

  clearInput(): void {
    this.skillModel = new SkillMasterModel();
  }

}
