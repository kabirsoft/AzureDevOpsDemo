using DataLayer.IRepositories;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureDevOpsDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepo _repo;
        public EmployeesController(IEmployeeRepo repo)
        {
            _repo = repo;
        }

        //GET: api/employees
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var res = await _repo.GetAll();
                if(res.Count == 0)
                {
                    return NoContent();
                }
                return Ok(res);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from Database");
            }

        }

        // GET: api/employees/2
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> Get(int id)
        {
            try
            {
                var emp = await _repo.Get(id);
                if (emp == null)
                {
                    return NotFound($"Employee with Id:{id} not found");
                }
                return emp;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from Database");
            }
        }

        // POST: api/employees/
        [HttpPost]
        public async Task<ActionResult<Employee>> Create(Employee emp)
        {
            try
            {
                if (emp == null)
                {
                    return BadRequest();
                }
                var newEmp = await _repo.AddNew(emp);
                return CreatedAtAction(nameof(Get), new { id = newEmp.Id }, newEmp);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Saving data to Database");
            }
        }

        // PUT: api/employees/2
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> Update(int id, Employee UpdateEmp)
        {
            try
            {
                var emp = await _repo.Get(id);
                if (emp == null)
                {
                    return NotFound($"Employee with Id:{id} not found");
                }
                if (id != UpdateEmp.Id)
                {
                    return BadRequest("Id mismatch");
                }
                await _repo.Update(UpdateEmp);
                return UpdateEmp;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Saving data to Database");
            }
        }

        // DELETE: api/employees/2
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Employee>> Delete(int id)
        {
            try
            {
                var emp = await _repo.Get(id);
                if (emp == null)
                {
                    return BadRequest($"Employee with Id:{id} not found");
                }
                await _repo.Remove(emp);
                return emp;

            }
            catch (Exception)
            {
                //just to check pipeline working or not, delete this line later
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting data to Database");
            }
        }
    }
}
