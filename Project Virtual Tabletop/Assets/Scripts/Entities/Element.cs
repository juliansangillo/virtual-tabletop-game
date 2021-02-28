namespace NaughtyBikerGames.ProjectVirtualTabletop.Entities {
	public class Element {
		public GridSpace CurrentSpace { get; set; }
		
		public Element(GridSpace space) {
			this.CurrentSpace = space;
		}
	}
}