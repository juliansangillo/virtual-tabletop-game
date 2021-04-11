using System;
using System.Collections.Generic;
using NUnit.Framework;
using Zenject;
using NaughtyBikerGames.SDK.Editor.Tests;
using NaughtyBikerGames.ProjectVirtualTabletop.GridManagement;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.Exceptions;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals;
using NaughtyBikerGames.ProjectVirtualTabletop.Signals.Installers;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.Tests.GridManagement {
	public class GridManagerTests : ZenjectTests {
		private GridManager gridManager;
        private GridDetails gridDetails;
        private SignalBus signalBus;

        [SetUp]
        public void SetUp() {
            SignalBusInstaller.Install(Container);
            GridSignalsInstaller.Install(Container);

            gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 3;
            gridDetails.NumberOfColumns = 3;
            gridDetails.Tokens = new List<Token>();

            signalBus = Container.Resolve<SignalBus>();
        }

        [TestCase(5,3,0,0)]
        [TestCase(10,10,9,9)]
		public void Initialize_GivenGridDetailsWithOneToken_CreateGrid(int numRows, int numColumns, int row1, int col1) {
            Token token1 = new Token(new GridSpace(row1, col1));
			GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = numRows;
            gridDetails.NumberOfColumns = numColumns;
            gridDetails.Tokens = new List<Token>();
            gridDetails.Tokens.Add(token1);
            gridManager = new GridManager(gridDetails, signalBus);

            gridManager.Initialize();

            Assert.AreEqual(numRows, gridManager.Grid.GetLength(AppConstants.ROW_DIMENSION));
            Assert.AreEqual(numColumns, gridManager.Grid.GetLength(AppConstants.COLUMN_DIMENSION));
            Assert.AreEqual(token1, gridManager.Grid[row1, col1]);
		}

        [TestCase(5,3,0,0,4,0,4,2)]
		public void Initialize_GivenGridDetailsWithThreeTokens_CreateGrid(int numRows, int numColumns, int row1, int col1, 
        int row2, int col2, int row3, int col3) {
            Token token1 = new Token(new GridSpace(row1, col1));
            Token token2 = new Token(new GridSpace(row2, col2));
            Token token3 = new Token(new GridSpace(row3, col3));
			GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = numRows;
            gridDetails.NumberOfColumns = numColumns;
            gridDetails.Tokens = new List<Token>();
            gridDetails.Tokens.Add(token1);
            gridDetails.Tokens.Add(token2);
            gridDetails.Tokens.Add(token3);
            gridManager = new GridManager(gridDetails, signalBus);

            gridManager.Initialize();            

            Assert.AreEqual(numRows, gridManager.Grid.GetLength(AppConstants.ROW_DIMENSION));
            Assert.AreEqual(numColumns, gridManager.Grid.GetLength(AppConstants.COLUMN_DIMENSION));
            Assert.AreEqual(token1, gridManager.Grid[row1, col1]);
            Assert.AreEqual(token2, gridManager.Grid[row2, col2]);
            Assert.AreEqual(token3, gridManager.Grid[row3, col3]);
		}

        [TestCase(0)]
        [TestCase(-1)]
        public void Initialize_GivenInvalidGridDetailsWhereNumberOfRowsAreZeroOrNegative_ThrowArgumentException(int numRows) {
            Token token1 = new Token(new GridSpace(0, 0));
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = numRows;
            gridDetails.NumberOfColumns = 1;
            gridDetails.Tokens = new List<Token>();
            gridDetails.Tokens.Add(token1);
            gridManager = new GridManager(gridDetails, signalBus);

            Exception expected = new ArgumentException(
                string.Format(ExceptionConstants.VA_NUMBER_OF_ROWS_INVALID, numRows), 
                "gridDetails"
            );

            Exception actual = Assert.Throws<ArgumentException>(() => {
                gridManager.Initialize();
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Initialize_GivenInvalidGridDetailsWhereNumberOfColumnsAreZeroOrNegative_ThrowArgumentException(int numColumns) {
            Token token1 = new Token(new GridSpace(0, 0));
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 1;
            gridDetails.NumberOfColumns = numColumns;
            gridDetails.Tokens = new List<Token>();
            gridDetails.Tokens.Add(token1);
            gridManager = new GridManager(gridDetails, signalBus);

            Exception expected = new ArgumentException(
                string.Format(ExceptionConstants.VA_NUMBER_OF_COLUMNS_INVALID, numColumns), 
                "gridDetails"
            );

            Exception actual = Assert.Throws<ArgumentException>(() => {
                gridManager.Initialize();
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Initialize_GivenInvalidGridDetailsWhereListOfTokensIsNull_ThrowArgumentException() {
            GridDetails gridDetails = new GridDetails();
            gridDetails.NumberOfRows = 1;
            gridDetails.NumberOfColumns = 1;
            gridDetails.Tokens = null;
            gridManager = new GridManager(gridDetails, signalBus);

            Exception expected = new ArgumentException(ExceptionConstants.VA_LIST_OF_TOKENS_INVALID, "gridDetails");

            Exception actual = Assert.Throws<ArgumentException>(() => {
                gridManager.Initialize();
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Initialize_GivenNullGridDetails_ThrowArgumentNullException() {
            gridManager = new GridManager(null, signalBus);
            Exception expected = new ArgumentNullException("gridDetails", ExceptionConstants.VA_ARGUMENT_NULL);

            Exception actual = Assert.Throws<ArgumentNullException>(() => {
                gridManager.Initialize();
            });
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [Test]
        public void Initialize_WhenCalled_FireAGridInitializeSignalWithCorrectArgs() {
            GridSpace expectedSpace1 = new GridSpace(0, 1);
            Element expectedElement1 = new Token(expectedSpace1);
            GridSpace expectedSpace2 = new GridSpace(1, 1);
            Element expectedElement2 = new Token(expectedSpace2);
            Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = expectedElement1;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = expectedElement2;
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;

            List<GridSpace> actualSpaces = new List<GridSpace>();
            List<Element> actualElements = new List<Element>();
            signalBus.Subscribe<GridInitializedSignal>(s => {
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
			Element expected = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace space = new Entities.GridSpace(1, 0);

			gridManager.AddTo(space, expected);
			Element actual = gridManager.Grid[1,0];

			Assert.AreEqual(expected, actual);
		}

        [Test]
        public void AddTo_GivenSpaceAndAnElement_FireAGridAddSignalWithCorrectArgs() {
            Element expected = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace space = new Entities.GridSpace(1, 0);

            GridSpace actualSpace = null;
            Element actualElement = null;
            signalBus.Subscribe<GridAddedSignal>(s => {
                actualElement = s.Element;
                actualSpace = s.Space;
            });

			gridManager.AddTo(space, expected);

            Assert.AreEqual(expected, actualElement);
            Assert.AreEqual(space, actualSpace);
        }

		[Test]
		public void AddTo_GivenSpaceWhereAnElementIsAlreadyLocatedOnIt_ThrowInvalidOperationException() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = element;
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;

			Assert.Throws<InvalidOperationException>(() => {
				gridManager.AddTo(new Entities.GridSpace(1, 1), new Token(null));
			});
		}

		[Test]
		public void AddTo_GivenNullSpace_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;

			Exception expected = new ArgumentNullException("space", ExceptionConstants.VA_ARGUMENT_NULL);

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				gridManager.AddTo(null, element);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void AddTo_GivenNullElement_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace space = new Entities.GridSpace(1, 1);

			Exception expected = new ArgumentNullException("element", ExceptionConstants.VA_ARGUMENT_NULL);

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				gridManager.AddTo(space, null);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void AddTo_GivenSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(5, 1);

			Assert.Throws<ArgumentException>(() => {
				gridManager.AddTo(nonExistingSpace, element);
			});
		}

		[Test]
		public void AddTo_GivenSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(1, 5);

			Assert.Throws<ArgumentException>(() => {
				gridManager.AddTo(nonExistingSpace, element);
			});
		}

		[Test]
		public void AddTo_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace invalidSpace = new Entities.GridSpace(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.AddTo(invalidSpace, element);
			});
		}

		[Test]
		public void AddTo_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace invalidSpace = new Entities.GridSpace(0, -1);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.AddTo(invalidSpace, element);
			});
		}

		[Test]
		public void GetElementOn_GivenSpace_ReturnCorrectElement() {
			Element expected = new Token(null);
			Element notExpected = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = expected;
			fakeGrid[1,0] = notExpected;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace expectedSpace = new Entities.GridSpace(0, 1);

			Element actual = gridManager.GetElementOn(expectedSpace);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetElementOn_GivenNullSpace_ThrowArgumentNullException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;

			Assert.Throws<ArgumentNullException>(() => {
				gridManager.GetElementOn(null);
			});
		}

		[Test]
		public void GetElementOn_GivenSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(5, 1);

			Assert.Throws<ArgumentException>(() => {
				gridManager.GetElementOn(nonExistingSpace);
			});
		}

		[Test]
		public void GetElementOn_GivenSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(1, 5);

			Assert.Throws<ArgumentException>(() => {
				gridManager.GetElementOn(nonExistingSpace);
			});
		}

		[Test]
		public void GetElementOn_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace invalidSpace = new Entities.GridSpace(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.GetElementOn(invalidSpace);
			});
		}

		[Test]
		public void GetElementOn_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace invalidSpace = new Entities.GridSpace(0, -1);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.GetElementOn(invalidSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenSpaceThatExistsOnMapAndSpaceIsEmpty_ReturnTrue() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = element;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace expectedSpace = new Entities.GridSpace(1, 1);

			bool result = gridManager.IsEmpty(expectedSpace);

			Assert.True(result);
		}

		[Test]
		public void IsEmpty_GivenSpaceThatExistsOnMapAndSpaceIsNotEmpty_ReturnFalse() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = element;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace expectedSpace = new Entities.GridSpace(0, 0);

			bool result = gridManager.IsEmpty(expectedSpace);

			Assert.False(result);
		}

		[Test]
		public void IsEmpty_GivenNullSpace_ThrowArgumentNullException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;

			Assert.Throws<ArgumentNullException>(() => {
				gridManager.IsEmpty(null);
			});
		}

		[Test]
		public void IsEmpty_GivenSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(5, 1);

			Assert.Throws<ArgumentException>(() => {
				gridManager.IsEmpty(nonExistingSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(1, 5);

			Assert.Throws<ArgumentException>(() => {
				gridManager.IsEmpty(nonExistingSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace invalidSpace = new Entities.GridSpace(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.IsEmpty(invalidSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace invalidSpace = new Entities.GridSpace(0, -1);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.IsEmpty(invalidSpace);
			});
		}

		[Test]
		public void Move_GivenTwoSpacesWhereAnElementIsLocatedOnTheFirstSpaceAndTheSecondSpaceIsEmpty_MoveElementFromTheFirstSpaceToTheSecondSpace() {
			Element expected = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = expected;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;

			gridManager.Move(new Entities.GridSpace(0, 0), new Entities.GridSpace(1, 1));
			Element actual = gridManager.Grid[1,1];

			Assert.AreEqual(expected, actual);
		}

        [Test]
        public void Move_GivenTwoSpacesWhereAnElementIsLocatedOnTheFirstSpaceAndTheSecondSpaceIsEmpty_FireAGridMoveSignalWithCorrectArgs() {
            Element expected = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = expected;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
            GridSpace expectedFrom = new Entities.GridSpace(0, 0);
            GridSpace expectedTo = new Entities.GridSpace(1, 1);

            GridSpace actualFrom = null;
            GridSpace actualTo = null;
            Element actualElement = null;
            signalBus.Subscribe<GridMovedSignal>(s => {
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
			Element element1 = new Token(null);
			Element element2 = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = element1;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = element2;
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;

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
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;

			Exception expected = new InvalidOperationException(ExceptionConstants.VA_ELEMENT_DOESNT_EXIST_ON_SPACE);

			Exception actual = Assert.Throws<InvalidOperationException>(() => {
				gridManager.Move(new Entities.GridSpace(0, 0), new Entities.GridSpace(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenFirstSpaceWhereRowDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(5, 1);

			Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "from");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				gridManager.Move(nonExistingSpace, new Entities.GridSpace(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenFirstSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(1, 5);

			Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "from");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				gridManager.Move(nonExistingSpace, new Entities.GridSpace(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenSecondSpaceWhereRowDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(5, 1);

			Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "to");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				gridManager.Move(new Entities.GridSpace(0, 0), nonExistingSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenSecondSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(1, 5);

			Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "to");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				gridManager.Move(new Entities.GridSpace(0, 0), nonExistingSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidFirstSpaceWhereRowIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace invalidSpace = new Entities.GridSpace(-1, 0);

			Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "from"));

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				gridManager.Move(invalidSpace, new Entities.GridSpace(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidFirstSpaceWhereColumnIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace invalidSpace = new Entities.GridSpace(0, -1);

			Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "from"));

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				gridManager.Move(invalidSpace, new Entities.GridSpace(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidSecondSpaceWhereRowIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace invalidSpace = new Entities.GridSpace(-1, 0);

			Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "to"));

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				gridManager.Move(new Entities.GridSpace(0, 0), invalidSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidSecondSpaceWhereColumnIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace invalidSpace = new Entities.GridSpace(0, -1);

			Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "to"));

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				gridManager.Move(new Entities.GridSpace(0, 0), invalidSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenNullFirstSpace_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;

			Exception expected = new ArgumentNullException("from", ExceptionConstants.VA_ARGUMENT_NULL);

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				gridManager.Move(null, new Entities.GridSpace(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenNullSecondSpace_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element element = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;

			Exception expected = new ArgumentNullException("to", ExceptionConstants.VA_ARGUMENT_NULL);

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				gridManager.Move(new Entities.GridSpace(0, 0), null);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void RemoveFrom_GivenSpace_RemoveElementFromThatSpaceAndReturnTheElement() {
			Element expected = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = expected;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;

			Element actual = gridManager.RemoveFrom(new Entities.GridSpace(0, 1));

			Assert.IsNull(gridManager.Grid[0,1]);
			Assert.AreEqual(expected, actual);
		}

        [Test]
        public void RemoveFrom_GivenSpace_FireAGridRemoveSignalWithCorrectArgs() {
            Element expected = new Token(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = expected;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
            GridSpace expectedSpace = new Entities.GridSpace(0, 1);

            GridSpace actualSpace = null;
            Element actualElement = null;
            signalBus.Subscribe<GridRemovedSignal>(s => {
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
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;

			Assert.Throws<InvalidOperationException>(() => {
				gridManager.RemoveFrom(new Entities.GridSpace(0, 1));
			});
		}

		[Test]
		public void RemoveFrom_GivenSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(5, 1);

			Assert.Throws<ArgumentException>(() => {
				gridManager.RemoveFrom(nonExistingSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace nonExistingSpace = new Entities.GridSpace(1, 5);

			Assert.Throws<ArgumentException>(() => {
				gridManager.RemoveFrom(nonExistingSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace invalidSpace = new Entities.GridSpace(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.RemoveFrom(invalidSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;
			Entities.GridSpace invalidSpace = new Entities.GridSpace(0, -1);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.RemoveFrom(invalidSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenNullSpace_ThrowArgumentNullException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(gridDetails, signalBus);
            gridManager.Grid = fakeGrid;

			Assert.Throws<ArgumentNullException>(() => {
				gridManager.RemoveFrom(null);
			});
		}
	}
}
