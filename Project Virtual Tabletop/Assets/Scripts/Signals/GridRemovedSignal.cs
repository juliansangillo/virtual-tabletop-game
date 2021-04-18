using NaughtyBikerGames.ProjectVirtualTabletop.Entities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals {
	public class GridRemovedSignal {
        public Element Element { get; set; }
        public GridSpace Space { get; set; }

		public GridRemovedSignal(Element element, GridSpace space) {
			this.Element = element;
			this.Space = space;
		}
	}
}