namespace ProjectVirtualTabletop.Entities {
	public class Element {
		public Space CurrentSpace { get; set; }
		
		public Element(Space space) {
			this.CurrentSpace = space;
		}
	}
}