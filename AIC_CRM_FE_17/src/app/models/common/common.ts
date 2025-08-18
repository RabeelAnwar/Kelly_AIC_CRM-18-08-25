export class CountryModel {
  id!: number;
  name!: string;
}

export class StateModel {
  id!: number;
  name!: string;
}

export class CityModel {
  id!: number;
  name!: string;
}


export class ContactTypeModel {
  id!: number;
  name!: string;
}

export class SkillMasterModel {
  id!: number;
  name!: string;
}

export class DepartmentModel {
  id!: number;
  name!: string;
  isActive!: boolean;
}


export class CallTypeModel {
  id!: number;
  name!: string;
  isActive!: boolean;
}

export class DocumentTypeModel {
  id!: number;
  documentTypeName!: string;
  userTypeId!: number;
}

export interface DropdownItem {
  id: number;
  name: string;
}
