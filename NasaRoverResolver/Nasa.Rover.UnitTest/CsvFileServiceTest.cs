using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nasa.Rover.Service;
using System.IO;
using System.Reflection;
using System.Data;

namespace Nasa.Rover.UnitTest
{
    [TestClass]
    public class CsvFileServiceTest
    {       
        [TestMethod]        
        public void CsvFile_Should_have_data()
        {
            //Arrange
            string path = string.Concat(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"\data\testData.csv");
            StreamReader readFile = new StreamReader(path);

            //Act
            var result = CsvFileService.LoadCsvData(readFile.BaseStream);

            //Assert
            Assert.IsTrue(result?.Rows.Count > 0);
        }        
    }
}
