export class Salary {

  public id: number
  public basicSalary: number
  public bonus: number
  public deductions: number
  public totalSalary: number
  public employeeID: number

  constructor(id: number, basicSalary: number, bonus: number, deductions: number, totalSalary: number, employeeID: number) {
    this.id = id
    this.basicSalary = basicSalary
    this.bonus = bonus
    this.deductions = deductions
    this.totalSalary = totalSalary
    this.employeeID = employeeID
  }
}
