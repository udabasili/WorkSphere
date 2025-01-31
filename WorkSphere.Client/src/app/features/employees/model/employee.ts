import {Salary} from '../../salary/model/salary';
import {ProjectTask} from '../../project-task/models/project-task';

export class Employee {
  id?: number;
  firstName: string;
  lastName: string;
  email: string;
  employmentDate: Date | string;
  salaryID?: number;
  salary?: Salary;
  projectTasks?: ProjectTask[];

  // @ts-ignore
  constructor(id?: number, firstName: string, lastName: string, email: string, employmentDate: Date, salaryID?: number, salary?: Salary, projectTasks?: ProjectTask[]) {
    this.id = id;
    this.firstName = firstName;
    this.lastName = lastName;
    this.email = email;
    this.employmentDate = employmentDate;
    this.salaryID = salaryID;
    this.salary = salary;
    this.projectTasks = projectTasks;
  }
}
