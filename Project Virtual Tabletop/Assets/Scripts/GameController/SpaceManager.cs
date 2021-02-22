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
			Map[to.Row, to.Column] = Map[from.Row, from.Column];
			Map[from.Row, from.Column] = null;
		}

		public Element RemoveFrom(Space space) {
			throw new System.NotImplementedException();
		}
	}
}