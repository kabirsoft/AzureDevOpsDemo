using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.IRepositories;
using Moq;
using DataLayer.Models;
using AzureDevOpsDemo.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using UnitTest.Fixtures;

namespace UnitTest.AzureDevOpsDemo
{
    public class Test_employees
    {       

        private readonly EmployeesController _sut; //system_under_test(sut) is convention for controller test
        private readonly Mock<IEmployeeRepo> _employeesRepoMock = new Mock<IEmployeeRepo>();        
        
        private readonly Employee _empObj = new Employee { Id = 1, Name = "john", Address = "Oslo", Email = "john@test.com", SalaryGrade = 61, DOB = Convert.ToDateTime("1991-01-21"), Created = DateTime.Now, Updated = DateTime.Now };

        public Test_employees()
        {
            _sut = new EmployeesController(_employeesRepoMock.Object);
        }

        [Fact]
        public async Task Test_Employees_GetAll_returnStatus200()
        {
            //Arrange            
            _employeesRepoMock.Setup(x=>x.GetAll()).ReturnsAsync(EmployeeFixture.GetEmployees());

            //Act
            var result =(OkObjectResult) await _sut.GetAll();

            //Assert
            result.GetType().Should().Be(typeof(OkObjectResult));
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task Test_Employees_GetAll_returnStatus204() // 204, return empty list []
        {
            //Arrange            
            _employeesRepoMock.Setup(x => x.GetAll()).ReturnsAsync(new List<Employee>());

            //Act
            var result = (NoContentResult) await _sut.GetAll();

            //Assert
            result.GetType().Should().Be(typeof(NoContentResult));
            result.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task Test_Employees_GetAll()
        {
            //Arrange 
            _employeesRepoMock.Setup(x => x.GetAll()).ReturnsAsync(EmployeeFixture.GetEmployees());            

            //Act
            var result = await _sut.GetAll();

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result;
            objectResult.Value.Should().BeOfType<List<Employee>>();


        }

        [Fact]
        public async Task Test_Employees_Get()
        {
            //Arrange
            var empId = 1;
            _employeesRepoMock.Setup(x => x.Get(empId)).ReturnsAsync(_empObj);

            //Act
            var result = await _sut.Get(empId);
            
            //Assert
            //result.Should().BeOfType<OkObjectResult>();
            //var objectResult = (OkObjectResult)result;
            result.Value.Should().BeOfType<Employee>();
            result.Value.Email.Should().Be("john@test.com");

        }
        [Fact]
        public async Task Test_Employees_Get_employeeNotFound_return404()
        {
            //Arrange
            var empId = 11;
            var emp = new Employee() { };
            emp = null;
            _employeesRepoMock.Setup(x => x.Get(empId)).ReturnsAsync(emp);

            //Act
            var result =   await _sut.Get(empId);            

            //Assert        
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async void Test_Employees_Post()
        {
            //Arrange
            var newEmp = new Employee {Id = 42, Name = "Lynda", Address = "Tromsø", Email = "lynda@test.com", SalaryGrade = 60, DOB = Convert.ToDateTime("1991-01-21"), Created = DateTime.Now, Updated = DateTime.Now };
            _employeesRepoMock.Setup(x=>x.AddNew(newEmp)).ReturnsAsync(newEmp);
            //Act
            var result = await _sut.Create(newEmp);
            

            //Assert
            _employeesRepoMock.Verify(x => x.AddNew(newEmp), Times.Exactly(3));
            
        }

        [Fact]
        public async void Test_Employees_Put()
        {
            //Arrange
            const int updatedId = 123;
            Employee updateEmp = new Employee() { Id = updatedId, Name="Alex", Address="Oslo", Email="alex@test.com", SalaryGrade = 61, DOB = Convert.ToDateTime("1991-01-21"), Updated = DateTime.Now };
            _employeesRepoMock.Setup(x => x.Get(updatedId)).ReturnsAsync(updateEmp);

            //Act
            var result = await _sut.Update(updatedId, updateEmp);
            
            //Assert
            _employeesRepoMock.Verify(x=>x.Update(updateEmp), Times.Exactly(1));
            result.Value.Email.Should().Be("alex@test.com");
            result.Value.Id.Should().Be(123);

        }

        [Fact]
        public async void Test_Employees_Delete()
        {
            //Arrange
            const int deletedId = 1;
            _employeesRepoMock.Setup(x => x.Get(deletedId)).ReturnsAsync(_empObj);

            //Act
            var result = await _sut.Delete(deletedId);

            //Assert        
            _employeesRepoMock.Verify(x => x.Remove(_empObj), Times.Exactly(1));
            result.Value.Id.Should().Be(1);

        }
    }
}
