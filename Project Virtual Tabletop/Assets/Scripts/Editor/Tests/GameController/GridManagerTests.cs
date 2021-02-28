﻿using NUnit.Framework;
using ProjectVirtualTabletop.GameController;
using ProjectVirtualTabletop.Entities;
using System;
using ProjectVirtualTabletop.Exceptions;
using ProjectVirtualTabletop.Constants;

namespace ProjectVirtualTabletop.Editor.Tests.GameController {
	public class GridManagerTests {
		GridManager gridManager;

		[Test]
		public void AddTo_GivenSpaceAndAnElement_SetElementToCorrectLocationOnMap() {
			Element expected = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(fakeGrid);
			Entities.Space space = new Entities.Space(1, 0);

			gridManager.AddTo(space, expected);
			Element actual = gridManager.Grid[1,0];

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void AddTo_GivenSpaceWhereAnElementIsAlreadyLocatedOnIt_ThrowInvalidOperationException() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = element;
			gridManager = new GridManager(fakeGrid);

			Assert.Throws<InvalidOperationException>(() => {
				gridManager.AddTo(new Entities.Space(1, 1), new Element(null));
			});
		}

		[Test]
		public void AddTo_GivenNullSpace_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);

			Exception expected = new ArgumentNullException("space", ExceptionConstants.VA_ARGUMENT_NULL);

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				gridManager.AddTo(null, element);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void AddTo_GivenNullElement_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space space = new Entities.Space(1, 1);

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
			gridManager = new GridManager(fakeGrid);
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Assert.Throws<ArgumentException>(() => {
				gridManager.AddTo(nonExistingSpace, element);
			});
		}

		[Test]
		public void AddTo_GivenSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Assert.Throws<ArgumentException>(() => {
				gridManager.AddTo(nonExistingSpace, element);
			});
		}

		[Test]
		public void AddTo_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.AddTo(invalidSpace, element);
			});
		}

		[Test]
		public void AddTo_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space invalidSpace = new Entities.Space(0, -1);

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
			gridManager = new GridManager(fakeGrid);
			Entities.Space expectedSpace = new Entities.Space(0, 1);

			Element actual = gridManager.GetElementOn(expectedSpace);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetElementOn_GivenNullSpace_ThrowArgumentNullException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);

			Assert.Throws<ArgumentNullException>(() => {
				gridManager.GetElementOn(null);
			});
		}

		[Test]
		public void GetElementOn_GivenSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Assert.Throws<ArgumentException>(() => {
				gridManager.GetElementOn(nonExistingSpace);
			});
		}

		[Test]
		public void GetElementOn_GivenSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Assert.Throws<ArgumentException>(() => {
				gridManager.GetElementOn(nonExistingSpace);
			});
		}

		[Test]
		public void GetElementOn_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.GetElementOn(invalidSpace);
			});
		}

		[Test]
		public void GetElementOn_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space invalidSpace = new Entities.Space(0, -1);

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
			gridManager = new GridManager(fakeGrid);
			Entities.Space expectedSpace = new Entities.Space(1, 1);

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
			gridManager = new GridManager(fakeGrid);
			Entities.Space expectedSpace = new Entities.Space(0, 0);

			bool result = gridManager.IsEmpty(expectedSpace);

			Assert.False(result);
		}

		[Test]
		public void IsEmpty_GivenNullSpace_ThrowArgumentNullException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);

			Assert.Throws<ArgumentNullException>(() => {
				gridManager.IsEmpty(null);
			});
		}

		[Test]
		public void IsEmpty_GivenSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Assert.Throws<ArgumentException>(() => {
				gridManager.IsEmpty(nonExistingSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Assert.Throws<ArgumentException>(() => {
				gridManager.IsEmpty(nonExistingSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.IsEmpty(invalidSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space invalidSpace = new Entities.Space(0, -1);

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
			gridManager = new GridManager(fakeGrid);

			gridManager.Move(new Entities.Space(0, 0), new Entities.Space(1, 1));
			Element actual = gridManager.Grid[1,1];

			Assert.AreEqual(expected, actual);
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
			gridManager = new GridManager(fakeGrid);

			Exception expected = new InvalidOperationException(ExceptionConstants.VA_ELEMENT_EXISTS_ON_SPACE);

			Exception actual = Assert.Throws<InvalidOperationException>(() => {
				gridManager.Move(new Entities.Space(0, 0), new Entities.Space(1, 1));
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
			gridManager = new GridManager(fakeGrid);

			Exception expected = new InvalidOperationException(ExceptionConstants.VA_ELEMENT_DOESNT_EXIST_ON_SPACE);

			Exception actual = Assert.Throws<InvalidOperationException>(() => {
				gridManager.Move(new Entities.Space(0, 0), new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenFirstSpaceWhereRowDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "from");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				gridManager.Move(nonExistingSpace, new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenFirstSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "from");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				gridManager.Move(nonExistingSpace, new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenSecondSpaceWhereRowDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "to");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				gridManager.Move(new Entities.Space(0, 0), nonExistingSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenSecondSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "to");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				gridManager.Move(new Entities.Space(0, 0), nonExistingSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidFirstSpaceWhereRowIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "from"));

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				gridManager.Move(invalidSpace, new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidFirstSpaceWhereColumnIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space invalidSpace = new Entities.Space(0, -1);

			Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "from"));

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				gridManager.Move(invalidSpace, new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidSecondSpaceWhereRowIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "to"));

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				gridManager.Move(new Entities.Space(0, 0), invalidSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidSecondSpaceWhereColumnIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space invalidSpace = new Entities.Space(0, -1);

			Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "to"));

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				gridManager.Move(new Entities.Space(0, 0), invalidSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenNullFirstSpace_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);

			Exception expected = new ArgumentNullException("from", ExceptionConstants.VA_ARGUMENT_NULL);

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				gridManager.Move(null, new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenNullSecondSpace_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element element = new Element(null);
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);

			Exception expected = new ArgumentNullException("to", ExceptionConstants.VA_ARGUMENT_NULL);

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				gridManager.Move(new Entities.Space(0, 0), null);
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
			gridManager = new GridManager(fakeGrid);

			Element actual = gridManager.RemoveFrom(new Entities.Space(0, 1));

			Assert.IsNull(gridManager.Grid[0,1]);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void RemoveFrom_GivenSpaceWhereAnElementDoesNotExistOnThatSpace_ThrowInvalidOperationException() {
			Element[,] fakeGrid = new Element[2,2];
			fakeGrid[0,0] = null;
			fakeGrid[0,1] = null;
			fakeGrid[1,0] = null;
			fakeGrid[1,1] = null;
			gridManager = new GridManager(fakeGrid);

			Assert.Throws<InvalidOperationException>(() => {
				gridManager.RemoveFrom(new Entities.Space(0, 1));
			});
		}

		[Test]
		public void RemoveFrom_GivenSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Assert.Throws<ArgumentException>(() => {
				gridManager.RemoveFrom(nonExistingSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Assert.Throws<ArgumentException>(() => {
				gridManager.RemoveFrom(nonExistingSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.RemoveFrom(invalidSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);
			Entities.Space invalidSpace = new Entities.Space(0, -1);

			Assert.Throws<InvalidSpaceException>(() => {
				gridManager.RemoveFrom(invalidSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenNullSpace_ThrowArgumentNullException() {
			Element[,] fakeGrid = new Element[2,2];
			gridManager = new GridManager(fakeGrid);

			Assert.Throws<ArgumentNullException>(() => {
				gridManager.RemoveFrom(null);
			});
		}
	}
}