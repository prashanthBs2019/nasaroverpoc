using System.IO;
using System.Data;
using Nasa.Rover.Service;
using System.Web;
using System.Web.Mvc;
using LumenWorks.Framework.IO.Csv;
using Nasa.Rover.DataTransferObject;

namespace Nasa.Rover.UI.Controllers
{
    public class RoverController : Controller
    {
        // GET: Rover
        public ActionResult MarsRover()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarsRover(HttpPostedFileBase upload)
        {
            if(ModelState.IsValid)
            {
                if(upload != null && upload.ContentLength >0)
                {
                    if(upload.FileName.EndsWith(".csv"))
                    {
                        Stream stream = upload.InputStream;
                        DataTable csvDataTable = CsvFileService.LoadCsvData(stream);
                        var result = RoverDeploymentService.MarsRoverResolver(csvDataTable);
                        ViewData["csvData"] = result;
                        ViewData["SurfaceData"] = RoverDeploymentService.CreatePlateau(5, 5);
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("File", Constants.FILEFORMATNOTSUPPORTED);
                    }
                }
                else
                {
                    ModelState.AddModelError("File", Constants.EMPTYFILEERROR);
                }
            }

            return View();
        }

        /// <summary>
        /// This method is used to see any active area from Rover movements
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public static bool IsActivePath(int col,int row)
        {
            if (col < 0 || row < 0) return false;
            return RoverDeploymentService.IsActiveArea(col, row);
        }
    }
}