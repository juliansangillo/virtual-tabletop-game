using System;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using Roy_T.AStar.Primitives;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Extensions {
	public static class GridPositionExtensions {
        public static GridSpace AsGridSpace(this GridPosition gridPosition) {
            return new GridSpace(gridPosition.Y, gridPosition.X);
        }
	}
}