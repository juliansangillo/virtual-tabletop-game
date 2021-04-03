using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Roy_T.AStar.Graphs;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;
using NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.Exceptions;
using NaughtyBikerGames.ProjectVirtualTabletop.PathManagement;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.Tests.PathManagement {
	public class PathManagerTests {
        private PathManager pathManager;

        private IPathFinder pathFinder;

        [SetUp]
        public void SetUp() {
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 3;
            gridDetails.NumberOfColumns = 3;

            pathFinder = Substitute.For<IPathFinder>();
            
            pathManager = new PathManager(gridDetails, pathFinder);
        }

        [Test]
        public void PathManager_GivenValidGridDetails_ReturnPathManagerWithCorrectGridSize() {
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 4;
            gridDetails.NumberOfColumns = 5;

            IPathFinder pathFinder = Substitute.For<IPathFinder>();

            PathManager pathManager = new PathManager(gridDetails, pathFinder);

            Assert.AreEqual(4, pathManager.GridSize.Rows);
            Assert.AreEqual(5, pathManager.GridSize.Columns);
        }

        [Test]
        public void PathManager_GivenValidGridDetails_ReturnPathManagerWithCorrectCellSize() {
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 4;
            gridDetails.NumberOfColumns = 5;

            IPathFinder pathFinder = Substitute.For<IPathFinder>();

            PathManager pathManager = new PathManager(gridDetails, pathFinder);

            Assert.AreEqual(Distance.FromMeters(1), pathManager.CellSize.Width);
            Assert.AreEqual(Distance.FromMeters(1), pathManager.CellSize.Height);
        }

        [Test]
        public void PathManager_GivenValidGridDetails_ReturnPathManagerWithCorrectTraversalVelocity() {
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 4;
            gridDetails.NumberOfColumns = 5;

            IPathFinder pathFinder = Substitute.For<IPathFinder>();

            PathManager pathManager = new PathManager(gridDetails, pathFinder);

            Assert.AreEqual(10f, pathManager.TraversalVelocity.KilometersPerHour);
        }

        [Test]
        public void PathManager_GivenNullGridDetails_ThrowArgumentNullException() {
            IPathFinder pathFinder = Substitute.For<IPathFinder>();
            Exception expected = new ArgumentNullException("gridDetails", ExceptionConstants.VA_ARGUMENT_NULL);

            Exception actual = Assert.Throws<ArgumentNullException>(() => {
                new PathManager(null, pathFinder);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void PathManager_GivenNullPathFinder_ThrowArgumentNullException() {
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 4;
            gridDetails.NumberOfColumns = 5;
            Exception expected = new ArgumentNullException("pathFinder", ExceptionConstants.VA_ARGUMENT_NULL);

            Exception actual = Assert.Throws<ArgumentNullException>(() => {
                new PathManager(gridDetails, null);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void PathManager_GivenInvalidGridDetailsWhereNumberOfRowsAreZeroOrNegative_ThrowArgumentException(int numRows) {
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = numRows;
            gridDetails.NumberOfColumns = 5;
            IPathFinder pathFinder = Substitute.For<IPathFinder>();
            Exception expected = new ArgumentException(
                string.Format(ExceptionConstants.VA_NUMBER_OF_ROWS_INVALID, numRows), 
                "gridDetails"
            );

            Exception actual = Assert.Throws<ArgumentException>(() => {
                new PathManager(gridDetails, pathFinder);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }
        
        [TestCase(0)]
        [TestCase(-1)]
        public void PathManager_GivenInvalidGridDetailsWhereNumberOfColumnsAreZeroOrNegative_ThrowArgumentException(int numColumns) {
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 4;
            gridDetails.NumberOfColumns = numColumns;
            IPathFinder pathFinder = Substitute.For<IPathFinder>();
            Exception expected = new ArgumentException(
                string.Format(ExceptionConstants.VA_NUMBER_OF_COLUMNS_INVALID, numColumns), 
                "gridDetails"
            );

            Exception actual = Assert.Throws<ArgumentException>(() => {
                new PathManager(gridDetails, pathFinder);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        //* The logical values used between Roy_T.AStar's Position and our GridSpace is flipped. This is because
        //* Position uses arguments (x, y) and GridSpace uses arguments (row, column) where column == x and row == y.
        //* i.e. Position(0, 1) == GridSpace(1, 0)
		[Test]
		public void Find_GivenTwoValidGridSpaces_ReturnListOfGridSpacesThatFallAlongTheShortestPathBetweenGivenSpacesInclusively() {
			GridSpace src = new GridSpace(0, 0);
            GridSpace dest = new GridSpace(2, 2);

            pathFinder.FindPath(GridPosition.Zero, new GridPosition(2, 2), Arg.Any<Grid>()).Returns(GetStubbedPath());

            List<GridSpace> actual = pathManager.Find(src, dest);

            Assert.Contains(new GridSpace(0, 0), actual);
            Assert.Contains(new GridSpace(1, 0), actual);
            Assert.Contains(new GridSpace(1, 1), actual);
            Assert.Contains(new GridSpace(1, 2), actual);
            Assert.Contains(new GridSpace(2, 2), actual);
		}

        [Test]
        public void Find_GivenFirstGridSpaceIsNull_ThrowArgumentNullException() {
            GridSpace dest = new GridSpace(2, 2);
            Exception expected = new ArgumentNullException("from", ExceptionConstants.VA_ARGUMENT_NULL);

            Exception actual = Assert.Throws<ArgumentNullException>(() => {
                pathManager.Find(null, dest);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Find_GivenSecondGridSpaceIsNull_ThrowArgumentNullException() {
            GridSpace src = new GridSpace(0, 0);
            Exception expected = new ArgumentNullException("to", ExceptionConstants.VA_ARGUMENT_NULL);

            Exception actual = Assert.Throws<ArgumentNullException>(() => {
                pathManager.Find(src, null);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Find_GivenInvalidFirstSpaceWhereRowIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
            GridSpace src = new GridSpace(-1, 0);
            GridSpace dest = new GridSpace(2, 2);
            Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "from"));

            Exception actual = Assert.Throws<InvalidSpaceException>(() => {
                pathManager.Find(src, dest);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Find_GivenInvalidFirstSpaceWhereColumnIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
            GridSpace src = new GridSpace(0, -1);
            GridSpace dest = new GridSpace(2, 2);
            Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "from"));

            Exception actual = Assert.Throws<InvalidSpaceException>(() => {
                pathManager.Find(src, dest);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Find_GivenInvalidSecondSpaceWhereRowIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
            GridSpace src = new GridSpace(0, 0);
            GridSpace dest = new GridSpace(-2, 2);
            Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "to"));

            Exception actual = Assert.Throws<InvalidSpaceException>(() => {
                pathManager.Find(src, dest);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Find_GivenInvalidSecondSpaceWhereColumnIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
            GridSpace src = new GridSpace(0, 0);
            GridSpace dest = new GridSpace(2, -2);
            Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "to"));

            Exception actual = Assert.Throws<InvalidSpaceException>(() => {
                pathManager.Find(src, dest);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Find_GivenFirstSpaceWhereRowDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
            GridSpace src = new GridSpace(999, 0);
            GridSpace dest = new GridSpace(2, 2);
            Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "from");

            Exception actual = Assert.Throws<ArgumentException>(() => {
                pathManager.Find(src, dest);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Find_GivenFirstSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
            GridSpace src = new GridSpace(0, 999);
            GridSpace dest = new GridSpace(2, 2);
            Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "from");

            Exception actual = Assert.Throws<ArgumentException>(() => {
                pathManager.Find(src, dest);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Find_GivenSecondSpaceWhereRowDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
            GridSpace src = new GridSpace(0, 0);
            GridSpace dest = new GridSpace(999, 2);
            Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "to");

            Exception actual = Assert.Throws<ArgumentException>(() => {
                pathManager.Find(src, dest);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Find_GivenSecondSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
            GridSpace src = new GridSpace(0, 0);
            GridSpace dest = new GridSpace(2, 999);
            Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "to");

            Exception actual = Assert.Throws<ArgumentException>(() => {
                pathManager.Find(src, dest);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        private Path GetStubbedPath() {
            List<IEdge> edges = new List<IEdge>();
            edges.Add(new Edge(new Node(Position.Zero), new Node(new Position(0, 1)), Velocity.FromKilometersPerHour(0)));
            edges.Add(new Edge(new Node(new Position(0, 1)), new Node(new Position(1, 1)), Velocity.FromKilometersPerHour(0)));
            edges.Add(new Edge(new Node(new Position(1, 1)), new Node(new Position(2, 1)), Velocity.FromKilometersPerHour(0)));
            edges.Add(new Edge(new Node(new Position(2, 1)), new Node(new Position(2, 2)), Velocity.FromKilometersPerHour(0)));
            
            return new Path(PathType.Complete, edges);
        }
	}
}
