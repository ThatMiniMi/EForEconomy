namespace Backend.Models;

public class EmployeeContract
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int OrganisationId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string ContractType { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string WorkHours { get; set; } = string.Empty;
    public string Benefits { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}