using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NaughtyBikerGames.ProjectVirtualTabletop.Components;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.Exceptions;
using NaughtyBikerGames.ProjectVirtualTabletop.Utilities;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.UnityTests.Utilities {
	public class GameObjectUtilsTests {
        GameObject gameObject1;
        GameObject gameObject2;
        GameObject gameObject3;

        [SetUp]
        public void SetUp() {
            gameObject1 = new GameObject("Some Random Obj");
            gameObject2 = new GameObject("Tile 1");
            gameObject3 = new GameObject("Tile 2");

            gameObject1.tag = "Player";
            gameObject2.tag = AppConstants.GRID_SPACE_TAG;
            gameObject3.tag = AppConstants.GRID_SPACE_TAG;

            GridSpaceMono gridSpaceMono = gameObject2.AddComponent<GridSpaceMono>();
            gridSpaceMono.Row = 1;
            gridSpaceMono.Col = 0;
            gridSpaceMono.Awake();

            gridSpaceMono = gameObject3.AddComponent<GridSpaceMono>();
            gridSpaceMono.Row = 2;
            gridSpaceMono.Col = 2;
            gridSpaceMono.Awake();
        }

        [UnityTest]
        public IEnumerator FindGameObjectFromGridSpace_GivenGridSpaceThatDoesntExist_ReturnNull() {
            yield return null;

            GameObject actual = GameObjectUtils.FindGameObjectFromGridSpace(new GridSpace(3, 4));

            yield return null;

            Assert.IsNull(actual);
        }

        [UnityTest]
        public IEnumerator FindGameObjectFromGridSpace_GivenGridSpaceThatExists_ReturnGameObjectThatRepresentsThatGridSpace() {
            yield return null;

            GameObject actual = GameObjectUtils.FindGameObjectFromGridSpace(new GridSpace(1, 0));

            yield return null;

            Assert.AreEqual(gameObject2.name, actual.name);
        }

        [UnityTest]
        public IEnumerator FindGameObjectFromGridSpace_GivenGridSpaceThatIsNull_ThrowArgumentNullException() {
            Exception expected = new ArgumentNullException("space", ExceptionConstants.VA_ARGUMENT_NULL);
            
            yield return null;

            Exception actual = Assert.Throws<ArgumentNullException>(() => {
                GameObjectUtils.FindGameObjectFromGridSpace(null);
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [UnityTest]
        public IEnumerator FindGameObjectFromGridSpace_GivenGridSpaceThatIsInvalidWhereRowIsNegative_ThrowInvalidSpaceException() {
            Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "space"));
            
            yield return null;

            Exception actual = Assert.Throws<InvalidSpaceException>(() => {
                GameObjectUtils.FindGameObjectFromGridSpace(new GridSpace(-1, 0));
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [UnityTest]
        public IEnumerator FindGameObjectFromGridSpace_GivenGridSpaceThatIsInvalidWhereColumnIsNegative_ThrowInvalidSpaceException() {
            Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "space"));
            
            yield return null;

            Exception actual = Assert.Throws<InvalidSpaceException>(() => {
                GameObjectUtils.FindGameObjectFromGridSpace(new GridSpace(0, -1));
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }
	}
}