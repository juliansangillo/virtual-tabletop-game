using System;
using ProjectVirtualTabletop.Entities;
using ProjectVirtualTabletop.Exceptions;
using ProjectVirtualTabletop.GameController.Interfaces;

namespace ProjectVirtualTabletop.GameController {
	public class SpaceManager : ISpaceManager {
		private const int rowDimension = 0;
		private const int columnDimension = 1;

		public Element[,] Map { get; set; }

		public void AddTo(Space space, Element element) {
			ThrowExceptionIfArgumentIsNull(space, "space", "A required argument was null");
			ThrowExceptionIfArgumentIsNull(element, "element", "A required argument was null");
			ThrowExceptionIfSpaceIsInvalid(space);
			ThrowExceptionIfSpaceIsOutOfBounds(space, "space", "Space doesn't exist on map");

			Map[space.Row, space.Column] = element;
		}

		public Element GetElementOn(Space space) {
			ThrowExceptionIfArgumentIsNull(space, "space", "A required argument was null");
			ThrowExceptionIfSpaceIsInvalid(space);
			ThrowExceptionIfSpaceIsOutOfBounds(space, "space", "Space doesn't exist on map");

			return Map[space.Row,space.Column];
		}

		public bool IsEmpty(Space space) {
			ThrowExceptionIfArgumentIsNull(space, "space", "A required argument was null");
			ThrowExceptionIfSpaceIsInvalid(space);
			ThrowExceptionIfSpaceIsOutOfBounds(space, "space", "Space doesn't exist on map");

			return Map[space.Row, space.Column] == null;
		}

		public void Move(Space from, Space to) {
			ThrowExceptionIfArgumentIsNull(from, "from", "A required argument was null");
			ThrowExceptionIfArgumentIsNull(to, "to", "A required argument was null");
			ThrowExceptionIfSpaceIsInvalid(from, "The space to move from is invalid. Please verify neither the row or column " +
				"are negative and try again.");
			ThrowExceptionIfSpaceIsInvalid(to, "The space to move to is invalid. Please verify neither the row or column " +
				"are negative and try again.");
			ThrowExceptionIfSpaceIsOutOfBounds(from, "from", "The space to move from does not exist on map");
			ThrowExceptionIfSpaceIsOutOfBounds(to, "to", "The space to move to does not exist on map");
			ThrowExceptionIfAnElementDoesNotExistOnSpace(from, "The space to move from is empty. An element must exist on the space in order to move it");
			ThrowExceptionIfAnElementExistsOnSpace(to, "The space to move to is not empty. A space must be unoccupied in order to add an element to it. " +
					"Please remove the existing element from this space first before adding another element.");

			Map[to.Row, to.Column] = Map[from.Row, from.Column];
			Map[from.Row, from.Column] = null;
		}

		public Element RemoveFrom(Space space) {
			ThrowExceptionIfArgumentIsNull(space, "space", "A required argument was null");
			ThrowExceptionIfSpaceIsInvalid(space);
			ThrowExceptionIfSpaceIsOutOfBounds(space, "space", "Space doesn't exist on map");
			ThrowExceptionIfAnElementDoesNotExistOnSpace(space, "The space is empty. An element must exist on the space in order to remove it");

			Element element = Map[space.Row, space.Column];
			Map[space.Row, space.Column] = null;
			return element;
		}

		private bool IsRowOutOfBounds(int row) {
			return row >= Map.GetLength(rowDimension);
		}

		private bool IsColumnOutOfBounds(int column) {
			return column >= Map.GetLength(columnDimension);
		}

		private void ThrowExceptionIfArgumentIsNull(object arg, string paramName, string message) {
			if(arg == null)
				throw new ArgumentNullException(paramName, message);
		}

		private void ThrowExceptionIfSpaceIsInvalid(Space space) {
			if(!space.IsValid())
				throw new InvalidSpaceException();
		}

		private void ThrowExceptionIfSpaceIsInvalid(Space space, string message) {
			if(!space.IsValid())
				throw new InvalidSpaceException(message);
		}

		private void ThrowExceptionIfSpaceIsOutOfBounds(Space space, string paramName, string message) {
			if(IsRowOutOfBounds(space.Row) || IsColumnOutOfBounds(space.Column))
				throw new ArgumentException(message, paramName);
		}

		private void ThrowExceptionIfAnElementDoesNotExistOnSpace(Space space, string message) {
			if(Map[space.Row, space.Column] == null)
				throw new InvalidOperationException(message);
		}

		private void ThrowExceptionIfAnElementExistsOnSpace(Space space, string message) {
			if(Map[space.Row, space.Column] != null)
				throw new InvalidOperationException(message);
		}
	}
}