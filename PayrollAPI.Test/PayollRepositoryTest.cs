using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayrollAPI.Controllers;
using PayrollAPI.Models;
using PayrollAPI.Repositories;
using System;
using System.IO;
using Xunit;

namespace PayrollAPI.Test
{
    public class PayollRepositoryTest
    {
        private readonly IPayrollRepository _service;
        private readonly PayrollController _controller;

        public PayollRepositoryTest()
        {
            _service = new PayrollServiceFake();
            _controller = new PayrollController(_service);
        }

        [Fact]
        public void GetPayrollReport_Should_ReturnsOk()
        {
            //Arrange

            //Act
            var okResult = _controller.GetPayrollReport();

            //Assert
            Assert.IsType<OkObjectResult>(okResult as OkObjectResult);
        }
        [Fact]
        public void GetPayrollReport_Should_Return_Data()
        {
            //Arrange

            //Act
            var result = _controller.GetPayrollReport();

            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public void PushTimeKeeping_Should_Return_Error_NoFile()
        {
            //Arrange
            IFormFile file = null;
            string expected_status_message = "File not found";
            string expected_status_type = "error";
            ResponseMessage responseMessage;


            //Act
            var response = _controller.PushTimeKeeping(file);
            var result = response as BadRequestObjectResult;
            responseMessage = result.Value as ResponseMessage;
           
            //Assert
            Assert.IsType<BadRequestObjectResult>(response as BadRequestObjectResult);
            Assert.Equal(expected_status_type, responseMessage.MessageStatus);
            Assert.Equal(expected_status_message, responseMessage.MessageText);
        }

        [Fact]
        public void PushTimeLeeping_Should_Return_Error_FileExist()
        {
            //Arrange
            var fake_file_name = "time-report-999.csv";
            string file_path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString(), fake_file_name);
            var stream = new MemoryStream(File.ReadAllBytes(file_path));
            var file_input = new FormFile(stream, 0, stream.Length, "fake_stream_file", fake_file_name);

            //Act
            var response = _controller.PushTimeKeeping(file_input);
            var response_result = response as UnprocessableEntityObjectResult;

            //Assert
            Assert.True(File.Exists(file_path));
            Assert.IsType<UnprocessableEntityObjectResult>(response_result as UnprocessableEntityObjectResult);
        }
    }
}
