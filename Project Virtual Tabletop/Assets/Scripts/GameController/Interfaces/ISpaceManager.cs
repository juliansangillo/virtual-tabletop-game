using ProjectVirtualTabletop.Entities;

namespace ProjectVirtualTabletop.GameController.Interfaces {
	public interface ISpaceManager {
		void AddTo(Space space, Element element);
		Element GetElementOn(Space space);
		bool IsEmpty(Space space);
		void Move(Space from, Space to);
		Element RemoveFrom(Space space);
	}
}