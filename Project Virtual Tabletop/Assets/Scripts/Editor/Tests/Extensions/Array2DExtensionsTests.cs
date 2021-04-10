using System;
using NUnit.Framework;
using NaughtyBikerGames.ProjectVirtualTabletop.Extensions;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.Tests.Extensions {
	public class Array2DExtensionsTests {
        [Test]
        public void AsFlat_GivenA2dSquareArray_ReturnAFlatArrayWithCorrectValues() {
            Object[,] objects = new Object[2, 2];
            objects[0, 0] = "string";
            objects[0, 1] = null;
            objects[1, 0] = 5;
            objects[1, 1] = true;

            Object[] actual = objects.AsFlat();

            Assert.AreEqual("string", actual[0]);
            Assert.AreEqual(null, actual[1]);
            Assert.AreEqual(5, actual[2]);
            Assert.AreEqual(true, actual[3]);
        }
	}
}