using System.IO;
using System.Data;
using LumenWorks.Framework.IO.Csv;

namespace Nasa.Rover.Service
{
    public static class CsvFileService
    {
        /// <summary>
        /// This method is used to load data from csv file
        /// </summary>
        /// <param name="importData"></param>
        /// <returns></returns>
        public static DataTable LoadCsvData(Stream importData)
        {
            DataTable csvDataTable = new DataTable();
            using (CsvReader csvReader = new CsvReader(new StreamReader(importData), false))
            {
                csvDataTable.Load(csvReader);
            }
            return csvDataTable;
        }
    }
}
