using Roy_T.AStar.Primitives;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Entities {
	public class GridSpace {
		public int Row { get; set; }
		public int Column { get; set; }

		public GridSpace(int row, int column) {
			this.Row = row;
			this.Column = column;
		}

        public GridPosition AsGridPosition() {
            return new GridPosition(Column, Row);
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

		public override bool Equals(object obj) {
			return obj is GridSpace space &&
				   Row == space.Row &&
				   Column == space.Column;
		}

		public override int GetHashCode() {
			int hashCode = 240067226;
			hashCode = hashCode * -1521134295 + Row.GetHashCode();
			hashCode = hashCode * -1521134295 + Column.GetHashCode();
			return hashCode;
		}

		public override string ToString() {
			return base.ToString() + $" ({Row}, {Column})";
		}
	}
}