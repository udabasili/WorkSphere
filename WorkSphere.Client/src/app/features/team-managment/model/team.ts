import {Employee} from '../../employees/model/employee';
import {ProjectManager} from '../../project-manager/model/project-manager';

export class Team {

  id: number;
  projectName: string;
  teamMembers: Employee[];
  projectManager: ProjectManager;
  numOfCompletedTasks: number;
  numOfPendingTasks: number;

  constructor(id: number, projectName: string, teamMembers: Employee[], projectManager: ProjectManager, numOfCompletedTasks: number, numOfPendingTasks: number) {
    this.id = id;
    this.projectName = projectName;
    this.teamMembers = teamMembers;
    this.projectManager = projectManager;
    this.numOfCompletedTasks = numOfCompletedTasks;
    this.numOfPendingTasks = numOfPendingTasks;
  }
}
