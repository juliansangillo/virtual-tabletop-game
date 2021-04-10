using Roy_T.AStar.Primitives;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Extensions {
	public static class GridPositionExtensions {
        public static GridSpace AsGridSpace(this GridPosition gridPosition) {
            return new GridSpace(gridPosition.Y, gridPosition.X);
        }
	}
}