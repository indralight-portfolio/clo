using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clo.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using X.PagedList;

namespace clo.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ILogger _logger;
        private readonly CloContext _cloContext;

        public EmployeeService(ILogger<EmployeeService> logger, CloContext cloContext)
        {
            _logger = logger;
            _cloContext = cloContext;
        }

        public async Task<List<Employee>> List(int page, int pageSize)
        {
            var employees = await _cloContext.Employees.OrderBy(e => e.Name).ToPagedListAsync(page, pageSize);
            return employees.ToList();
        }

        public async Task<Employee> Search(string name)
        {
            var employee = await _cloContext.Employees.FindAsync(name);
            return employee;
        }

        public async Task<bool> Upload(string content)
        {
            _logger.LogInformation(content.Replace(Environment.NewLine, string.Empty));

            if (string.IsNullOrEmpty(content))
            {
                return false;
            }

            var employees = ParseJson(content) ?? await ParseCsv(content);
            if (employees == null)
                return false;

            foreach (var employee in employees)
            {
                var entry = _cloContext.Employees.Find(employee.Name);
                if (entry == null)
                {
                    _cloContext.Employees.Add(employee);
                }
                else
                {
                    _cloContext.Employees.Update(employee);
                }

                await _cloContext.SaveChangesAsync();
            }

            return true;
        }

        private List<Employee> ParseJson(string bodyStr)
        {
            try
            {
                var employees = JsonConvert.DeserializeObject<List<Employee>>(bodyStr);
                return employees;
            }
            catch
            {
                return null;
            }
        }

        private async Task<List<Employee>> ParseCsv(string bodyStr)
        {
            try
            {
                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                writer.Write(bodyStr);
                writer.Flush();
                stream.Position = 0;

                using var reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true);
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Encoding = Encoding.UTF8,
                    Delimiter = ",",
                    HasHeaderRecord = false
                };
                using var csv = new CsvReader(reader, config);

                csv.Context.RegisterClassMap<CsvMap.EmployeeMap>();

                var data = csv.GetRecordsAsync<Employee>();
                var employees = new List<Employee>();
                await foreach (var item in data)
                {
                    employees.Add(item);
                }
                return employees;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}