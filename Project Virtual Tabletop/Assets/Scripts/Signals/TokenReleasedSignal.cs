using NaughtyBikerGames.ProjectVirtualTabletop.Entities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals {
	public class TokenReleasedSignal {
        public GridSpace Source { get; private set; }
        public GridSpace Destination { get; private set; }

		public TokenReleasedSignal(GridSpace source, GridSpace destination) {
			this.Source = source;
			this.Destination = destination;
		}
	}
}