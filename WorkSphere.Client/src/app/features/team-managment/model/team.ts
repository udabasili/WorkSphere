import {Employee} from '../../employees/model/employee';

export class TeamManagement {

  id: number;
  projectName: string;
  teamMembers: Employee[];
  projectManager: string;
  numOfCompletedTasks: number;
  numOfPendingTasks: number;

  constructor(id: number, projectName: string, teamMembers: Employee[], projectManager: string, numOfCompletedTasks: number, numOfPendingTasks: number) {
    this.id = id;
    this.projectName = projectName;
    this.teamMembers = teamMembers;
    this.projectManager = projectManager;
    this.numOfCompletedTasks = numOfCompletedTasks;
    this.numOfPendingTasks = numOfPendingTasks;
  }
}
