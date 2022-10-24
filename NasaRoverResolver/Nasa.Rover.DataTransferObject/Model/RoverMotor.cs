using Nasa.Rover.DataTransferObject.Enums;

namespace Nasa.Rover.DataTransferObject.Model
{
    public class RoverMotor
    {
        public int TrackerId { get; set; }
        public int CellPosition { get; set; }
        public int RowPosition { get; set; }
        public RoverDirection RoverPosition { get; set; }

        public RoverMotor(int cell, int row, int prevTrackerId, RoverDirection roverDirection)
        {
            CellPosition = cell;
            RowPosition = row;
            TrackerId = prevTrackerId + 1;
            RoverPosition = roverDirection;
        }

    }
}
