/**
 *  public class Project : BaseEntity
 *  {
 *
 *
 *      [Required(ErrorMessage = "Project name is required")]
 *      public string? Name { get; set; }
 *
 *      public string? Description { get; set; }
 *
 *      [Required(ErrorMessage = "Start date is required")]
 *      [DataType(DataType.Date)]
 *      public DateTime StartDate { get; set; }
 *
 *      [Required(ErrorMessage = "End date is required")]
 *      [DataType(DataType.Date)]
 *      public DateTime EndDate { get; set; }
 *
 *      public Status Status { get; set; }
 *
 *      //navigation properties
 *      // Foreign key for the Project Manager
 *      [ForeignKey("ProjectManager")]
 *      public int? ProjectManagerID { get; set; }
 *      public virtual ProjectManager? ProjectManager { get; set; }
 *
 *      // Relationship: One Project can have multiple tasks
 *      public virtual ICollection<ProjectTask>? ProjectTasks { get; set; }
 *
 *  }
 */


export class Project {
  id: number;
  name: string;
  description: string;
  startDate: Date | string;
  endDate: Date | string;
  status: number;
  projectManagerID?: number;
  projectManager?: any;
  projectTasks?: any[];

  constructor(id: number = null, name: string, description: string, startDate: Date, endDate: Date, status: number, projectManagerID?: number, projectManager?: any, projectTasks?: any[]) {
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
