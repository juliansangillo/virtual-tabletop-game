using NaughtyBikerGames.ProjectVirtualTabletop.Entities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals {
	public class GridAddedSignal {
        public Element Element { get; set; }
        public GridSpace Space { get; set; }

		public GridAddedSignal(Element element, GridSpace space) {
			this.Element = element;
			this.Space = space;
		}
	}
}