
export class ProjectTask {

  id: number;
  createdAt: Date;
  updatedAt: Date;
  name: string;
  description: string;
  order: number;
  status: number;
  employeeID: number;
  projectID: number;

  constructor(id: number, createdAt: Date, updatedAt: Date, name: string, description: string, order: number, status: number, employeeID: number, projectID: number) {
    this.id = id;
    this.createdAt = createdAt;
    this.updatedAt = updatedAt;
    this.name = name;
    this.description = description;
    this.order = order;
    this.status = status;
    this.employeeID = employeeID;
    this.projectID = projectID;
  }
}
