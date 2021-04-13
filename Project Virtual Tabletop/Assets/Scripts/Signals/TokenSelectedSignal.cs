using NaughtyBikerGames.ProjectVirtualTabletop.Entities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Signals {
	public class TokenSelectedSignal {
        public GridSpace CurrentSpace { get; private set; }

        public TokenSelectedSignal(GridSpace currentSpace) {
            this.CurrentSpace = currentSpace;
        }
	}
}