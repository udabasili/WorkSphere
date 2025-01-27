
import {Salary} from './salary';
import {ProjectTask} from './project-task';

export class Employee {
    firstName: string;
    lastName: string;
    email: string;
    employmentDate: Date;
    fullName: string;
    salaryID: number;
    salary: Salary;
    projectTasks: ProjectTask[];

    constructor(firstName: string, lastName: string, email: string, employmentDate: Date, fullName: string, salaryID: number, salary: Salary, projectTasks: ProjectTask[]) {
        this.firstName = firstName;
        this.lastName = lastName;
        this.email = email;
        this.employmentDate = employmentDate;
        this.fullName = fullName;
        this.salaryID = salaryID;
        this.salary = salary;
        this.projectTasks = projectTasks;
    }
}
