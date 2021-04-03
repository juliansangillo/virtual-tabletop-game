using System.Collections.Generic;
using System.Linq;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.PathManagement.Interfaces;
using Roy_T.AStar.Graphs;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;

namespace NaughtyBikerGames.ProjectVirtualTabletop.PathManagement {
	public class PathManager : IPathManager {
        private Grid grid;
        
		public PathManager(GridDetails gridDetails) {
            GridSize gridSize = new GridSize(gridDetails.NumberOfColumns, gridDetails.NumberOfRows);
            Size cellSize = new Size(Distance.FromMeters(1), Distance.FromMeters(1));
            Velocity traversalVelocity = Velocity.FromKilometersPerHour(10);

            grid = Grid.CreateGridWithLateralConnections(gridSize, cellSize, traversalVelocity);
		}

		public List<GridSpace> Find(GridSpace from, GridSpace to) {
			Path path = new PathFinder().FindPath(new GridPosition(from.Column, from.Row), new GridPosition(to.Column, to.Row), grid);
            Position firstPosition = path.Edges.First().Start.Position;

            List<GridSpace> result = new List<GridSpace>();
            result.Add(new GridSpace((int)firstPosition.Y, (int)firstPosition.X));

            foreach(IEdge edge in path.Edges) {
                Position position = edge.End.Position;
                result.Add(new GridSpace((int)position.Y, (int)position.X));
            }
            
            return result;
		}
	}
}