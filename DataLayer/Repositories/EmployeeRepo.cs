using DataLayer.IRepositories;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly DataContext _db;
        public EmployeeRepo(DataContext db)
        {
            this._db = db;
        }
        public async Task<Employee> AddNew(Employee employee)
        {
            var emp = await _db.Employees.AddAsync(employee);
            await _db.SaveChangesAsync();
            return emp.Entity;
        }

        public async Task<Employee> Get(int id)
        {
            return await _db.Employees.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Employee>> GetAll()
        {
            return await _db.Employees.ToListAsync();
        }

        public async Task<Employee> Remove(Employee emp)
        {
            var r_emp = await Get(emp.Id);
            if (r_emp == null)
            {
                return null;
            }
            _db.Employees.Remove(r_emp);
            await _db.SaveChangesAsync();
            return r_emp;
        }

        public async Task<Employee> Update(Employee emp)
        {
            var emp_db = await Get(emp.Id);
            if (emp_db == null)
            {
                return null;
            }
            emp_db.Name = emp.Name;           
            emp_db.Address = emp.Address;
            emp_db.Email = emp.Email;
            emp_db.SalaryGrade = emp.SalaryGrade;
            emp_db.DOB = emp.DOB;
            emp_db.Updated = DateTime.Now;
            await _db.SaveChangesAsync();
            return emp;
        }
    }
}
