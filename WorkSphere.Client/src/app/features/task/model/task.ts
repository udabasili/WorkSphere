//status: Active, Inactive, Completed
export type Status = 'Active' | 'Inactive' | 'Completed';

export class Task {
  id: number;
  name: string;
  description: string;
  order: number;
  status: Status | number;
  duration: number;
  employeeIDs: Array<number>;
  projectID: number;
  numOfTeamMembers?: number;

  constructor(id: number, name: string, description: string, order: number, status: Status, duration: number, employeeIDs: Array<number>, projectID: number, numOfTeamMembers?: number) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.order = order;
    this.status = status;
    this.duration = duration;
    this.employeeIDs = employeeIDs;
    this.projectID = projectID;
    this.numOfTeamMembers = numOfTeamMembers;
  }
}
