using System;
using ProjectVirtualTabletop.Entities;
using ProjectVirtualTabletop.Exceptions;
using ProjectVirtualTabletop.GameController.Interfaces;

namespace ProjectVirtualTabletop.GameController {
	public class SpaceManager : ISpaceManager {
		public Element[,] Map { get; set; }

		public void AddTo(Space space, Element element) {
			if(space == null || element == null)
				throw new ArgumentNullException("space,element", string.Format("A required argument was null. space = {0} , element = {1}", space, element));
			else if(!space.IsValid())
				throw new InvalidSpaceException();
			else if(space.Row >= Map.GetLength(0) || space.Column >= Map.GetLength(1))
				throw new ArgumentException("Space doesn't exist on map", "space");

			Map[space.Row, space.Column] = element;
		}

		public Element GetElementOn(Space space) {
			if(space == null)
				throw new ArgumentNullException("space", "Space cannot be null.");
			else if(!space.IsValid())
				throw new InvalidSpaceException();
			else if(space.Row >= Map.GetLength(0) || space.Column >= Map.GetLength(1))
				throw new ArgumentException("Space doesn't exist on map", "space");

			return Map[space.Row,space.Column];
		}

		public bool IsEmpty(Space space) {
			if(space == null)
				throw new ArgumentNullException("space", "Space cannot be null.");
			else if(!space.IsValid())
				throw new InvalidSpaceException();
			else if(space.Row >= Map.GetLength(0) || space.Column >= Map.GetLength(1))
				throw new ArgumentException("Space doesn't exist on map", "space");

			return Map[space.Row, space.Column] == null;
		}

		public void Move(Space from, Space to) {
			if(from == null)
				throw new ArgumentNullException("from", "The space to move from cannot be null.");
			else if(to == null)
				throw new ArgumentNullException("to", "The space to move to cannot be null.");
			else if(!from.IsValid())
				throw new InvalidSpaceException("The space to move from is invalid. Please verify neither the row or column " +
					"are negative and try again.");
			else if(!to.IsValid())
				throw new InvalidSpaceException("The space to move to is invalid. Please verify neither the row or column " +
				"are negative and try again.");
			else if(from.Row >= Map.GetLength(0) || from.Column >= Map.GetLength(1))
				throw new ArgumentException("The space to move from does not exist on map", "from");
			else if(to.Row >= Map.GetLength(0) || to.Column >= Map.GetLength(1))
				throw new ArgumentException("The space to move to does not exist on map", "to");
			else if(Map[from.Row, from.Column] == null)
				throw new InvalidOperationException("The space to move from is empty. An element must exist on the space to move it");
			else if(Map[to.Row, to.Column] != null)
				throw new InvalidOperationException("The space to move to is not empty. A space must be unoccupied to add an element to it. " +
					"Please remove the existing element from this space first before adding another element.");

			Map[to.Row, to.Column] = Map[from.Row, from.Column];
			Map[from.Row, from.Column] = null;
		}

		public Element RemoveFrom(Space space) {
			if(space == null)
				throw new ArgumentNullException("space", "Space cannot be null.");
			else if(!space.IsValid())
				throw new InvalidSpaceException();
			else if(space.Row >= Map.GetLength(0) || space.Column >= Map.GetLength(1))
				throw new ArgumentException("Space doesn't exist on map", "space");
			else if(Map[space.Row, space.Column] == null)
				throw new InvalidOperationException("The space is empty. An element must exist on the space to remove from it");

			Element element = Map[space.Row, space.Column];
			Map[space.Row, space.Column] = null;
			return element;
		}
	}
}