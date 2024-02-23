using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using webapi.Controllers;
using webapi.Interfaces;

namespace ReenbitTestTask.Tests
{
    public class Tests
    {
        private Mock<IBlobStorageService> _blobServiceMock;
        private FileController _controller;


        [SetUp]
        public void Setup()
        {
            _blobServiceMock = new Mock<IBlobStorageService>();
            _controller = new FileController(_blobServiceMock.Object);
        }


        [Test]
        public async Task Upload_ValidFileAndEmail_ReturnsOkResult()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.ContentType).Returns("application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            var email = "test@example.com";

            
            var result = await _controller.Upload(fileMock.Object, email);

            
            Assert.IsInstanceOf<OkResult>(result);
            _blobServiceMock.Verify(b => b.UploadFileToBlobStorage(It.IsAny<IFormFile>(), email), Times.Once);
        }


        [TestCase(null, "test@example.com")]
        [TestCase("invalid/type", "test@example.com")]
        public async Task Upload_InvalidFile_ReturnsBadRequest(string? contentType, string email)
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.ContentType).Returns(contentType);


            var result = await _controller.Upload(fileMock.Object, email);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual("Invalid file type. Allowed type DOCX.", ((BadRequestObjectResult)result).Value);
            _blobServiceMock.Verify(b => b.UploadFileToBlobStorage(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Never);
        }


        [TestCase("invalidemail")]
        [TestCase("test@example")]
        [TestCase("")]
        public async Task Upload_InvalidEmail_ReturnsBadRequest(string email)
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.ContentType).Returns("application/vnd.openxmlformats-officedocument.wordprocessingml.document");

            var result = await _controller.Upload(fileMock.Object, email);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual("Invalid email address", ((BadRequestObjectResult)result).Value);
            _blobServiceMock.Verify(b => b.UploadFileToBlobStorage(It.IsAny<IFormFile>(), It.IsAny<string>()), Times.Never);
        }
    }
}