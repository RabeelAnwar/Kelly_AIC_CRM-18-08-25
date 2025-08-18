export class AllRecruitersModel {
    fromDate?: Date;
    toDate?: Date;

    recruiter?: string;
    submittals?: number;
    interviews?: number;
    starts?: number;
    consultantsAdded?: number;
    callLogsAdded?: number;
    requisitionsDetails?: string[] = [];
}

export class GroupedRecruiterReport {
    recruiter!: string;
    requisitionsDetails: string[] = [];

    constructor(init?: Partial<GroupedRecruiterReport>) {
        Object.assign(this, init);
    }
}