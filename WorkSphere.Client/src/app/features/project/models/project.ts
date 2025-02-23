import {ProjectManager} from '../../project-manager/model/project-manager';
import {ProjectTask} from '../../project-task/models/project-task';


export class Project {
  id: number;
  name: string;
  description: string;
  startDate: Date | string;
  endDate: Date | string;
  status: number;
  projectManagerID?: number;
  projectManager?: ProjectManager
  projectTasks?: ProjectTask[];

  constructor(id: number = null, name: string, description: string, startDate: Date, endDate: Date, status: number, projectManagerID: number = null, projectManager?: ProjectManager, projectTasks?: ProjectTask[]) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.startDate = startDate;
    this.endDate = endDate;
    this.status = status;
    this.projectManagerID = projectManagerID;
    this.projectManager = projectManager;
    this.projectTasks = projectTasks;
  }
}
