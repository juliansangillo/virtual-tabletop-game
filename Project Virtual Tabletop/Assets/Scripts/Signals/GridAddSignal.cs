using NaughtyBikerGames.ProjectVirtualTabletop.Entities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals {
	public class GridAddSignal {
        public Element Element { get; set; }
        public GridSpace Space { get; set; }

		public GridAddSignal(Element element, GridSpace space) {
			this.Element = element;
			this.Space = space;
		}
	}
}