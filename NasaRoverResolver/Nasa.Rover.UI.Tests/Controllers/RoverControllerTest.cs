using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Nasa.Rover.UI.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Nasa.Rover.UI.Tests.Controllers
{
    [TestClass]
    public class RoverControllerTest
    {
        private Stream baseStream;
        [TestInitialize]
        public void SetUp()
        {
            string path = string.Concat(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"\Data\testData.csv");
            StreamReader readFile = new StreamReader(path);
            baseStream = readFile.BaseStream;
            // Other stuff
        }
        [TestMethod]
        public void MarsRover()
        {
            // Arrange
            RoverController controller = new RoverController();

            // Act
            ViewResult result = controller.MarsRover() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void FileUpload()
        {
            // Arrange
            RoverController controller = new RoverController();

            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var files = new Mock<HttpFileCollectionBase>();
            var file = new Mock<HttpPostedFileBase>();
            context.Setup(x => x.Request).Returns(request.Object);

            files.Setup(x => x.Count).Returns(1);

            
            file.Setup(x => x.InputStream).Returns(baseStream);
            file.Setup(x => x.ContentLength).Returns((int)baseStream.Length);
            file.Setup(x => x.FileName).Returns("testData.csv");

            files.Setup(x => x.Get(0).InputStream).Returns(file.Object.InputStream);
            request.Setup(x => x.Files).Returns(files.Object);
            request.Setup(x => x.Files[0]).Returns(file.Object);

            // Act
            ViewResult result = controller.MarsRover(file.Object) as ViewResult;

            
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IsActivePath()
        {
            // Arrange
            

            // Act
            bool result = RoverController.IsActivePath(-1,0);

            // Assert
            Assert.IsFalse(result);
        }
       
    }
}
