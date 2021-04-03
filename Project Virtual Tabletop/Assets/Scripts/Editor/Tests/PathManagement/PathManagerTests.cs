using System.Collections.Generic;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.PathManagement;
using NSubstitute;
using NUnit.Framework;
using Roy_T.AStar.Graphs;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.Tests.PathManagement {
	public class PathManagerTests {
        private PathManager pathManager;

        //TODO: Create a PathFinderAdapter to stub out for this
        private PathFinder pathFinder;

        [SetUp]
        public void SetUp() {
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 3;
            gridDetails.NumberOfColumns = 3;
            
            pathManager = new PathManager(gridDetails);
        }

		[Test]
		public void Find_GivenTwoValidGridSpaces_ReturnListOfGridSpacesThatFallAlongTheShortestPathBetweenGivenSpacesInclusively() {
			GridSpace src = new GridSpace(0, 0);
            GridSpace dest = new GridSpace(2, 2);

            List<GridSpace> actual = pathManager.Find(src, dest);

            Assert.Contains(new GridSpace(0, 0), actual);
            Assert.Contains(new GridSpace(0, 1), actual);
            Assert.Contains(new GridSpace(1, 1), actual);
            Assert.Contains(new GridSpace(2, 1), actual);
            Assert.Contains(new GridSpace(2, 2), actual);
		}

        private Path GetStubbedPath() {
            List<IEdge> edges = new List<IEdge>();
            edges.Add(new Edge(new Node(Position.Zero), new Node(new Position(1, 0)), Velocity.FromKilometersPerHour(0)));
            edges.Add(new Edge(new Node(new Position(1, 0)), new Node(new Position(1, 1)), Velocity.FromKilometersPerHour(0)));
            edges.Add(new Edge(new Node(new Position(1, 1)), new Node(new Position(2, 1)), Velocity.FromKilometersPerHour(0)));
            edges.Add(new Edge(new Node(new Position(2, 1)), new Node(new Position(2, 2)), Velocity.FromKilometersPerHour(0)));
            
            return new Path(PathType.Complete, edges);
        }
	}
}
