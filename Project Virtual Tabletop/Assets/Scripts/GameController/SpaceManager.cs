using System;
using ProjectVirtualTabletop.Constants;
using ProjectVirtualTabletop.Entities;
using ProjectVirtualTabletop.Exceptions;
using ProjectVirtualTabletop.GameController.Interfaces;

namespace ProjectVirtualTabletop.GameController {
	public class SpaceManager : ISpaceManager {
		private const int rowDimension = 0;
		private const int columnDimension = 1;

		public Element[,] Map { get; set; }

		public void AddTo(Space space, Element element) {
			ThrowExceptionIfArgumentIsNull(space, "space", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfArgumentIsNull(element, "element", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfSpaceIsInvalid(space);
			ThrowExceptionIfSpaceIsOutOfBounds(space, "space", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);
			ThrowExceptionIfAnElementExistsOnSpace(space, ExceptionConstants.VA_ELEMENT_EXISTS_ON_SPACE);

			Map[space.Row, space.Column] = element;
		}

		public Element GetElementOn(Space space) {
			ThrowExceptionIfArgumentIsNull(space, "space", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfSpaceIsInvalid(space);
			ThrowExceptionIfSpaceIsOutOfBounds(space, "space", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);

			return Map[space.Row,space.Column];
		}

		public bool IsEmpty(Space space) {
			ThrowExceptionIfArgumentIsNull(space, "space", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfSpaceIsInvalid(space);
			ThrowExceptionIfSpaceIsOutOfBounds(space, "space", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);

			return Map[space.Row, space.Column] == null;
		}

		public void Move(Space from, Space to) {
			ThrowExceptionIfArgumentIsNull(from, "from", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfArgumentIsNull(to, "to", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfSpaceIsInvalid(from, "from", ExceptionConstants.VA_SPACE_INVALID);
			ThrowExceptionIfSpaceIsInvalid(to, "to", ExceptionConstants.VA_SPACE_INVALID);
			ThrowExceptionIfSpaceIsOutOfBounds(from, "from", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);
			ThrowExceptionIfSpaceIsOutOfBounds(to, "to", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);
			ThrowExceptionIfAnElementDoesNotExistOnSpace(from, ExceptionConstants.VA_ELEMENT_DOESNT_EXIST_ON_SPACE);
			ThrowExceptionIfAnElementExistsOnSpace(to, ExceptionConstants.VA_ELEMENT_EXISTS_ON_SPACE);

			Map[to.Row, to.Column] = Map[from.Row, from.Column];
			Map[from.Row, from.Column] = null;
		}

		public Element RemoveFrom(Space space) {
			ThrowExceptionIfArgumentIsNull(space, "space", ExceptionConstants.VA_ARGUMENT_NULL);
			ThrowExceptionIfSpaceIsInvalid(space);
			ThrowExceptionIfSpaceIsOutOfBounds(space, "space", ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS);
			ThrowExceptionIfAnElementDoesNotExistOnSpace(space, ExceptionConstants.VA_ELEMENT_DOESNT_EXIST_ON_SPACE);

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

		private void ThrowExceptionIfSpaceIsInvalid(Space space, string paramName, string message) {
			if(!space.IsValid())
				throw new InvalidSpaceException(string.Format(message, paramName));
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