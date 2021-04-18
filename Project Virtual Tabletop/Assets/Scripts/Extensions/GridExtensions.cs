using System.Collections.Generic;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Primitives;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Extensions {
	public static class GridExtensions {
		public static void ReconnectNode(this Grid grid, GridPosition position, Velocity traversalVelocity) {
            if (grid.IsDisconnectedNode(position))
				ConnectSurroundingNodes(grid, position, traversalVelocity);
		}

		public static bool IsInsideGrid(this Grid grid, GridPosition position) => position.X >= 0 && position.X < grid.Columns && 
            position.Y >= 0 && position.Y < grid.Rows;

        public static bool IsDisconnectedNode(this Grid grid, GridPosition position) => grid.GetNode(position).Incoming.Count == 0 && 
            grid.GetNode(position).Outgoing.Count == 0;

        private static void ConnectSurroundingNodes(Grid grid, GridPosition position, Velocity traversalVelocity) {
			IList<GridPosition> others = new List<GridPosition>();
			others.Add(new GridPosition(position.X, position.Y - 1));
			others.Add(new GridPosition(position.X - 1, position.Y));
			others.Add(new GridPosition(position.X, position.Y + 1));
			others.Add(new GridPosition(position.X + 1, position.Y));

			foreach (GridPosition other in others)
				if (grid.IsInsideGrid(other) && !grid.IsDisconnectedNode(other)) {
					grid.AddEdge(other, position, traversalVelocity);
					grid.AddEdge(position, other, traversalVelocity);
				}
		}
	}
}