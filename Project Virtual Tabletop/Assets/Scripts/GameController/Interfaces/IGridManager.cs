using ProjectVirtualTabletop.Entities;

namespace ProjectVirtualTabletop.GameController.Interfaces {
	public interface IGridManager {
		void AddTo(GridSpace space, Element element);
		Element GetElementOn(GridSpace space);
		bool IsEmpty(GridSpace space);
		void Move(GridSpace from, GridSpace to);
		Element RemoveFrom(GridSpace space);
	}
}