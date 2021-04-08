using System.Collections.Generic;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals {
	public class GridInitializeSignal {
        public List<Element> Elements { get; private set; }
        public List<GridSpace> Spaces { get; private set; }

        public GridInitializeSignal(List<Element> elements, List<GridSpace> spaces) {
            this.Elements = elements;
            this.Spaces = spaces;
        }
	}
}