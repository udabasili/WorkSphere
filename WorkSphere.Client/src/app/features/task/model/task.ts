/**
 * {
 *     public class ProjectTask : BaseEntity
 *     {
 *         [Required(ErrorMessage = "Task name is required")]
 *         public string? Name { get; set; }
 *         public string? Description { get; set; }
 *
 *         public int? Order { get; set; }
 *
 *         public Status Status { get; set; }
 *
 *         //navigation properties
 *         [ForeignKey("Employee")]
 *
 *         public int? EmployeeID { get; set; }
 *
 *         [JsonIgnore]
 *         public virtual Employee? Employee { get; set; }
 *
 *         // Foreign key for the Project the task belongs to
 *         [ForeignKey("Project")]
 *         public int? ProjectID { get; set; }
 *
 *         [JsonIgnore]
 *         public virtual Project? Project { get; set; }
 *     }
 * }
 */
import {Employee} from '../../employees/model/employee';
import {Project} from '../../project/models/project';

export class Task {
  id: number;
  name: string;
  description: string;
  order: number;
  status: string;
  employeeID: number;
  employee: Employee;
  projectID: number;
  project: Project

  constructor(id: number, name: string, description: string, order: number, status: string, employeeID: number, employee: Employee, projectID: number, project: Project) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.order = order;
    this.status = status;
    this.employeeID = employeeID;
    this.employee = employee;
    this.projectID = projectID;
    this.project = project;
  }
}
