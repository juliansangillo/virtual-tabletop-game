using Roy_T.AStar.Grids;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Interfaces {
	public interface IPathFinder {
        Path FindPath(GridPosition start, GridPosition end, Grid grid);
	}
}