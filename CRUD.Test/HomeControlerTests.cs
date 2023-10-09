using System;
using System.Collections.Generic;
using System.Linq;
using CRUD.Controllers;
using CRUD.Interface;
using CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CRUD.Test
{
    public class HomeControlerTests
    {

        [Fact]
        public async void GetAll_ReturnsOkResult()
        {
            // Arrange -  setup the testing objects and prepare the prerequisites for your test.
            var mockRepository = new Mock<IStudentRepository>();
            mockRepository.Setup(repo => repo.GetAllStudentsAsync()).ReturnsAsync(new List<Student>());

            var controller = new HomeController(mockRepository.Object);

            // Act -  perform the actual work of the test.
            var result = await controller.GetAll();

            // Assert - verify the result.
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetById_ExistingStudent_ReturnsOkResult()
        {
            // Arrange
            var mockRepository = new Mock<IStudentRepository>();
            var studentId = 1;
            var existingStudent = new Student { Id = studentId, FirstName = "Ivan", LastName = "Black" };
            mockRepository.Setup(repo => repo.GetStudentByIdAsync(studentId)).ReturnsAsync(existingStudent);

            var controller = new HomeController(mockRepository.Object);

            // Act
            var result = await controller.GetById(studentId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task Create_ValidStudent_ReturnsCreatedAtRouteResult()
        {
           
            var mockRepository = new Mock<IStudentRepository>();
            var validStudent = new Student { };
            mockRepository.Setup(repo => repo.AddStudentAsync(validStudent));

            var controller = new HomeController(mockRepository.Object);

            var result = await controller.Create(validStudent);

            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal("GetStudent", createdAtRouteResult.RouteName);
            Assert.Equal(validStudent.Id, createdAtRouteResult.RouteValues["id"]);
        }

        [Fact]
        public async Task Delete_ExistingStudent_ReturnsNoContent()
        {
            
            var mockRepository = new Mock<IStudentRepository>();
            var studentId = 1001;
            mockRepository.Setup(repo => repo.DeleteStudentAsync(studentId));

            var controller = new HomeController(mockRepository.Object);

            var result = await controller.Delete(studentId);


            Assert.IsType<NoContentResult>(result);
        }

    }
}