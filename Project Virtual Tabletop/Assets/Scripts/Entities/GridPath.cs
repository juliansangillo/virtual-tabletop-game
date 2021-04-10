using System.Collections.Generic;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Entities {
	public class GridPath {
        public int Count { get; private set; }
        public float DistanceInMeters { get; private set; }
        public IEnumerable<GridSpace> Spaces { get; private set; }

		public GridPath(int count, float distanceInMeters, IEnumerable<GridSpace> spaces) {
			this.Count = count;
			this.DistanceInMeters = distanceInMeters;
			this.Spaces = spaces;
		}
	}
}