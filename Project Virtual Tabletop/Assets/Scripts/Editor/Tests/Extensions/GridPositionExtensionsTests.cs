using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.Extensions;
using NUnit.Framework;
using Roy_T.AStar.Primitives;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.Tests.Extensions {
	public class GridPositionExtensionsTests {
        [Test]
        public void AsGridSpace_GivenGridPosition_ReturnGridSpaceWithCorrectRowAndColumnValues() {
            GridPosition gridPosition = new GridPosition(1, 0);
            GridSpace expected = new GridSpace(0, 1);
            
            GridSpace actual = gridPosition.AsGridSpace();

            Assert.AreEqual(expected, actual);
        }
	}
}