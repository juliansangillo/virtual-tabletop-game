using NaughtyBikerGames.ProjectVirtualTabletop.Entities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals {
	public class TokenDraggedSignal {
        public GridSpace Source { get; private set; }
        public GridSpace Destination { get; private set; }

		public TokenDraggedSignal(GridSpace source, GridSpace destination) {
			this.Source = source;
			this.Destination = destination;
		}
	}
}