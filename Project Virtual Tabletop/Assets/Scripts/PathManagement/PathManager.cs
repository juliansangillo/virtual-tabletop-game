using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;
using Roy_T.AStar.Graphs;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;
using NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.Exceptions;
using NaughtyBikerGames.ProjectVirtualTabletop.Extensions;
using NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals;

namespace NaughtyBikerGames.ProjectVirtualTabletop.PathManagement {
	public class PathManager : IPathManager, IDisposable {
		private readonly IPathFinder pathFinder;
        private readonly SignalBus signalBus;

		public GridSize GridSize { get; private set; }
		public Size CellSize { get; private set; }
		public Velocity TraversalVelocity { get; private set; }

		public Grid Grid { get; private set; }

        [Inject]
		public PathManager(GridDetails gridDetails, IPathFinder pathFinder, SignalBus signalBus) {
			ThrowExceptionIfArgumentIsNull(gridDetails, "gridDetails", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfArgumentIsNull(pathFinder, "pathFinder", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfGridDetailsIsInvalid(gridDetails);

			GridSize = new GridSize(gridDetails.NumberOfColumns, gridDetails.NumberOfRows);
			CellSize = new Size(Distance.FromMeters(AppConstants.SPACE_WIDTH_IN_METERS), Distance.FromMeters(AppConstants.SPACE_HEIGHT_IN_METERS));
			TraversalVelocity = Velocity.FromKilometersPerHour(1);
            
            this.Grid = Grid.CreateGridWithLateralAndDiagonalConnections(GridSize, CellSize, TraversalVelocity);
			this.pathFinder = pathFinder;
            this.signalBus = signalBus;

            signalBus.Subscribe<GridInitializedSignal>(OnGridInitialize);
            signalBus.Subscribe<GridMovedSignal>(OnGridMove);
            signalBus.Subscribe<GridAddedSignal>(OnGridAdd);
            signalBus.Subscribe<GridRemovedSignal>(OnGridRemove);
		}

		public GridPath Find(GridSpace from, GridSpace to) {
			ThrowExceptionIfArgumentIsNull(from, "from", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfArgumentIsNull(to, "to", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfSpaceIsInvalid(from, "from", ExceptionConstants.VA_SPACE_INVALID);
			ThrowExceptionIfSpaceIsInvalid(to, "to", ExceptionConstants.VA_SPACE_INVALID);
			ThrowExceptionIfSpaceIsOutOfBounds(from, "from", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);
			ThrowExceptionIfSpaceIsOutOfBounds(to, "to", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);

			Path path = pathFinder.FindPath(from.AsGridPosition(), to.AsGridPosition(), Grid);
			GridPosition firstPosition = GetGridPositionFromRoundedPosition(path.Edges.First().Start.Position);

			ICollection<GridSpace> spaces = new List<GridSpace>();
			spaces.Add(firstPosition.AsGridSpace());

			foreach (IEdge edge in path.Edges) {
				GridPosition gridPosition = GetGridPositionFromRoundedPosition(edge.End.Position);
				spaces.Add(gridPosition.AsGridSpace());
			}

			return new GridPath(spaces.Count - 1, path.Distance.Meters, spaces);
		}

		public virtual void Disconnect(GridSpace space) {
			ThrowExceptionIfArgumentIsNull(space, "space", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfSpaceIsInvalid(space, "space", ExceptionConstants.VA_SPACE_INVALID);
			ThrowExceptionIfSpaceIsOutOfBounds(space, "space", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);

			Grid.DisconnectNode(space.AsGridPosition());
		}

        public virtual void DisconnectAll(IList<GridSpace> spaces) {
            ThrowExceptionIfArgumentIsNull(spaces, "spaces", ExceptionConstants.VA_ARGUMENT_NULL);

			foreach (GridSpace space in spaces)
                Disconnect(space);
		}

        public virtual void Reconnect(GridSpace space) {
            ThrowExceptionIfArgumentIsNull(space, "space", ExceptionConstants.VA_ARGUMENT_NULL);
            ThrowExceptionIfSpaceIsInvalid(space, "space", ExceptionConstants.VA_SPACE_INVALID);
            ThrowExceptionIfSpaceIsOutOfBounds(space, "space", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);

			Grid.ReconnectNode(space.AsGridPosition(), TraversalVelocity);
		}

        public void Dispose() {
            signalBus.Unsubscribe<GridRemovedSignal>(OnGridRemove);
            signalBus.Unsubscribe<GridAddedSignal>(OnGridAdd);
            signalBus.Unsubscribe<GridMovedSignal>(OnGridMove);
            signalBus.Unsubscribe<GridInitializedSignal>(OnGridInitialize);
		}

        private void OnGridInitialize(GridInitializedSignal signal) => DisconnectAll(signal.Spaces);
        private void OnGridMove(GridMovedSignal signal) { Reconnect(signal.From); Disconnect(signal.To); }
        private void OnGridAdd(GridAddedSignal signal) => Disconnect(signal.Space);
        private void OnGridRemove(GridRemovedSignal signal) => Reconnect(signal.Space);

        private GridPosition GetGridPositionFromRoundedPosition(Position position) {
			return new GridPosition((int)Math.Round(position.X / AppConstants.SPACE_WIDTH_IN_METERS),
				(int)Math.Round(position.Y / AppConstants.SPACE_HEIGHT_IN_METERS));
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