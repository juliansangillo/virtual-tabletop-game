using NUnit.Framework;
using NaughtyBikerGames.ProjectVirtualTabletop.GridManagement;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using System;
using NaughtyBikerGames.ProjectVirtualTabletop.Exceptions;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.SDK.Editor.Tests;
using Zenject;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals.Installers;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals;
using System.Collections.Generic;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.Tests.GridManagement {
	public class GridManagerTests : ZenjectTests {
		private GridManager gridManager;
        private SignalBus signalBus;

        [SetUp]
        public void SetUp() {
            SignalBusInstaller.Install(Container);
            GridSignalsBaseInstaller.Install(Container);

            signalBus = Container.Resolve<SignalBus>();
        }

        [Test]
        public void Initialize_WhenCalled_FireAGridInitializeSignalWithCorrectArgs() {
            GridSpace expectedSpace1 = new GridSpace(0, 1);
            Element expectedElement1 = new Element(expectedSpace1);
            GridSpace expectedSpace2 = new GridSpace(1, 1);
            Element expectedElement2 = new Element(expectedSpace2);
            Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = expectedElement1;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = expectedElement2;
			gridManager = new GridManager(fakeGrid, signalBus);

            List<GridSpace> actualSpaces = new List<GridSpace>();
            List<Element> actualElements = new List<Element>();
            signalBus.Subscribe<GridInitializeSignal>(s => {
                actualElements = (List<Element>)s.Elements;
                actualSpaces = (List<GridSpace>)s.Spaces;
            });

            gridManager.Initialize();

            Assert.Contains(expectedElement1, actualElements);
            Assert.Contains(expectedSpace1, actualSpaces);
            Assert.Contains(expectedElement2, actualElements);
            Assert.Contains(expectedSpace2, actualSpaces);
        }

		[Test]
		public void AddTo_GivenSpaceAndAnElement_SetElementToCorrectLocationOnMap() {
			Element expected = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace space = new Entities.GridSpace(1, 0);

			gridManager.AddTo(space, expected);
			Element actual = gridManager.Grid[1,0];

			Assert.AreEqual(expected, actual);
		}

        [Test]
        public void AddTo_GivenSpaceAndAnElement_FireAGridAddSignalWithCorrectArgs() {
            Element expected = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace space = new Entities.GridSpace(1, 0);

            GridSpace actualSpace = null;
            Element actualElement = null;
            signalBus.Subscribe<GridAddSignal>(s => {
                actualElement = s.Element;
                actualSpace = s.Space;
            });

			gridManager.AddTo(space, expected);

            Assert.AreEqual(expected, actualElement);
            Assert.AreEqual(space, actualSpace);
        }

		[Test]
		public void AddTo_GivenSpaceWhereAnElementIsAlreadyLocatedOnIt_ThrowInvalidOperationException() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = element;
			gridManager = new GridManager(fakeGrid, signalBus);

			Assert.Throws<InvalidOperationException>(() => {
				gridManager.AddTo(new Entities.GridSpace(1, 1), new Element(null));
			});
		}

		[Test]
		public void AddTo_GivenNullSpace_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);

			Exception expected = new ArgumentNullException("space", ExceptionConstants.VA_ARGUMENT_NULL);

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				gridManager.AddTo(null, element);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void AddTo_GivenNullElement_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace space = new Entities.GridSpace(1, 1);

			Exception expected = new ArgumentNullException("element", ExceptionConstants.VA_ARGUMENT_NULL);

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				gridManager.AddTo(space, null);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void AddTo_GivenSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(5, 1);

			Assert.Throws<ArgumentException>(() => {
				gridManager.AddTo(nonExistingSpace, element);
			});
		}

		[Test]
		public void AddTo_GivenSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(1, 5);

			Assert.Throws<ArgumentException>(() => {
				gridManager.AddTo(nonExistingSpace, element);
			});
		}

		[Test]
		public void AddTo_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace invalidSpace = new Entities.GridSpace(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.AddTo(invalidSpace, element);
			});
		}

		[Test]
		public void AddTo_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace invalidSpace = new Entities.GridSpace(0, -1);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.AddTo(invalidSpace, element);
			});
		}

		[Test]
		public void GetElementOn_GivenSpace_ReturnCorrectElement() {
			Element expected = new Element(null);
			Element notExpected = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = expected;
			fakeGrid[1,0] = notExpected;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace expectedSpace = new Entities.GridSpace(0, 1);

			Element actual = gridManager.GetElementOn(expectedSpace);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetElementOn_GivenNullSpace_ThrowArgumentNullException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);

			Assert.Throws<ArgumentNullException>(() => {
				gridManager.GetElementOn(null);
			});
		}

		[Test]
		public void GetElementOn_GivenSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(5, 1);

			Assert.Throws<ArgumentException>(() => {
				gridManager.GetElementOn(nonExistingSpace);
			});
		}

		[Test]
		public void GetElementOn_GivenSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(1, 5);

			Assert.Throws<ArgumentException>(() => {
				gridManager.GetElementOn(nonExistingSpace);
			});
		}

		[Test]
		public void GetElementOn_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace invalidSpace = new Entities.GridSpace(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.GetElementOn(invalidSpace);
			});
		}

		[Test]
		public void GetElementOn_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace invalidSpace = new Entities.GridSpace(0, -1);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.GetElementOn(invalidSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenSpaceThatExistsOnMapAndSpaceIsEmpty_ReturnTrue() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = element;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace expectedSpace = new Entities.GridSpace(1, 1);

			bool result = gridManager.IsEmpty(expectedSpace);

			Assert.True(result);
		}

		[Test]
		public void IsEmpty_GivenSpaceThatExistsOnMapAndSpaceIsNotEmpty_ReturnFalse() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = element;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace expectedSpace = new Entities.GridSpace(0, 0);

			bool result = gridManager.IsEmpty(expectedSpace);

			Assert.False(result);
		}

		[Test]
		public void IsEmpty_GivenNullSpace_ThrowArgumentNullException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);

			Assert.Throws<ArgumentNullException>(() => {
				gridManager.IsEmpty(null);
			});
		}

		[Test]
		public void IsEmpty_GivenSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(5, 1);

			Assert.Throws<ArgumentException>(() => {
				gridManager.IsEmpty(nonExistingSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(1, 5);

			Assert.Throws<ArgumentException>(() => {
				gridManager.IsEmpty(nonExistingSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace invalidSpace = new Entities.GridSpace(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.IsEmpty(invalidSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace invalidSpace = new Entities.GridSpace(0, -1);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.IsEmpty(invalidSpace);
			});
		}

		[Test]
		public void Move_GivenTwoSpacesWhereAnElementIsLocatedOnTheFirstSpaceAndTheSecondSpaceIsEmpty_MoveElementFromTheFirstSpaceToTheSecondSpace() {
			Element expected = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = expected;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(fakeGrid, signalBus);

			gridManager.Move(new Entities.GridSpace(0, 0), new Entities.GridSpace(1, 1));
			Element actual = gridManager.Grid[1,1];

			Assert.AreEqual(expected, actual);
		}

        [Test]
        public void Move_GivenTwoSpacesWhereAnElementIsLocatedOnTheFirstSpaceAndTheSecondSpaceIsEmpty_FireAGridMoveSignalWithCorrectArgs() {
            Element expected = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = expected;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(fakeGrid, signalBus);
            GridSpace expectedFrom = new Entities.GridSpace(0, 0);
            GridSpace expectedTo = new Entities.GridSpace(1, 1);

            GridSpace actualFrom = null;
            GridSpace actualTo = null;
            Element actualElement = null;
            signalBus.Subscribe<GridMoveSignal>(s => {
                actualElement = s.Element;
                actualFrom = s.From;
                actualTo = s.To;
            });

			gridManager.Move(expectedFrom, expectedTo);

            Assert.AreEqual(expected, actualElement);
            Assert.AreEqual(expectedFrom, actualFrom);
            Assert.AreEqual(expectedTo, actualTo);
        }

		[Test]
		public void Move_GivenTwoSpacesWhereAnElementIsAlreadyLocatedOnTheSecondSpace_ThrowInvalidOperationExceptionWithCorrectMessage() {
			Element element1 = new Element(null);
			Element element2 = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = element1;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = element2;
			gridManager = new GridManager(fakeGrid, signalBus);

			Exception expected = new InvalidOperationException(ExceptionConstants.VA_ELEMENT_EXISTS_ON_SPACE);

			Exception actual = Assert.Throws<InvalidOperationException>(() => {
				gridManager.Move(new Entities.GridSpace(0, 0), new Entities.GridSpace(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenTwoSpacesWhereTheFirstSpaceIsEmpty_ThrowInvalidOperationExceptionWithCorrectMessage() {
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(fakeGrid, signalBus);

			Exception expected = new InvalidOperationException(ExceptionConstants.VA_ELEMENT_DOESNT_EXIST_ON_SPACE);

			Exception actual = Assert.Throws<InvalidOperationException>(() => {
				gridManager.Move(new Entities.GridSpace(0, 0), new Entities.GridSpace(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenFirstSpaceWhereRowDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(5, 1);

			Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "from");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				gridManager.Move(nonExistingSpace, new Entities.GridSpace(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenFirstSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(1, 5);

			Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "from");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				gridManager.Move(nonExistingSpace, new Entities.GridSpace(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenSecondSpaceWhereRowDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(5, 1);

			Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "to");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				gridManager.Move(new Entities.GridSpace(0, 0), nonExistingSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenSecondSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(1, 5);

			Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "to");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				gridManager.Move(new Entities.GridSpace(0, 0), nonExistingSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidFirstSpaceWhereRowIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace invalidSpace = new Entities.GridSpace(-1, 0);

			Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "from"));

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				gridManager.Move(invalidSpace, new Entities.GridSpace(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidFirstSpaceWhereColumnIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace invalidSpace = new Entities.GridSpace(0, -1);

			Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "from"));

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				gridManager.Move(invalidSpace, new Entities.GridSpace(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidSecondSpaceWhereRowIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace invalidSpace = new Entities.GridSpace(-1, 0);

			Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "to"));

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				gridManager.Move(new Entities.GridSpace(0, 0), invalidSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidSecondSpaceWhereColumnIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace invalidSpace = new Entities.GridSpace(0, -1);

			Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "to"));

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				gridManager.Move(new Entities.GridSpace(0, 0), invalidSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenNullFirstSpace_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);

			Exception expected = new ArgumentNullException("from", ExceptionConstants.VA_ARGUMENT_NULL);

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				gridManager.Move(null, new Entities.GridSpace(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenNullSecondSpace_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);

			Exception expected = new ArgumentNullException("to", ExceptionConstants.VA_ARGUMENT_NULL);

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				gridManager.Move(new Entities.GridSpace(0, 0), null);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void RemoveFrom_GivenSpace_RemoveElementFromThatSpaceAndReturnTheElement() {
			Element expected = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = expected;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(fakeGrid, signalBus);

			Element actual = gridManager.RemoveFrom(new Entities.GridSpace(0, 1));

			Assert.IsNull(gridManager.Grid[0,1]);
			Assert.AreEqual(expected, actual);
		}

        [Test]
        public void RemoveFrom_GivenSpace_FireAGridRemoveSignalWithCorrectArgs() {
            Element expected = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = expected;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(fakeGrid, signalBus);
            GridSpace expectedSpace = new Entities.GridSpace(0, 1);

            GridSpace actualSpace = null;
            Element actualElement = null;
            signalBus.Subscribe<GridRemoveSignal>(s => {
                actualElement = s.Element;
                actualSpace = s.Space;
            });

			gridManager.RemoveFrom(expectedSpace);

            Assert.AreEqual(expected, actualElement);
            Assert.AreEqual(expectedSpace, actualSpace);
        }

		[Test]
		public void RemoveFrom_GivenSpaceWhereAnElementDoesNotExistOnThatSpace_ThrowInvalidOperationException() {
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(fakeGrid, signalBus);

			Assert.Throws<InvalidOperationException>(() => {
				gridManager.RemoveFrom(new Entities.GridSpace(0, 1));
			});
		}

		[Test]
		public void RemoveFrom_GivenSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(5, 1);

			Assert.Throws<ArgumentException>(() => {
				gridManager.RemoveFrom(nonExistingSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(1, 5);

			Assert.Throws<ArgumentException>(() => {
				gridManager.RemoveFrom(nonExistingSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace invalidSpace = new Entities.GridSpace(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.RemoveFrom(invalidSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);
			Entities.GridSpace invalidSpace = new Entities.GridSpace(0, -1);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.RemoveFrom(invalidSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenNullSpace_ThrowArgumentNullException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid, signalBus);

			Assert.Throws<ArgumentNullException>(() => {
				gridManager.RemoveFrom(null);
			});
		}
	}
}
