using System.Collections.Generic;
using System.Threading.Tasks;
using clo.Models;

namespace clo.Services
{
    public interface IEmployeeService
    {
        public Task<List<Employee>> List(int page, int pageSize);

        public Task<Employee> Search(string name);

        public Task<bool> Upload(string content);
    }
}