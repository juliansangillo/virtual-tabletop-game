using System.Collections.Generic;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals {
	public class GridInitializeSignal {
        public IList<Element> Elements { get; private set; }
        public IList<GridSpace> Spaces { get; private set; }

        public GridInitializeSignal(IList<Element> elements, IList<GridSpace> spaces) {
            this.Elements = elements;
            this.Spaces = spaces;
        }
	}
}