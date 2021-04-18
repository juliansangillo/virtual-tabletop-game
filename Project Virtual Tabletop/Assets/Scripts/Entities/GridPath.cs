using System.Collections.Generic;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Entities {
	public class GridPath {
        public int Length { get; private set; }
        public float DistanceInMeters { get; private set; }
        public IEnumerable<GridSpace> Spaces { get; private set; }

		public GridPath(int length, float distanceInMeters, IEnumerable<GridSpace> spaces) {
			this.Length = length;
			this.DistanceInMeters = distanceInMeters;
			this.Spaces = spaces;
		}
	}
}