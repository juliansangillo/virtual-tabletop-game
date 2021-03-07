using NaughtyBikerGames.ProjectVirtualTabletop.Entities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.GridManagement.Interfaces {
	public interface IGridManager {
		void AddTo(GridSpace space, Element element);
		Element GetElementOn(GridSpace space);
		bool IsEmpty(GridSpace space);
		void Move(GridSpace from, GridSpace to);
		Element RemoveFrom(GridSpace space);
	}
}