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

//status: Active, Inactive, Completed
export type Status = 'Active' | 'Inactive' | 'Completed';

export class Task {
  id: number;
  name: string;
  description: string;
  order: number;
  status: Status | number;
  employeeID: number;
  projectID: number;
  numOfTeamMembers: number;

  constructor(id: number, name: string, description: string, order: number, status: Status, employeeID: number, projectID: number, numOfTeamMembers: number) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.order = order;
    this.status = status;
    this.employeeID = employeeID;
    this.projectID = projectID;
    this.numOfTeamMembers = numOfTeamMembers;
  }
}
