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
using NaughtyBikerGames.ProjectVirtualTabletop.Extensions;
using Zenject;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals;

namespace NaughtyBikerGames.ProjectVirtualTabletop.PathManagement {
	public class PathManager : IPathManager, IInitializable, IDisposable {
		private readonly IPathFinder pathFinder;
        private readonly SignalBus signalBus;

		public GridSize GridSize { get; private set; }
		public Size CellSize { get; private set; }
		public Velocity TraversalVelocity { get; private set; }

		public Grid Grid { get; private set; }

        internal Action<GridInitializeSignal> GridInitializeCallback { get; set; }
        internal Action<GridMoveSignal> GridMoveCallback { get; set; }
        internal Action<GridAddSignal> GridAddCallback { get; set; }
        internal Action<GridRemoveSignal> GridRemoveCallback { get; set; }

		public PathManager(GridDetails gridDetails, IPathFinder pathFinder, SignalBus signalBus) {
			ThrowExceptionIfArgumentIsNull(gridDetails, "gridDetails", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfArgumentIsNull(pathFinder, "pathFinder", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfGridDetailsIsInvalid(gridDetails);

			GridSize = new GridSize(gridDetails.NumberOfColumns, gridDetails.NumberOfRows);
			CellSize = new Size(Distance.FromMeters(1), Distance.FromMeters(1));
			TraversalVelocity = Velocity.FromKilometersPerHour(10);

			this.Grid = Grid.CreateGridWithLateralConnections(GridSize, CellSize, TraversalVelocity);
			this.pathFinder = pathFinder;
            this.signalBus = signalBus;

            this.GridInitializeCallback = s => DisconnectAll(s.Spaces);
            this.GridMoveCallback = s => {
                Reconnect(s.From);
                Disconnect(s.To);
            };
            this.GridAddCallback = s => Disconnect(s.Space);
            this.GridRemoveCallback = s => Reconnect(s.Space);
		}

        public void Initialize() {
			signalBus.Subscribe<GridInitializeSignal>(GridInitializeCallback);
            signalBus.Subscribe<GridMoveSignal>(GridMoveCallback);
            signalBus.Subscribe<GridAddSignal>(GridAddCallback);
            signalBus.Subscribe<GridRemoveSignal>(GridRemoveCallback);
		}

		public List<GridSpace> Find(GridSpace from, GridSpace to) {
			ThrowExceptionIfArgumentIsNull(from, "from", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfArgumentIsNull(to, "to", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfSpaceIsInvalid(from, "from", ExceptionConstants.VA_SPACE_INVALID);
			ThrowExceptionIfSpaceIsInvalid(to, "to", ExceptionConstants.VA_SPACE_INVALID);
			ThrowExceptionIfSpaceIsOutOfBounds(from, "from", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);
			ThrowExceptionIfSpaceIsOutOfBounds(to, "to", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);

			Path path = pathFinder.FindPath(from.AsGridPosition(), to.AsGridPosition(), Grid);
			Position firstPosition = path.Edges.First().Start.Position;

			List<GridSpace> result = new List<GridSpace>();
			result.Add(new GridSpace((int)firstPosition.Y, (int)firstPosition.X));

			foreach (IEdge edge in path.Edges) {
				Position position = edge.End.Position;
				result.Add(new GridSpace((int)position.Y, (int)position.X));
			}

			return result;
		}

		public void Disconnect(GridSpace space) {
			ThrowExceptionIfArgumentIsNull(space, "space", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfSpaceIsInvalid(space, "space", ExceptionConstants.VA_SPACE_INVALID);
			ThrowExceptionIfSpaceIsOutOfBounds(space, "space", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);

			Grid.DisconnectNode(space.AsGridPosition());
		}

        public void DisconnectAll(IList<GridSpace> spaces) {
            ThrowExceptionIfArgumentIsNull(spaces, "spaces", ExceptionConstants.VA_ARGUMENT_NULL);

			foreach (GridSpace space in spaces)
                Disconnect(space);
		}

        public void Reconnect(GridSpace space) {
            ThrowExceptionIfArgumentIsNull(space, "space", ExceptionConstants.VA_ARGUMENT_NULL);
            ThrowExceptionIfSpaceIsInvalid(space, "space", ExceptionConstants.VA_SPACE_INVALID);
            ThrowExceptionIfSpaceIsOutOfBounds(space, "space", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);

			Grid.ReconnectNode(space.AsGridPosition(), TraversalVelocity);
		}

        public void Dispose() {
            signalBus.Unsubscribe<GridRemoveSignal>(GridRemoveCallback);
            signalBus.Unsubscribe<GridAddSignal>(GridAddCallback);
            signalBus.Unsubscribe<GridMoveSignal>(GridMoveCallback);
            signalBus.Unsubscribe<GridInitializeSignal>(GridInitializeCallback);
		}

		private void ThrowExceptionIfSpaceIsInvalid(GridSpace space, string paramName, string message) {
			if (!space.IsValid())
				throw new InvalidSpaceException(string.Format(message, paramName));
		}

		private void ThrowExceptionIfSpaceIsOutOfBounds(GridSpace space, string paramName, string message) {
			if (IsRowOutOfBounds(space.Row) || IsColumnOutOfBounds(space.Column))
				throw new ArgumentException(message, paramName);
		}

		private bool IsRowOutOfBounds(int row) {
			return row >= Grid.Rows;
		}

		private bool IsColumnOutOfBounds(int column) {
			return column >= Grid.Columns;
		}

        private void ThrowExceptionIfArgumentIsNull(object arg, string paramName, string message) {
			if (arg == null)
				throw new ArgumentNullException(paramName, message);
		}

        private void ThrowExceptionIfGridDetailsIsInvalid(GridDetails gridDetails) {
			if (!gridDetails.IsNumberOfRowsValid())
				throw new ArgumentException(
					string.Format(ExceptionConstants.VA_NUMBER_OF_ROWS_INVALID, gridDetails.NumberOfRows),
					"gridDetails"
				);
			else if (!gridDetails.IsNumberOfColumnsValid())
				throw new ArgumentException(
					string.Format(ExceptionConstants.VA_NUMBER_OF_COLUMNS_INVALID, gridDetails.NumberOfColumns),
					"gridDetails"
				);
		}
	}
}