using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NaughtyBiker.Editor.Tests;
using ProjectVirtualTabletop.GameController;
using ProjectVirtualTabletop.GameController.Interfaces;
using ProjectVirtualTabletop.GameController.Installers;
using ProjectVirtualTabletop.Entities;
using System;
using ProjectVirtualTabletop.Exceptions;
using ProjectVirtualTabletop.Constants;

namespace ProjectVirtualTabletop.Editor.Tests.GameController {
	public class SpaceManagerTests {
		SpaceManager spaceManager;

		[Test]
		public void AddTo_GivenSpaceAndAnElement_SetElementToCorrectLocationOnMap() {
			Element expected = new Element();
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = null;
			fakeMap[0,1] = null;
			fakeMap[1,0] = null;
			fakeMap[1,1] = null;
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space space = new Entities.Space(1, 0);

			spaceManager.AddTo(space, expected);
			Element actual = spaceManager.Map[1,0];

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void AddTo_GivenSpaceWhereAnElementIsAlreadyLocatedOnIt_ThrowInvalidOperationException() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = null;
			fakeMap[0,1] = null;
			fakeMap[1,0] = null;
			fakeMap[1,1] = element;
			spaceManager = new SpaceManager(fakeMap);

			Assert.Throws<InvalidOperationException>(() => {
				spaceManager.AddTo(new Entities.Space(1, 1), new Element());
			});
		}

		[Test]
		public void AddTo_GivenNullSpace_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);

			Exception expected = new ArgumentNullException("space", ExceptionConstants.VA_ARGUMENT_NULL);

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				spaceManager.AddTo(null, element);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void AddTo_GivenNullElement_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space space = new Entities.Space(1, 1);

			Exception expected = new ArgumentNullException("element", ExceptionConstants.VA_ARGUMENT_NULL);

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				spaceManager.AddTo(space, null);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void AddTo_GivenSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Assert.Throws<ArgumentException>(() => {
				spaceManager.AddTo(nonExistingSpace, element);
			});
		}

		[Test]
		public void AddTo_GivenSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Assert.Throws<ArgumentException>(() => {
				spaceManager.AddTo(nonExistingSpace, element);
			});
		}

		[Test]
		public void AddTo_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.AddTo(invalidSpace, element);
			});
		}

		[Test]
		public void AddTo_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space invalidSpace = new Entities.Space(0, -1);

			Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.AddTo(invalidSpace, element);
			});
		}

		[Test]
		public void GetElementOn_GivenSpace_ReturnCorrectElement() {
			Element expected = new Element();
			Element notExpected = new Element();
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = null;
			fakeMap[0,1] = expected;
			fakeMap[1,0] = notExpected;
			fakeMap[1,1] = null;
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space expectedSpace = new Entities.Space(0, 1);

			Element actual = spaceManager.GetElementOn(expectedSpace);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetElementOn_GivenNullSpace_ThrowArgumentNullException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);

			Assert.Throws<ArgumentNullException>(() => {
				spaceManager.GetElementOn(null);
			});
		}

		[Test]
		public void GetElementOn_GivenSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Assert.Throws<ArgumentException>(() => {
				spaceManager.GetElementOn(nonExistingSpace);
			});
		}

		[Test]
		public void GetElementOn_GivenSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Assert.Throws<ArgumentException>(() => {
				spaceManager.GetElementOn(nonExistingSpace);
			});
		}

		[Test]
		public void GetElementOn_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.GetElementOn(invalidSpace);
			});
		}

		[Test]
		public void GetElementOn_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space invalidSpace = new Entities.Space(0, -1);

			Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.GetElementOn(invalidSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenSpaceThatExistsOnMapAndSpaceIsEmpty_ReturnTrue() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = element;
			fakeMap[0,1] = null;
			fakeMap[1,0] = null;
			fakeMap[1,1] = null;
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space expectedSpace = new Entities.Space(1, 1);

			bool result = spaceManager.IsEmpty(expectedSpace);

			Assert.True(result);
		}

		[Test]
		public void IsEmpty_GivenSpaceThatExistsOnMapAndSpaceIsNotEmpty_ReturnFalse() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = element;
			fakeMap[0,1] = null;
			fakeMap[1,0] = null;
			fakeMap[1,1] = null;
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space expectedSpace = new Entities.Space(0, 0);

			bool result = spaceManager.IsEmpty(expectedSpace);

			Assert.False(result);
		}

		[Test]
		public void IsEmpty_GivenNullSpace_ThrowArgumentNullException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);

			Assert.Throws<ArgumentNullException>(() => {
				spaceManager.IsEmpty(null);
			});
		}

		[Test]
		public void IsEmpty_GivenSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Assert.Throws<ArgumentException>(() => {
				spaceManager.IsEmpty(nonExistingSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Assert.Throws<ArgumentException>(() => {
				spaceManager.IsEmpty(nonExistingSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.IsEmpty(invalidSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space invalidSpace = new Entities.Space(0, -1);

			Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.IsEmpty(invalidSpace);
			});
		}

		[Test]
		public void Move_GivenTwoSpacesWhereAnElementIsLocatedOnTheFirstSpaceAndTheSecondSpaceIsEmpty_MoveElementFromTheFirstSpaceToTheSecondSpace() {
			Element expected = new Element();
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = expected;
			fakeMap[0,1] = null;
			fakeMap[1,0] = null;
			fakeMap[1,1] = null;
			spaceManager = new SpaceManager(fakeMap);

			spaceManager.Move(new Entities.Space(0, 0), new Entities.Space(1, 1));
			Element actual = spaceManager.Map[1,1];

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Move_GivenTwoSpacesWhereAnElementIsAlreadyLocatedOnTheSecondSpace_ThrowInvalidOperationExceptionWithCorrectMessage() {
			Element element1 = new Element();
			Element element2 = new Element();
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = element1;
			fakeMap[0,1] = null;
			fakeMap[1,0] = null;
			fakeMap[1,1] = element2;
			spaceManager = new SpaceManager(fakeMap);

			Exception expected = new InvalidOperationException(ExceptionConstants.VA_ELEMENT_EXISTS_ON_SPACE);

			Exception actual = Assert.Throws<InvalidOperationException>(() => {
				spaceManager.Move(new Entities.Space(0, 0), new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenTwoSpacesWhereTheFirstSpaceIsEmpty_ThrowInvalidOperationExceptionWithCorrectMessage() {
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = null;
			fakeMap[0,1] = null;
			fakeMap[1,0] = null;
			fakeMap[1,1] = null;
			spaceManager = new SpaceManager(fakeMap);

			Exception expected = new InvalidOperationException(ExceptionConstants.VA_ELEMENT_DOESNT_EXIST_ON_SPACE);

			Exception actual = Assert.Throws<InvalidOperationException>(() => {
				spaceManager.Move(new Entities.Space(0, 0), new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenFirstSpaceWhereRowDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "from");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				spaceManager.Move(nonExistingSpace, new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenFirstSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "from");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				spaceManager.Move(nonExistingSpace, new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenSecondSpaceWhereRowDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "to");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				spaceManager.Move(new Entities.Space(0, 0), nonExistingSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenSecondSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Exception expected = new ArgumentException(ExceptionConstants.VA_SPACE_OUT_OF_BOUNDS, "to");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				spaceManager.Move(new Entities.Space(0, 0), nonExistingSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidFirstSpaceWhereRowIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "from"));

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.Move(invalidSpace, new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidFirstSpaceWhereColumnIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space invalidSpace = new Entities.Space(0, -1);

			Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "from"));

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.Move(invalidSpace, new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidSecondSpaceWhereRowIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "to"));

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.Move(new Entities.Space(0, 0), invalidSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidSecondSpaceWhereColumnIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space invalidSpace = new Entities.Space(0, -1);

			Exception expected = new InvalidSpaceException(string.Format(ExceptionConstants.VA_SPACE_INVALID, "to"));

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.Move(new Entities.Space(0, 0), invalidSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenNullFirstSpace_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);

			Exception expected = new ArgumentNullException("from", ExceptionConstants.VA_ARGUMENT_NULL);

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				spaceManager.Move(null, new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenNullSecondSpace_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);

			Exception expected = new ArgumentNullException("to", ExceptionConstants.VA_ARGUMENT_NULL);

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				spaceManager.Move(new Entities.Space(0, 0), null);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void RemoveFrom_GivenSpace_RemoveElementFromThatSpaceAndReturnTheElement() {
			Element expected = new Element();
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = null;
			fakeMap[0,1] = expected;
			fakeMap[1,0] = null;
			fakeMap[1,1] = null;
			spaceManager = new SpaceManager(fakeMap);

			Element actual = spaceManager.RemoveFrom(new Entities.Space(0, 1));

			Assert.IsNull(spaceManager.Map[0,1]);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void RemoveFrom_GivenSpaceWhereAnElementDoesNotExistOnThatSpace_ThrowInvalidOperationException() {
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = null;
			fakeMap[0,1] = null;
			fakeMap[1,0] = null;
			fakeMap[1,1] = null;
			spaceManager = new SpaceManager(fakeMap);

			Assert.Throws<InvalidOperationException>(() => {
				spaceManager.RemoveFrom(new Entities.Space(0, 1));
			});
		}

		[Test]
		public void RemoveFrom_GivenSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Assert.Throws<ArgumentException>(() => {
				spaceManager.RemoveFrom(nonExistingSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Assert.Throws<ArgumentException>(() => {
				spaceManager.RemoveFrom(nonExistingSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.RemoveFrom(invalidSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);
			Entities.Space invalidSpace = new Entities.Space(0, -1);

			Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.RemoveFrom(invalidSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenNullSpace_ThrowArgumentNullException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager = new SpaceManager(fakeMap);

			Assert.Throws<ArgumentNullException>(() => {
				spaceManager.RemoveFrom(null);
			});
		}
	}
}
