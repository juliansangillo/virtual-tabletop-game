using System;
using ProjectVirtualTabletop.Entities;
using ProjectVirtualTabletop.Exceptions;
using ProjectVirtualTabletop.GameController.Interfaces;

namespace ProjectVirtualTabletop.GameController {
	public class SpaceManager : ISpaceManager {
		public Element[,] Map { get; set; }

		public void AddTo(Space space, Element element) {
			if(!space.IsValid())
				throw new InvalidSpaceException();
			else if(space.Row >= Map.GetLength(0) || space.Column >= Map.GetLength(1))
				throw new ArgumentException("Space doesn't exist on map", "space");

			Map[space.Row, space.Column] = element;
		}

		public Element GetElementOn(Space space) {
			if(!space.IsValid())
				throw new InvalidSpaceException();
			else if(space.Row >= Map.GetLength(0) || space.Column >= Map.GetLength(1))
				throw new ArgumentException("Space doesn't exist on map", "space");

			return Map[space.Row,space.Column];
		}

		public bool IsEmpty(Space space) {
			if(!space.IsValid())
				throw new InvalidSpaceException();
			else if(space.Row >= Map.GetLength(0) || space.Column >= Map.GetLength(1))
				throw new ArgumentException("Space doesn't exist on map", "space");

			return Map[space.Row, space.Column] == null;
		}

		public void Move(Space from, Space to) {
			throw new System.NotImplementedException();
		}

		public Element RemoveFrom(Space space) {
			throw new System.NotImplementedException();
		}
	}
}