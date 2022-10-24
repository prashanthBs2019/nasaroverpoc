using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nasa.Rover.DataTransferObject
{
    public static class Constants
    {
        public static string INVALIDINPUTFORPROCESS = "No data for Rover Process: instructionsToLaunch";
        public static string INPUT = "Input";
        public static string OUTPUT = "Output";
        public static string INVALIDINPUTSFORPROGRAM = "Invalid Inputs/Cannot be empty";
        public static string INVALIDINSTRUCTIONSFORPOSITION = "Position/Instructions are not valid";
        public static string INVALIDFORMATTOPROCESS = "Not valid Instructions, Please check Input format.";
        public static string FILEFORMATNOTSUPPORTED = "This file format is not supported";
        public static string EMPTYFILEERROR = "Please upload your file";
    }
}
