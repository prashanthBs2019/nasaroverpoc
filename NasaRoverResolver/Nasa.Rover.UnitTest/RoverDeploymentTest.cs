using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.IO;
using Nasa.Rover.Service;
using System.Reflection;
using Nasa.Rover.DataTransferObject;

namespace Nasa.Rover.UnitTest
{
    [TestClass]
    public class RoverDeploymentTest
    {
        private DataTable sampleTestData;

        [TestInitialize]
        public void SetUp()
        {
            sampleTestData = new DataTable();
            string path = string.Concat(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"\data\testData.csv");
            StreamReader readFile = new StreamReader(path);

            sampleTestData = CsvFileService.LoadCsvData(readFile.BaseStream);
        }

        public void Process_Complete_Data_ForGivenInstructions()
        {
            //Arrange

            //Act
            var result = RoverDeploymentService.MarsRoverResolver(sampleTestData);

            //Assert
            Assert.IsTrue(result.Rows.Count == 2);            
        }

        [TestMethod]
        public void Check_Result_From_MarsRoverResolver()
        {
            //Arrange

            //Act
            var result = RoverDeploymentService.MarsRoverResolver(sampleTestData);

            //Assert            
            Assert.AreEqual("1 3 N", result.Rows[0]["Output"]);
            Assert.AreEqual("5 1 E", result.Rows[1]["Output"]);
        }

        [TestMethod]
        public void Dont_Change_Position_ForTurnRight()
        {
            //Arrange
            string input = "2 3 E|RRRR";

            //Act
            var result = RoverDeploymentService.StartProgram(input);

            //Assert
            Assert.AreEqual("2 3 E", result);
        }

        [TestMethod]
        public void Dont_Change_Position_ForTurnLeft()
        {
            //Arrange
            string input = "3 4 E|LLL";

            //Act
            var result = RoverDeploymentService.StartProgram(input);

            //Assert
            Assert.AreEqual("3 4 S", result);
        }

        [TestMethod]
        public void Path_Should_Move_ForMoveForward()
        {
            //Arrange
            string input = "3 4 E|LM";

            //Act
            var result = RoverDeploymentService.StartProgram(input);

            //Assert
            Assert.AreEqual("3 5 N", result);
        }

        [TestMethod]
        public void Instructions_Without_StartPosition()
        {
            //Arrange
            string input = "|LLL";

            //Act
            var result = RoverDeploymentService.StartProgram(input);

            //Assert
            Assert.AreEqual(Constants.INVALIDINSTRUCTIONSFORPOSITION, result);
        }

        [TestMethod]
        public void Instructions_Without_Delimeter()
        {
            //Arrange
            string input = "1 2 ELLL";

            //Act
            var result = RoverDeploymentService.StartProgram(input);

            //Assert
            Assert.AreEqual(Constants.INVALIDINPUTSFORPROGRAM, result);
        }

        [TestMethod]
        public void Instructions_With_InvalidStartPosition()
        {
            //Arrange
            string input = "1 2E|LLL";

            //Act
            var result = RoverDeploymentService.StartProgram(input);

            //Assert
            Assert.AreEqual(Constants.INVALIDINSTRUCTIONSFORPOSITION, result);
        }

        [TestMethod]
        public void Instructions_With_InvalidMovements()
        {
            //Arrange
            string input = "1 2 E|AB CDEF";

            //Act
            var result = RoverDeploymentService.StartProgram(input);

            //Assert
            Assert.AreEqual(Constants.INVALIDINSTRUCTIONSFORPOSITION, result);
        }


        [TestMethod]
        public void Empty_File_ToProcessRover()
        {
            //Arrange
            var data = new DataTable();
            string errorMsg = string.Empty;

            //Act
            try
            {
                 RoverDeploymentService.MarsRoverResolver(data);
            }
            catch(ArgumentNullException ex)
            {
                errorMsg = ex.ParamName;
            }
           

            //Assert
            Assert.AreEqual(Constants.INVALIDINPUTFORPROCESS, errorMsg);
        }

        [TestMethod]
        public void Validate_Plateau_Area()
        {
            //Arrange
            int width = 5;
            int height = 5;

            //Act
            var result = RoverDeploymentService.CreatePlateau(width, height);


            //Assert
            Assert.IsTrue(result.Rows.Count == height+1 && result.Columns.Count == width+1);
        }
    }
}
