import {Employee} from '../../employees/model/employee';
import {ProjectManager} from '../../project-manager/model/project-manager';

export class Salary {

  id?: number;
  employeeID?: number;
  projectManagerID?: number;
  employee?: Employee;
  projectManager?: ProjectManager
  basicSalary: number;
  bonus: number;
  deductions: number;
  totalSalary: number;
  createdAt?: Date;
  updatedAt?: Date;

  constructor(data: any) {
    this.id = data.id;
    this.employeeID = data.employeeID;
    this.projectManagerID = data.projectManagerID;
    this.employee = data.employee;
    this.projectManager = data.projectManager;
    this.basicSalary = data.basicSalary;
    this.bonus = data.bonus;
    this.deductions = data.deductions;
    this.totalSalary = data.totalSalary;
    this.createdAt = data.createdAt;
    this.updatedAt = data.updatedAt;
  }

}
