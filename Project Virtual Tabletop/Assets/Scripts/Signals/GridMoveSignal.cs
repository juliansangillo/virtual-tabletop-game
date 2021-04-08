using NaughtyBikerGames.ProjectVirtualTabletop.Entities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals {
	public class GridMoveSignal {
        public Element Element { get; set; }
        public GridSpace From { get; set; }
        public GridSpace To { get; set; }

        public GridMoveSignal(Element element, GridSpace from, GridSpace to) {
            this.Element = element;
            this.From = from;
            this.To = to;
        }
	}
}