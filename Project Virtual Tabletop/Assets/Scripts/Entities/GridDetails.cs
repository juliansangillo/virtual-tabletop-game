using System.Collections.Generic;

namespace ProjectVirtualTabletop.Entities {
	public class GridDetails {
        public int NumberOfRows { get; set; }
        public int NumberOfColumns { get; set; }
        public List<Token> Tokens { get; set; }

        public bool IsNumberOfRowsValid() {
            return NumberOfRows > 0;
        }

        public bool IsNumberOfColumnsValid() {
            return NumberOfColumns > 0;
        }

        public bool IsTokensValid() {
            return Tokens != null;
        }
	}
}