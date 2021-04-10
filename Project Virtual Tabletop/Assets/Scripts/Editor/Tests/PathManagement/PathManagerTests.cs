using System;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using Roy_T.AStar.Graphs;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Paths;
using Roy_T.AStar.Primitives;
using Zenject;
using NaughtyBikerGames.SDK.Editor.Tests;
using NaughtyBikerGames.ProjectVirtualTabletop.Adapters.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.Exceptions;
using NaughtyBikerGames.ProjectVirtualTabletop.PathManagement;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals.Installers;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.Tests.PathManagement {
	public class PathManagerTests : ZenjectTests {
        private PathManager pathManager;

        private IPathFinder pathFinder;
        private SignalBus signalBus;

        [SetUp]
        public void SetUp() {
            SignalBusInstaller.Install(Container);
            GridSignalsBaseInstaller.Install(Container);

            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 3;
            gridDetails.NumberOfColumns = 3;

            pathFinder = Substitute.For<IPathFinder>();
            signalBus = Container.Resolve<SignalBus>();
            
            pathManager = new PathManager(gridDetails, pathFinder, signalBus);
        }

        [Test]
        public void PathManager_GivenValidGridDetails_ReturnPathManagerWithCorrectGridSize() {
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 4;
            gridDetails.NumberOfColumns = 5;

            IPathFinder pathFinder = Substitute.For<IPathFinder>();
            SignalBus signalBus = Container.Resolve<SignalBus>();

            PathManager pathManager = new PathManager(gridDetails, pathFinder, signalBus);

            Assert.AreEqual(4, pathManager.GridSize.Rows);
            Assert.AreEqual(5, pathManager.GridSize.Columns);
        }

        [Test]
        public void PathManager_GivenValidGridDetails_ReturnPathManagerWithCorrectCellSize() {
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 4;
            gridDetails.NumberOfColumns = 5;

            IPathFinder pathFinder = Substitute.For<IPathFinder>();
            SignalBus signalBus = Container.Resolve<SignalBus>();

            PathManager pathManager = new PathManager(gridDetails, pathFinder, signalBus);

            Assert.AreEqual(Distance.FromMeters(AppConstants.SPACE_WIDTH_IN_METERS), pathManager.CellSize.Width);
            Assert.AreEqual(Distance.FromMeters(AppConstants.SPACE_HEIGHT_IN_METERS), pathManager.CellSize.Height);
        }

        [Test]
        public void PathManager_GivenValidGridDetails_ReturnPathManagerWithCorrectTraversalVelocity() {
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 4;
            gridDetails.NumberOfColumns = 5;

            IPathFinder pathFinder = Substitute.For<IPathFinder>();
            SignalBus signalBus = Container.Resolve<SignalBus>();

            PathManager pathManager = new PathManager(gridDetails, pathFinder, signalBus);

            Assert.AreEqual(1f, pathManager.TraversalVelocity.KilometersPerHour);
        }

        [Test]
        public void PathManager_GivenNullGridDetails_ThrowArgumentNullException() {
            IPathFinder pathFinder = Substitute.For<IPathFinder>();
            SignalBus signalBus = Container.Resolve<SignalBus>();
            Exception expected = new ArgumentNullException("gridDetails", ExceptionConstants.VA_ARGUMENT_NULL);

            Exception actual = Assert.Throws<ArgumentNullException>(() => {
                new PathManager(null, pathFinder, signalBus);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void PathManager_GivenNullPathFinder_ThrowArgumentNullException() {
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 4;
            gridDetails.NumberOfColumns = 5;
            SignalBus signalBus = Container.Resolve<SignalBus>();
            Exception expected = new ArgumentNullException("pathFinder", ExceptionConstants.VA_ARGUMENT_NULL);

            Exception actual = Assert.Throws<ArgumentNullException>(() => {
                new PathManager(gridDetails, null, signalBus);
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
            SignalBus signalBus = Container.Resolve<SignalBus>();
            Exception expected = new ArgumentException(
                string.Format(ExceptionConstants.VA_NUMBER_OF_ROWS_INVALID, numRows), 
                "gridDetails"
            );

            Exception actual = Assert.Throws<ArgumentException>(() => {
                new PathManager(gridDetails, pathFinder, signalBus);
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
            SignalBus signalBus = Container.Resolve<SignalBus>();
            Exception expected = new ArgumentException(
                string.Format(ExceptionConstants.VA_NUMBER_OF_COLUMNS_INVALID, numColumns), 
                "gridDetails"
            );

            Exception actual = Assert.Throws<ArgumentException>(() => {
                new PathManager(gridDetails, pathFinder, signalBus);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Initialize_GivenSignalBus_FiringTheGridInitializeSignalCallsSubscriptionToGridInitializeCallbackWithCorrectArgs() {
            List<GridSpace> spaces = new List<GridSpace>();
            GridSpace space = new GridSpace(1, 0);
            spaces.Add(space);
            List<Element> elements = new List<Element>();
            Token token = new Token(space);
            elements.Add(token);
            
            List<GridSpace> actualSpaces = null;
            List<Element> actualElements = null;
            pathManager.GridInitializeCallback = s => {
                actualElements = (List<Element>)s.Elements;
                actualSpaces = (List<GridSpace>)s.Spaces;
            };

            pathManager.Initialize();
            signalBus.Fire(new GridInitializeSignal(elements, spaces));

            Assert.AreEqual(elements, actualElements);
            Assert.AreEqual(spaces, actualSpaces);
        }

        [Test]
        public void Initialize_GivenSignalBus_FiringTheGridMoveSignalCallsSubscriptionToGridMoveCallbackWithCorrectArgs() {
            GridSpace from = new GridSpace(0, 0);
            GridSpace to = new GridSpace(2, 2);
            Token token = new Token(from);

            GridSpace actualFrom = null;
            GridSpace actualTo = null;
            Element actualElement = null;
            pathManager.GridMoveCallback = s => {
                actualElement = s.Element;
                actualFrom = s.From;
                actualTo = s.To;
            };

            pathManager.Initialize();
            signalBus.Fire(new GridMoveSignal(token, from, to));

            Assert.AreEqual(token, actualElement);
            Assert.AreEqual(from, actualFrom);
            Assert.AreEqual(to, actualTo);
        }

        [Test]
        public void Initialize_GivenSignalBus_FiringTheGridAddSignalCallsSubscriptionToGridAddCallbackWithCorrectArgs() {
            GridSpace space = new GridSpace(1, 1);
            Token token = new Token(space);

            GridSpace actualSpace = null;
            Element actualElement = null;
            pathManager.GridAddCallback = s => {
                actualElement = s.Element;
                actualSpace = s.Space;
            };

            pathManager.Initialize();
            signalBus.Fire(new GridAddSignal(token, space));

            Assert.AreEqual(token, actualElement);
            Assert.AreEqual(space, actualSpace);
        }

        [Test]
        public void Initialize_GivenSignalBus_FiringTheGridRemoveSignalCallsSubscriptionToGridRemoveCallbackWithCorrectArgs() {
            GridSpace space = new GridSpace(1, 1);
            Token token = new Token(space);

            GridSpace actualSpace = null;
            Element actualElement = null;
            pathManager.GridRemoveCallback = s => {
                actualElement = s.Element;
                actualSpace = s.Space;
            };

            pathManager.Initialize();
            signalBus.Fire(new GridRemoveSignal(token, space));

            Assert.AreEqual(token, actualElement);
            Assert.AreEqual(space, actualSpace);
        }

        //* The logical values used between Roy_T.AStar's Position and our GridSpace is flipped. This is because
        //* Position uses arguments (x, y) and GridSpace uses arguments (row, column) where column == x and row == y.
        //* i.e. Position(0, 1) == GridSpace(1, 0)
		[Test]
		public void Find_GivenTwoValidGridSpaces_ReturnGridPathWithListOfGridSpacesThatFallAlongTheShortestPathBetweenGivenSpacesInclusively() {
			GridSpace src = new GridSpace(0, 0);
            GridSpace dest = new GridSpace(2, 2);

            pathFinder.FindPath(GridPosition.Zero, new GridPosition(2, 2), Arg.Any<Grid>()).Returns(GetStubbedPath());

            List<GridSpace> actual = (List<GridSpace>)pathManager.Find(src, dest).Spaces;

            Assert.Contains(new GridSpace(0, 0), actual);
            Assert.Contains(new GridSpace(1, 0), actual);
            Assert.Contains(new GridSpace(1, 1), actual);
            Assert.Contains(new GridSpace(1, 2), actual);
            Assert.Contains(new GridSpace(2, 2), actual);
		}

        [Test]
		public void Find_GivenTwoValidGridSpaces_ReturnGridPathWithCountOfSpacesOnPathExclusiveOfStartingSpace() {
			GridSpace src = new GridSpace(0, 0);
            GridSpace dest = new GridSpace(2, 2);

            pathFinder.FindPath(GridPosition.Zero, new GridPosition(2, 2), Arg.Any<Grid>()).Returns(GetStubbedPath());

            int actual = pathManager.Find(src, dest).Count;

            Assert.AreEqual(4, actual);
		}

        [Test]
		public void Find_GivenTwoValidGridSpaces_ReturnGridPathWithDistanceInMetersOfPathRoundedToNearestWholeNumber() {
			GridSpace src = new GridSpace(0, 0);
            GridSpace dest = new GridSpace(2, 2);

            pathFinder.FindPath(GridPosition.Zero, new GridPosition(2, 2), Arg.Any<Grid>()).Returns(GetStubbedPath());

            float actual = pathManager.Find(src, dest).DistanceInMeters;

            Assert.AreEqual(6d, Math.Round(actual));
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
            Size cellSize = new Size(Distance.FromMeters(AppConstants.SPACE_WIDTH_IN_METERS), Distance.FromMeters(AppConstants.SPACE_HEIGHT_IN_METERS));
            List<IEdge> edges = new List<IEdge> {
                new Edge(new Node(Position.Zero), 
                    new Node(Position.FromOffset(cellSize.Width * 0, cellSize.Height * 1)), 
                    Velocity.FromKilometersPerHour(1)),
                new Edge(new Node(Position.FromOffset(cellSize.Width * 0, cellSize.Height * 1)), 
                    new Node(Position.FromOffset(cellSize.Width * 1, cellSize.Height * 1)), 
                    Velocity.FromKilometersPerHour(1)),
                new Edge(new Node(Position.FromOffset(cellSize.Width * 1, cellSize.Height * 1)), 
                    new Node(Position.FromOffset(cellSize.Width * 2, cellSize.Height * 1)), 
                    Velocity.FromKilometersPerHour(1)),
                new Edge(new Node(Position.FromOffset(cellSize.Width * 2, cellSize.Height * 1)), 
                    new Node(Position.FromOffset(cellSize.Width * 2, cellSize.Height * 2)), 
                    Velocity.FromKilometersPerHour(1))
            };
            
            return new Path(PathType.Complete, edges);
        }

        [Test]
        public void Disconnect_GivenValidGridSpace_DisconnectNodeForTheCorrespondingGridPosition() {
            GridSpace space = new GridSpace(1, 1);

            pathManager.Disconnect(space);
            INode actual = pathManager.Grid.GetNode(space.AsGridPosition());

            Assert.AreEqual(0, actual.Incoming.Count);
            Assert.AreEqual(0, actual.Outgoing.Count);
        }

        [Test]
        public void Disconnect_GivenGridSpaceIsNull_ThrowArgumentNullException() {
            Exception expected = new ArgumentNullException("space", ExceptionConstants.VA_ARGUMENT_NULL);

            Exception actual = Assert.Throws<ArgumentNullException>(() => {
                pathManager.Disconnect(null);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Disconnect_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
            GridSpace space = new GridSpace(-1, 1);
            Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "space"));

            Exception actual = Assert.Throws<InvalidSpaceException>(() => {
                pathManager.Disconnect(space);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Disconnect_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
            GridSpace space = new GridSpace(1, -1);
            Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "space"));

            Exception actual = Assert.Throws<InvalidSpaceException>(() => {
                pathManager.Disconnect(space);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Disconnect_GivenGridSpaceWhereRowDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
            GridSpace space = new GridSpace(999, 1);
            Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "space");

            Exception actual = Assert.Throws<ArgumentException>(() => {
                pathManager.Disconnect(space);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Disconnect_GivenGridSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
            GridSpace space = new GridSpace(1, 999);
            Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "space");

            Exception actual = Assert.Throws<ArgumentException>(() => {
                pathManager.Disconnect(space);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Reconnect_GivenValidGridSpaceSurroundedBy4ConnectedSpaces_ReconnectTheDisconnectedNodeToAll4SurroundingPositions() {
            GridSpace space = new GridSpace(1, 1);
            pathManager.Grid.DisconnectNode(space.AsGridPosition());

            pathManager.Reconnect(space);
            INode actual = pathManager.Grid.GetNode(space.AsGridPosition());

            Assert.AreEqual(4, actual.Incoming.Count);
            Assert.AreEqual(4, actual.Outgoing.Count);
        }

        [Test]
        public void Reconnect_GivenGridSpaceIsNull_ThrowArgumentNullException() {
            Exception expected = new ArgumentNullException("space", ExceptionConstants.VA_ARGUMENT_NULL);

            Exception actual = Assert.Throws<ArgumentNullException>(() => {
                pathManager.Reconnect(null);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Reconnect_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
            GridSpace space = new GridSpace(-1, 1);
            Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "space"));

            Exception actual = Assert.Throws<InvalidSpaceException>(() => {
                pathManager.Reconnect(space);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Reconnect_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
            GridSpace space = new GridSpace(1, -1);
            Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "space"));

            Exception actual = Assert.Throws<InvalidSpaceException>(() => {
                pathManager.Reconnect(space);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Reconnect_GivenGridSpaceWhereRowDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
            GridSpace space = new GridSpace(999, 1);
            Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "space");

            Exception actual = Assert.Throws<ArgumentException>(() => {
                pathManager.Reconnect(space);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Reconnect_GivenGridSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
            GridSpace space = new GridSpace(1, 999);
            Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "space");

            Exception actual = Assert.Throws<ArgumentException>(() => {
                pathManager.Reconnect(space);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void DisconnectAll_GivenListOfGridSpaces_DisconnectEachNodeForAllCorrespondingGridPositions() {
            List<GridSpace> spaces = new List<GridSpace>();
            spaces.Add(new GridSpace(1, 0));
            spaces.Add(new GridSpace(1, 1));
            spaces.Add(new GridSpace(2, 1));

            pathManager.DisconnectAll(spaces);
            List<INode> actuals = new List<INode>();
            actuals.Add(pathManager.Grid.GetNode(spaces[0].AsGridPosition()));
            actuals.Add(pathManager.Grid.GetNode(spaces[1].AsGridPosition()));
            actuals.Add(pathManager.Grid.GetNode(spaces[2].AsGridPosition()));

            Assert.AreEqual(0, actuals[0].Incoming.Count);
            Assert.AreEqual(0, actuals[0].Outgoing.Count);
            Assert.AreEqual(0, actuals[1].Incoming.Count);
            Assert.AreEqual(0, actuals[1].Outgoing.Count);
            Assert.AreEqual(0, actuals[2].Incoming.Count);
            Assert.AreEqual(0, actuals[2].Outgoing.Count);
        }

        [Test]
        public void DisconnectAll_GivenListIsNull_ThrowArgumentNullException() {
            Exception expected = new ArgumentNullException("spaces", ExceptionConstants.VA_ARGUMENT_NULL);

            Exception actual = Assert.Throws<ArgumentNullException>(() => {
                pathManager.DisconnectAll(null);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Dispose_WhenCalled_UnsubscribeFromAllSignals() {
            signalBus.Subscribe<GridInitializeSignal>(pathManager.GridInitializeCallback);
            signalBus.Subscribe<GridMoveSignal>(pathManager.GridMoveCallback);
            signalBus.Subscribe<GridAddSignal>(pathManager.GridAddCallback);
            signalBus.Subscribe<GridRemoveSignal>(pathManager.GridRemoveCallback);

            pathManager.Dispose();

            Assert.AreEqual(0, signalBus.NumSubscribers);
        }
	}
}
