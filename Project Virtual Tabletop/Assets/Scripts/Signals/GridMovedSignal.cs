using NaughtyBikerGames.ProjectVirtualTabletop.Entities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals {
	public class GridMovedSignal {
        public Element Element { get; set; }
        public GridSpace From { get; set; }
        public GridSpace To { get; set; }

        public GridMovedSignal(Element element, GridSpace from, GridSpace to) {
            this.Element = element;
            this.From = from;
            this.To = to;
        }
	}
}