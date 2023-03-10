using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using clo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace clo.Controllers
{
    [Route("~/api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _service;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> List(int page = 1, int pageSize = 10)
        {
            var employees = await _service.List(page, pageSize);
            return Ok(employees);
        }

        [Route("{name}")]
        [HttpGet]
        public async Task<IActionResult> Search(string name)
        {
            var employee = await _service.Search(name);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            Stream stream = null;
            if (Request.ContentType?.StartsWith("multipart/form-data") ?? false)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                if (file == null)
                    return BadRequest();

                stream = file.OpenReadStream();
            }
            else
            {
                stream = Request.Body;
            }
            using var reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true);
            var bodyStr = await reader.ReadToEndAsync();
            if (await _service.Upload(bodyStr) == false)
                return BadRequest();

            return StatusCode(StatusCodes.Status201Created, null);
        }
    }
}