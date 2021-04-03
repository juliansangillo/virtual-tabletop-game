using System;
using System.Collections.Generic;
using System.Linq;
using Roy_T.AStar.Graphs;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;
using NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.Exceptions;
using NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Interfaces;

namespace NaughtyBikerGames.ProjectVirtualTabletop.PathManagement {
	public class PathManager : IPathManager {
        private readonly Grid grid;
        private readonly IPathFinder pathFinder;

        public GridSize GridSize { get; private set; }
        public Size CellSize { get; private set; }
        public Velocity TraversalVelocity { get; private set; }
        
		public PathManager(GridDetails gridDetails, IPathFinder pathFinder) {
            ThrowExceptionIfArgumentIsNull(gridDetails, "gridDetails", ExceptionConstants.VA_ARGUMENT_NULL);
            ThrowExceptionIfArgumentIsNull(pathFinder, "pathFinder", ExceptionConstants.VA_ARGUMENT_NULL);
            ThrowExceptionIfGridDetailsIsInvalid(gridDetails);

            GridSize = new GridSize(gridDetails.NumberOfColumns, gridDetails.NumberOfRows);
            CellSize = new Size(Distance.FromMeters(1), Distance.FromMeters(1));
            TraversalVelocity = Velocity.FromKilometersPerHour(10);

            this.grid = Grid.CreateGridWithLateralConnections(GridSize, CellSize, TraversalVelocity);
            this.pathFinder = pathFinder;
		}

		public List<GridSpace> Find(GridSpace from, GridSpace to) {
            ThrowExceptionIfArgumentIsNull(from, "from", ExceptionConstants.VA_ARGUMENT_NULL);
            ThrowExceptionIfArgumentIsNull(to, "to", ExceptionConstants.VA_ARGUMENT_NULL);
            ThrowExceptionIfSpaceIsInvalid(from, "from", ExceptionConstants.VA_SPACE_INVALID);
            ThrowExceptionIfSpaceIsInvalid(to, "to", ExceptionConstants.VA_SPACE_INVALID);
            ThrowExceptionIfSpaceIsOutOfBounds(from, "from", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);
            ThrowExceptionIfSpaceIsOutOfBounds(to, "to", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);

			Path path = pathFinder.FindPath(new GridPosition(from.Column, from.Row), new GridPosition(to.Column, to.Row), grid);
            Position firstPosition = path.Edges.First().Start.Position;

            List<GridSpace> result = new List<GridSpace>();
            result.Add(new GridSpace((int)firstPosition.Y, (int)firstPosition.X));

            foreach(IEdge edge in path.Edges) {
                Position position = edge.End.Position;
                result.Add(new GridSpace((int)position.Y, (int)position.X));
            }
            
            return result;
		}

        private void ThrowExceptionIfArgumentIsNull(object arg, string paramName, string message) {
			if(arg == null)
				throw new ArgumentNullException(paramName, message);
		}

        private void ThrowExceptionIfGridDetailsIsInvalid(GridDetails gridDetails) {
            if(!gridDetails.IsNumberOfRowsValid())
                throw new ArgumentException(
                    string.Format(ExceptionConstants.VA_NUMBER_OF_ROWS_INVALID, gridDetails.NumberOfRows),
                    "gridDetails"
                );
            else if(!gridDetails.IsNumberOfColumnsValid())
                throw new ArgumentException(
                    string.Format(ExceptionConstants.VA_NUMBER_OF_COLUMNS_INVALID, gridDetails.NumberOfColumns), 
                    "gridDetails"
                );
        }

        private void ThrowExceptionIfSpaceIsInvalid(GridSpace space, string paramName, string message) {
			if(!space.IsValid())
				throw new InvalidSpaceException(string.Format(message, paramName));
		}

        private void ThrowExceptionIfSpaceIsOutOfBounds(GridSpace space, string paramName, string message) {
			if(IsRowOutOfBounds(space.Row) || IsColumnOutOfBounds(space.Column))
				throw new ArgumentException(message, paramName);
		}

        private bool IsRowOutOfBounds(int row) {
			return row >= grid.Rows;
		}

        private bool IsColumnOutOfBounds(int column) {
			return column >= grid.Columns;
		}
	}
}