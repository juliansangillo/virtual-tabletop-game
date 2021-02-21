using System;

namespace ProjectVirtualTabletop.Entities {
	public class Space {
		public int Row { get; set; }
		public int Column { get; set; }

		public Space(int row, int column) {
			this.Row = row;
			this.Column = column;
		}

		public bool IsValid() {
			return IsRowNonNegative() && IsColumnNonNegative();
		}

		private bool IsRowNonNegative() {
			return Row >= 0;
		}

		private bool IsColumnNonNegative() {
			return Column >= 0;
		}
	}
}