using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.Exceptions;
using NaughtyBikerGames.ProjectVirtualTabletop.Extensions;
using NaughtyBikerGames.ProjectVirtualTabletop.GridManagement.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals;
using Zenject;

namespace NaughtyBikerGames.ProjectVirtualTabletop.GridManagement {
	public class GridManager : IGridManager, IInitializable {
		private readonly SignalBus signalBus;

        public Element[,] Grid { get; }
		
		public GridManager(Element[,] grid, SignalBus signalBus) {
			this.Grid = grid;
            this.signalBus = signalBus;
		}

        public void Initialize() {
            IList<Element> elements = Grid.AsFlat().Where(element => element != null).ToList();
            IList<GridSpace> spaces = elements.Select(element => element.CurrentSpace).ToList();

            signalBus.Fire(new GridInitializeSignal(elements, spaces));
		}

		public void AddTo(GridSpace space, Element element) {
			ThrowExceptionIfArgumentIsNull(space, "space", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfArgumentIsNull(element, "element", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfSpaceIsInvalid(space);
			ThrowExceptionIfSpaceIsOutOfBounds(space, "space", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);
			ThrowExceptionIfAnElementExistsOnSpace(space, ExceptionConstants.VA_ELEMENT_EXISTS_ON_SPACE);

			Grid[space.Row, space.Column] = element;

            signalBus.Fire(new GridAddSignal(element, space));
		}

		public Element GetElementOn(GridSpace space) {
			ThrowExceptionIfArgumentIsNull(space, "space", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfSpaceIsInvalid(space);
			ThrowExceptionIfSpaceIsOutOfBounds(space, "space", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);

			return Grid[space.Row,space.Column];
		}

		public bool IsEmpty(GridSpace space) {
			ThrowExceptionIfArgumentIsNull(space, "space", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfSpaceIsInvalid(space);
			ThrowExceptionIfSpaceIsOutOfBounds(space, "space", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);

			return Grid[space.Row, space.Column] == null;
		}

		public void Move(GridSpace from, GridSpace to) {
			ThrowExceptionIfArgumentIsNull(from, "from", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfArgumentIsNull(to, "to", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfSpaceIsInvalid(from, "from", ExceptionConstants.VA_SPACE_INVALID);
			ThrowExceptionIfSpaceIsInvalid(to, "to", ExceptionConstants.VA_SPACE_INVALID);
			ThrowExceptionIfSpaceIsOutOfBounds(from, "from", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);
			ThrowExceptionIfSpaceIsOutOfBounds(to, "to", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);
			ThrowExceptionIfAnElementDoesNotExistOnSpace(from, ExceptionConstants.VA_ELEMENT_DOESNT_EXIST_ON_SPACE);
			ThrowExceptionIfAnElementExistsOnSpace(to, ExceptionConstants.VA_ELEMENT_EXISTS_ON_SPACE);

			Grid[to.Row, to.Column] = Grid[from.Row, from.Column];
			Grid[from.Row, from.Column] = null;

            signalBus.Fire(new GridMoveSignal(Grid[to.Row, to.Column], from, to));
		}

		public Element RemoveFrom(GridSpace space) {
			ThrowExceptionIfArgumentIsNull(space, "space", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfSpaceIsInvalid(space);
			ThrowExceptionIfSpaceIsOutOfBounds(space, "space", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);
			ThrowExceptionIfAnElementDoesNotExistOnSpace(space, ExceptionConstants.VA_ELEMENT_DOESNT_EXIST_ON_SPACE);

			Element element = Grid[space.Row, space.Column];
			Grid[space.Row, space.Column] = null;

            signalBus.Fire(new GridRemoveSignal(element, space));

			return element;
		}

		private bool IsRowOutOfBounds(int row) {
			return row >= Grid.GetLength(AppConstants.ROW_DIMENSION);
		}

		private bool IsColumnOutOfBounds(int column) {
			return column >= Grid.GetLength(AppConstants.COLUMN_DIMENSION);
		}

		private void ThrowExceptionIfArgumentIsNull(object arg, string paramName, string message) {
			if(arg == null)
				throw new ArgumentNullException(paramName, message);
		}

		private void ThrowExceptionIfSpaceIsInvalid(GridSpace space) {
			if(!space.IsValid())
				throw new InvalidSpaceException();
		}

		private void ThrowExceptionIfSpaceIsInvalid(GridSpace space, string paramName, string message) {
			if(!space.IsValid())
				throw new InvalidSpaceException(string.Format(message, paramName));
		}

		private void ThrowExceptionIfSpaceIsOutOfBounds(GridSpace space, string paramName, string message) {
			if(IsRowOutOfBounds(space.Row) || IsColumnOutOfBounds(space.Column))
				throw new ArgumentException(message, paramName);
		}

		private void ThrowExceptionIfAnElementDoesNotExistOnSpace(GridSpace space, string message) {
			if(Grid[space.Row, space.Column] == null)
				throw new InvalidOperationException(message);
		}

		private void ThrowExceptionIfAnElementExistsOnSpace(GridSpace space, string message) {
			if(Grid[space.Row, space.Column] != null)
				throw new InvalidOperationException(message);
		}
	}
}