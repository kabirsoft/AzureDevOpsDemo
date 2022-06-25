using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.IRepositories
{
    public interface IEmployeeRepo
    {
        Task<List<Employee>> GetAll();
        Task<Employee> Get(int id);
        Task<Employee> AddNew(Employee employee);
        Task<Employee> Remove(Employee emp);
        Task<Employee> Update(Employee emp);
    }
}
