using System.Collections.Generic;

namespace ProjectVirtualTabletop.Entities {
	public class MapDetails {
        public int NumberOfRows { get; set; }
        public int NumberOfColumns { get; set; }
        public List<Token> Tokens { get; set; }
	}
}