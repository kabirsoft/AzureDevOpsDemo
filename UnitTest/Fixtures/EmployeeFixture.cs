using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Fixtures
{
    public static class EmployeeFixture
    {
        public static List<Employee> GetEmployees() => 
        new()
        {
            new Employee{Id=1, Name="test1", Address="Oslo", Email="test1@test.com", SalaryGrade=61, DOB=Convert.ToDateTime("1991-01-21"), Created=DateTime.Now, Updated=DateTime.Now },
            new Employee{Id=2, Name="test2", Address="Tromsø", Email="test2@test.com", SalaryGrade=62, DOB=Convert.ToDateTime("1992-02-22"), Created=DateTime.Now, Updated=DateTime.Now }
        };        
    }
}
