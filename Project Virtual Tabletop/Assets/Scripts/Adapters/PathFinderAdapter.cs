using Roy_T.AStar.Grids;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;
using NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Interfaces;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Adapters {
	public class PathFinderAdapter : IPathFinder {
        private PathFinder pathFinder;

        public PathFinderAdapter() {
            pathFinder = new PathFinder();
        }

		public Path FindPath(GridPosition start, GridPosition end, Grid grid) {
			return pathFinder.FindPath(start, end, grid);
		}
	}
}