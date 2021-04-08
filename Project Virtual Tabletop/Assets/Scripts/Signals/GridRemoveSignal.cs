using NaughtyBikerGames.ProjectVirtualTabletop.Entities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals {
	public class GridRemoveSignal {
        public Element Element { get; set; }
        public GridSpace Space { get; set; }

		public GridRemoveSignal(Element element, GridSpace space) {
			this.Element = element;
			this.Space = space;
		}
	}
}