using System.Linq;
using NUnit.Framework;
using Roy_T.AStar.Graphs;
using Roy_T.AStar.Grids;
using Roy_T.AStar.Primitives;
using NaughtyBikerGames.ProjectVirtualTabletop.Extensions;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.Tests.Extensions {
	public class GridExtensionsTests {
        private Grid grid;

        [SetUp]
        public void SetUp() {
            grid = Grid.CreateGridWithLateralConnections(
                new GridSize(3, 3),
                new Size(Distance.FromMeters(1), Distance.FromMeters(1)),
                Velocity.FromKilometersPerHour(10)
            );
        }

        [Test]
        public void ReconnectNode_GivenGridPositionInMiddleOfGrid_AddIncomingAndOutgoingEdgesToAll4SurroundingPositions() {
            GridPosition position = new GridPosition(1, 1);
            grid.DisconnectNode(position);
            INode node = grid.GetNode(position);

            Assert.AreEqual(0, node.Incoming.Count);
            Assert.AreEqual(0, node.Outgoing.Count);

            grid.ReconnectNode(position, Velocity.FromKilometersPerHour(10));

            Assert.AreEqual(4, node.Incoming.Count);
            Assert.AreEqual(1, node.Incoming.Where(edge => edge.Start == grid.GetNode(new GridPosition(1, 0))).Count());
            Assert.AreEqual(1, node.Incoming.Where(edge => edge.Start == grid.GetNode(new GridPosition(0, 1))).Count());
            Assert.AreEqual(1, node.Incoming.Where(edge => edge.Start == grid.GetNode(new GridPosition(1, 2))).Count());
            Assert.AreEqual(1, node.Incoming.Where(edge => edge.Start == grid.GetNode(new GridPosition(2, 1))).Count());
            Assert.AreEqual(4, node.Outgoing.Count);
            Assert.AreEqual(1, node.Outgoing.Where(edge => edge.End == grid.GetNode(new GridPosition(1, 0))).Count());
            Assert.AreEqual(1, node.Outgoing.Where(edge => edge.End == grid.GetNode(new GridPosition(0, 1))).Count());
            Assert.AreEqual(1, node.Outgoing.Where(edge => edge.End == grid.GetNode(new GridPosition(1, 2))).Count());
            Assert.AreEqual(1, node.Outgoing.Where(edge => edge.End == grid.GetNode(new GridPosition(2, 1))).Count());
        }

        [Test]
        public void ReconnectNode_GivenGridPositionOnEdgeOfGrid_AddIncomingAndOutgoingEdgesToAll3SurroundingPositions() {
            GridPosition position = new GridPosition(1, 2);
            grid.DisconnectNode(position);
            INode node = grid.GetNode(position);

            Assert.AreEqual(0, node.Incoming.Count);
            Assert.AreEqual(0, node.Outgoing.Count);

            grid.ReconnectNode(position, Velocity.FromKilometersPerHour(10));

            Assert.AreEqual(3, node.Incoming.Count);
            Assert.AreEqual(1, node.Incoming.Where(edge => edge.Start == grid.GetNode(new GridPosition(1, 1))).Count());
            Assert.AreEqual(1, node.Incoming.Where(edge => edge.Start == grid.GetNode(new GridPosition(0, 2))).Count());
            Assert.AreEqual(1, node.Incoming.Where(edge => edge.Start == grid.GetNode(new GridPosition(2, 2))).Count());
            Assert.AreEqual(3, node.Outgoing.Count);
            Assert.AreEqual(1, node.Outgoing.Where(edge => edge.End == grid.GetNode(new GridPosition(1, 1))).Count());
            Assert.AreEqual(1, node.Outgoing.Where(edge => edge.End == grid.GetNode(new GridPosition(0, 2))).Count());
            Assert.AreEqual(1, node.Outgoing.Where(edge => edge.End == grid.GetNode(new GridPosition(2, 2))).Count());
        }

        [Test]
        public void ReconnectNode_GivenGridPositionOnBottomRightCornerOfGrid_AddIncomingAndOutgoingEdgesToAll2SurroundingPositions() {
            GridPosition position = new GridPosition(2, 0);
            grid.DisconnectNode(position);
            INode node = grid.GetNode(position);

            Assert.AreEqual(0, node.Incoming.Count);
            Assert.AreEqual(0, node.Outgoing.Count);

            grid.ReconnectNode(position, Velocity.FromKilometersPerHour(10));

            Assert.AreEqual(2, node.Incoming.Count);
            Assert.AreEqual(1, node.Incoming.Where(edge => edge.Start == grid.GetNode(new GridPosition(1, 0))).Count());
            Assert.AreEqual(1, node.Incoming.Where(edge => edge.Start == grid.GetNode(new GridPosition(2, 1))).Count());
            Assert.AreEqual(2, node.Outgoing.Count);
            Assert.AreEqual(1, node.Outgoing.Where(edge => edge.End == grid.GetNode(new GridPosition(1, 0))).Count());
            Assert.AreEqual(1, node.Outgoing.Where(edge => edge.End == grid.GetNode(new GridPosition(2, 1))).Count());
        }

        [Test]
        public void ReconnectNode_GivenGridPositionOnTopLeftCornerOfGridAndBottomPositionIsAlsoDisconnected_AddIncomingAndOutgoingEdgesToRightPosition() {
            GridPosition position = new GridPosition(0, 2);
            GridPosition other = new GridPosition(0, 1);
            grid.DisconnectNode(position);
            grid.DisconnectNode(other);
            INode node = grid.GetNode(position);
            INode disconnectedNode = grid.GetNode(other);

            Assert.AreEqual(0, node.Incoming.Count);
            Assert.AreEqual(0, node.Outgoing.Count);
            Assert.AreEqual(0, disconnectedNode.Incoming.Count);
            Assert.AreEqual(0, disconnectedNode.Outgoing.Count);

            grid.ReconnectNode(position, Velocity.FromKilometersPerHour(10));

            Assert.AreEqual(1, node.Incoming.Count);
            Assert.AreEqual(1, node.Incoming.Where(edge => edge.Start == grid.GetNode(new GridPosition(1, 2))).Count());
            Assert.AreEqual(1, node.Outgoing.Count);
            Assert.AreEqual(1, node.Outgoing.Where(edge => edge.End == grid.GetNode(new GridPosition(1, 2))).Count());
        }

        [Test]
        public void ReconnectNode_GivenGridPositionThatIsNotDisconnected_DoNothing() {
            GridPosition position = new GridPosition(1, 1);
            INode node = grid.GetNode(position);

            Assert.AreEqual(4, node.Incoming.Count);
            Assert.AreEqual(4, node.Outgoing.Count);

            grid.ReconnectNode(position, Velocity.FromKilometersPerHour(10));

            Assert.AreEqual(4, node.Incoming.Count);
            Assert.AreEqual(4, node.Outgoing.Count);
        }

        [Test]
        public void IsInsideGrid_GivenGridPositionWhereNodeExistsInsideGrid_ReturnTrue() {
            GridPosition position = new GridPosition(1, 1);

            Assert.True(grid.IsInsideGrid(position));
        }

        [Test]
        public void IsInsideGrid_GivenGridPositionWherePositionXIsLessThanBoundary_ReturnFalse() {
            GridPosition position = new GridPosition(-1, 1);

            Assert.False(grid.IsInsideGrid(position));
        }

        [Test]
        public void IsInsideGrid_GivenGridPositionWherePositionXIsGreaterThanBoundary_ReturnFalse() {
            GridPosition position = new GridPosition(999, 1);

            Assert.False(grid.IsInsideGrid(position));
        }

        [Test]
        public void IsInsideGrid_GivenGridPositionWherePositionYIsLessThanBoundary_ReturnFalse() {
            GridPosition position = new GridPosition(1, -1);

            Assert.False(grid.IsInsideGrid(position));
        }

        [Test]
        public void IsInsideGrid_GivenGridPositionWherePositionYIsGreaterThanBoundary_ReturnFalse() {
            GridPosition position = new GridPosition(1, 999);

            Assert.False(grid.IsInsideGrid(position));
        }

        [Test]
        public void IsDisconnectedNode_GivenGridPositionWhereNodeHasNoConnections_ReturnTrue() {
            GridPosition position = new GridPosition(1, 1);
            grid.DisconnectNode(position);

            Assert.True(grid.IsDisconnectedNode(position));
        }

        [Test]
        public void IsDisconnectedNode_GivenGridPositionWhereNodeHasIncomingConnections_ReturnFalse() {
            GridPosition position = new GridPosition(1, 1);
            grid.RemoveEdge(position, new GridPosition(1, 0));
            grid.RemoveEdge(position, new GridPosition(0, 1));
            grid.RemoveEdge(position, new GridPosition(1, 2));
            grid.RemoveEdge(position, new GridPosition(2, 1));

            Assert.False(grid.IsDisconnectedNode(position));
        }

        [Test]
        public void IsDisconnectedNode_GivenGridPositionWhereNodeHasOutgoingConnections_ReturnFalse() {
            GridPosition position = new GridPosition(1, 1);
            grid.RemoveEdge(new GridPosition(1, 0), position);
            grid.RemoveEdge(new GridPosition(0, 1), position);
            grid.RemoveEdge(new GridPosition(1, 2), position);
            grid.RemoveEdge(new GridPosition(2, 1), position);

            Assert.False(grid.IsDisconnectedNode(position));
        }
	}
}