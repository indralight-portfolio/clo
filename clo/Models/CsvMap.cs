using CsvHelper.Configuration;

namespace clo.Models;

public class CsvMap
{
    public sealed class EmployeeMap : ClassMap<Models.Employee>
    {
        public EmployeeMap()
        {
            Map(m => m.Name);
            Map(m => m.Email);
            Map(m => m.Tel);
            Map(m => m.Joined);
        }
    }
}