import {Project} from '../../project/models/project';
import {Salary} from '../../salary/model/salary';

export class ProjectManager {
  id?: number;
  firstName: string;
  lastName: string;
  email: string;
  employmentDate: Date | string;
  salaryID?: number;
  salary?: Salary;
  managedProjects: Project[];

  constructor(id: number = null, firstName: string, lastName: string, email: string, employmentDate: Date, salaryID?: number, salary?: Salary, managedProjects: Project[] = []) {
    this.id = id;
    this.firstName = firstName;
    this.lastName = lastName;
    this.email = email;
    this.employmentDate = employmentDate;
    this.salaryID = salaryID;
    this.salary = salary;
    this.managedProjects = managedProjects;

  }
}
