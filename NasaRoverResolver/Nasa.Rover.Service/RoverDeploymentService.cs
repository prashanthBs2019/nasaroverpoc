using System;
using System.Collections.Generic;
using System.Linq;
using Nasa.Rover.DataTransferObject.Model;
using Nasa.Rover.DataTransferObject.Enums;
using System.Data;
using Nasa.Rover.DataTransferObject;

namespace Nasa.Rover.Service
{
    /// <summary>
    /// This service is used to Deploy Rover over the Plateau
    /// </summary>
    public static class RoverDeploymentService
    {        
        private static List<RoverMotor> motorTrack = new List<RoverMotor>();//This object hold information Rover movement for given instructions
        private static List<Surface> SurfaceTrack = new List<Surface>();//This object hold informtion complete movement of the surface

        /// <summary>
        /// This method is used to change position of rover to left
        /// </summary>
        /// <param name="prevMotorTrack">this motor object helps in changing position based current position</param>
        /// <returns>the active position of Rover after turning left</returns>
        private static RoverMotor TurnLeft(RoverMotor prevMotorTrack)
        {
            RoverMotor currentMotorTrack = new RoverMotor(prevMotorTrack.CellPosition, prevMotorTrack.RowPosition, prevMotorTrack.TrackerId, prevMotorTrack.RoverPosition);
            switch(prevMotorTrack.RoverPosition)
            {
                case RoverDirection.N:currentMotorTrack.RoverPosition = RoverDirection.W;break;
                case RoverDirection.E: currentMotorTrack.RoverPosition = RoverDirection.N; break;
                case RoverDirection.W: currentMotorTrack.RoverPosition = RoverDirection.S; break;
                case RoverDirection.S: currentMotorTrack.RoverPosition = RoverDirection.E; break;
            }
            return currentMotorTrack;
        }

        /// <summary>
        /// This method is used to set active movements
        /// </summary>
        /// <param name="position">this object says what is the current active position of Rover</param>
        /// <returns>the area which is active</returns>
        private static Surface SetActiveArea(RoverMotor position) => new Surface { Width = position.CellPosition, Height = position.RowPosition, IsActivePath = true };

        /// <summary>
        /// This method is used to move forward based on current position
        /// </summary>
        /// <param name="prevMotorTrack">this object says where is current Rover position</param>
        /// <returns>the active position of Rover after moving</returns>
        private static RoverMotor MoveForward(RoverMotor prevMotorTrack)
        {
            RoverMotor currentMotorTrack = new RoverMotor(prevMotorTrack.CellPosition, prevMotorTrack.RowPosition, prevMotorTrack.TrackerId, prevMotorTrack.RoverPosition);
            switch (prevMotorTrack.RoverPosition)
            {
                case RoverDirection.N: currentMotorTrack.RowPosition++; SurfaceTrack.Add(SetActiveArea(currentMotorTrack)); break;
                case RoverDirection.E: currentMotorTrack.CellPosition++; SurfaceTrack.Add(SetActiveArea(currentMotorTrack)); break;
                case RoverDirection.W: currentMotorTrack.CellPosition--; SurfaceTrack.Add(SetActiveArea(currentMotorTrack)); break;
                case RoverDirection.S: currentMotorTrack.RowPosition--; SurfaceTrack.Add(SetActiveArea(currentMotorTrack)); break;
            }
            return currentMotorTrack;
        }

        /// <summary>
        /// This method is used to change position of rover to left 
        /// </summary>
        /// <param name="prevMotorTrack">this object says what is the current active position of Rover</param>
        /// <returns>the active position of Rover after turning right</returns>
        private static RoverMotor TurnRight(RoverMotor prevMotorTrack)
        {
            RoverMotor currentMotorTrack = new RoverMotor(prevMotorTrack.CellPosition, prevMotorTrack.RowPosition, prevMotorTrack.TrackerId, prevMotorTrack.RoverPosition);
            switch (prevMotorTrack.RoverPosition)
            {
                case RoverDirection.N: currentMotorTrack.RoverPosition = RoverDirection.E; break;
                case RoverDirection.E: currentMotorTrack.RoverPosition = RoverDirection.S; break;
                case RoverDirection.W: currentMotorTrack.RoverPosition = RoverDirection.N; break;
                case RoverDirection.S: currentMotorTrack.RoverPosition = RoverDirection.W; break;
            }
            return currentMotorTrack;
        }

        /// <summary>
        /// This method is used to create Plateau Area based on the defined width and height
        /// </summary>
        /// <param name="width">Coordinate of x</param>
        /// <param name="height">Coordinate of y</param>
        /// <returns>Surface Area</returns>
        public static DataTable CreatePlateau(int width, int height)
        {
            DataTable dtPlateau = new DataTable();
            for(int index =0; index <=width; index++)
            {
                DataColumn dtColumn = new DataColumn
                {
                    ColumnName = index.ToString()
                };
            dtPlateau.Columns.Add(dtColumn);
            }

            for(int index=0; index<=height; index++)
            {
                DataRow dtRow = dtPlateau.NewRow();
                for (int innerIndex = 0; innerIndex <= width; innerIndex++)
                {
                    dtRow[innerIndex.ToString()] = string.Concat(innerIndex, " ", index);
                }
                dtPlateau.Rows.Add(dtRow);
            }

            return dtPlateau;
        }

        /// <summary>
        /// This method is used to convert string to enum for given Rover Direction
        /// </summary>
        /// <param name="direction">Direction</param>
        /// <returns>Enum val</returns>
        private static RoverDirection? ToEnumRoverDirection(string direction)
        {
            switch(direction.ToUpper())
            {
                case "N": return RoverDirection.N;
                case "E": return RoverDirection.E;
                case "W": return RoverDirection.W;
                case "S": return RoverDirection.S;
            }

            return null;
        }

        /// <summary>
        /// This method is used to convert string to enum for given Rover Action
        /// </summary>
        /// <param name="action">action</param>
        /// <returns>Enum val</returns>
        private static RoverAction? ToEnumRoverAction(char action)
        {
            switch(action)
            {
                case 'L': return RoverAction.Left;
                case 'R': return RoverAction.Right;
                case 'M': return RoverAction.Move;
            }
            return null;
        }

        /// <summary>
        /// This method is used to check for given position whether any active movements found
        /// </summary>
        /// <param name="col">x coordinate</param>
        /// <param name="row">y coordinate</param>
        /// <returns>Is active or not</returns>
        public static bool IsActiveArea(int col,int row)
        {
            var result = SurfaceTrack?.Find(s => s.Height == row && s.Width == col);
            return result != null && result.IsActivePath;
        }

        /// <summary>
        /// This method used to resolve mars Rover problem 
        /// </summary>
        /// <param name="instructionsToLaunch"></param>
        /// <returns></returns>
        public static DataTable MarsRoverResolver(DataTable instructionsToLaunch)
        {
            if (instructionsToLaunch == null || instructionsToLaunch.Rows.Count == 0)
                throw new ArgumentNullException(Constants.INVALIDINPUTFORPROCESS);

            DataTable result = new DataTable();
            DataColumn dtInput = new DataColumn(Constants.INPUT);
            DataColumn dtOutput = new DataColumn(Constants.OUTPUT);
            result.Columns.Add(dtInput);
            result.Columns.Add(dtOutput);
            for (int index = 0; index < instructionsToLaunch.Rows.Count; index++)
            {
                var output = RoverDeploymentService.StartProgram(instructionsToLaunch.Rows[index][0].ToString());
                DataRow dtRow = result.NewRow();
                dtRow[0] = instructionsToLaunch.Rows[index][0];
                dtRow[1] = output;
                result.Rows.Add(dtRow);
            }
            return result;
        }

        /// <summary>
        /// This method is used to start Rover based on given Instructions
        /// </summary>
        /// <param name="inputs">Instructions</param>
        /// <returns>Position of end point</returns>
        public static string StartProgram(string inputs)
        {
            motorTrack = new List<RoverMotor>();
            if (string.IsNullOrEmpty(inputs) || !inputs.Contains("|")) return Constants.INVALIDINPUTSFORPROGRAM;

            string[] instructions = inputs.Split('|');
            if(instructions.Length>1)
            {
                string[] begin = instructions[0].Split(' ');
                if (begin.Length > 2 && IsValidInstructions(instructions[1]))
                {
                    RoverMotor roverMotor = new RoverMotor(Convert.ToInt32(begin[0]), Convert.ToInt32(begin[1]), 0, ToEnumRoverDirection(begin[2]).Value);
                    var routeInfo = instructions[1].Trim().ToCharArray().Select(x => ToEnumRoverAction(x).Value).ToList<RoverAction>();
                    MotorStart(roverMotor, routeInfo);
                    var result = motorTrack.Find(f => f.TrackerId == motorTrack.Max(m => m.TrackerId));
                    return string.Concat(result.CellPosition, " ", result.RowPosition, " ", result.RoverPosition.ToString());
                }
                else
                    return Constants.INVALIDINSTRUCTIONSFORPOSITION;
            }
            else
                return Constants.INVALIDFORMATTOPROCESS;
        }

        private static bool IsValidInstructions(string instruction)
        {
            return !string.IsNullOrEmpty(instruction) && instruction.ToUpper().Replace("L", "").Replace("R", "").Replace("M", "").Length == 0;
        }

        /// <summary>
        /// This is used to ride motor on based given insturctions
        /// </summary>
        /// <param name="startMotor"></param>
        /// <param name="routeInstructions"></param>
        private static void MotorStart(RoverMotor startMotor, List<RoverAction> routeInstructions)
        {
            if (startMotor == null) throw new ArgumentNullException("Invalid start position: startMotor");
            if (routeInstructions == null || routeInstructions.Count == 0) throw new ArgumentNullException("Invalid instructions: routeInstructions");

            RoverMotor prevMotor = startMotor;
            motorTrack.Add(startMotor);
            SurfaceTrack.Add(SetActiveArea(startMotor));

            for(int index=0; index<routeInstructions.Count; index++)
            {
                RoverMotor currentMotor;
                switch (routeInstructions[index])
                {
                    case RoverAction.Left: currentMotor = TurnLeft(prevMotor); motorTrack.Add(currentMotor);prevMotor = currentMotor; break;
                    case RoverAction.Right: currentMotor = TurnRight(prevMotor); motorTrack.Add(currentMotor); prevMotor = currentMotor; break;
                    case RoverAction.Move: currentMotor = MoveForward(prevMotor); motorTrack.Add(currentMotor); prevMotor = currentMotor; break;
                }
            }
        }
    }
}
