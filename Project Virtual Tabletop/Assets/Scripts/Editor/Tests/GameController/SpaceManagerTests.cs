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

namespace ProjectVirtualTabletop.Editor.Tests.GameController {
	public class SpaceManagerTests : ZenjectTests {
		SpaceManager spaceManager;

		[SetUp]
		public void SetUp() {
			SpaceManagerBaseInstaller.Install(Container);
			spaceManager = Container.Resolve<ISpaceManager>() as SpaceManager;
		}

		[Test]
		public void AddTo_GivenValidSpaceThatExistsOnMapAndAnElement_SetElementToCorrectLocationOnMap() {
			Element expected = new Element();
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = null;
			fakeMap[0,1] = null;
			fakeMap[1,0] = null;
			fakeMap[1,1] = null;
			spaceManager.Map = fakeMap;
			Entities.Space space = new Entities.Space(1, 0);

			spaceManager.AddTo(space, expected);
			Element actual = spaceManager.Map[1,0];

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void AddTo_GivenNullSpace_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;

			Exception expected = new ArgumentNullException("space", "A required argument was null");

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				spaceManager.AddTo(null, element);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void AddTo_GivenNullElement_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space space = new Entities.Space(1, 1);

			Exception expected = new ArgumentNullException("element", "A required argument was null");

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				spaceManager.AddTo(space, null);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void AddTo_GivenValidSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Assert.Throws<ArgumentException>(() => {
				spaceManager.AddTo(nonExistingSpace, element);
			});
		}

		[Test]
		public void AddTo_GivenValidSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Assert.Throws<ArgumentException>(() => {
				spaceManager.AddTo(nonExistingSpace, element);
			});
		}

		[Test]
		public void AddTo_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.AddTo(invalidSpace, element);
			});
		}

		[Test]
		public void AddTo_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space invalidSpace = new Entities.Space(0, -1);

			Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.AddTo(invalidSpace, element);
			});
		}

		[Test]
		public void GetElementOn_GivenValidSpaceThatExistsOnMap_ReturnCorrectElement() {
			Element expected = new Element();
			Element notExpected = new Element();
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = null;
			fakeMap[0,1] = expected;
			fakeMap[1,0] = notExpected;
			fakeMap[1,1] = null;
			spaceManager.Map = fakeMap;
			Entities.Space expectedSpace = new Entities.Space(0, 1);

			Element actual = spaceManager.GetElementOn(expectedSpace);

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetElementOn_GivenNullSpace_ThrowArgumentNullException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;

			Assert.Throws<ArgumentNullException>(() => {
				spaceManager.GetElementOn(null);
			});
		}

		[Test]
		public void GetElementOn_GivenValidSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Assert.Throws<ArgumentException>(() => {
				spaceManager.GetElementOn(nonExistingSpace);
			});
		}

		[Test]
		public void GetElementOn_GivenValidSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Assert.Throws<ArgumentException>(() => {
				spaceManager.GetElementOn(nonExistingSpace);
			});
		}

		[Test]
		public void GetElementOn_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.GetElementOn(invalidSpace);
			});
		}

		[Test]
		public void GetElementOn_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space invalidSpace = new Entities.Space(0, -1);

			Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.GetElementOn(invalidSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenValidSpaceThatExistsOnMapAndSpaceIsEmpty_ReturnTrue() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = element;
			fakeMap[0,1] = null;
			fakeMap[1,0] = null;
			fakeMap[1,1] = null;
			spaceManager.Map = fakeMap;
			Entities.Space expectedSpace = new Entities.Space(1, 1);

			bool result = spaceManager.IsEmpty(expectedSpace);

			Assert.True(result);
		}

		[Test]
		public void IsEmpty_GivenValidSpaceThatExistsOnMapAndSpaceIsNotEmpty_ReturnFalse() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = element;
			fakeMap[0,1] = null;
			fakeMap[1,0] = null;
			fakeMap[1,1] = null;
			spaceManager.Map = fakeMap;
			Entities.Space expectedSpace = new Entities.Space(0, 0);

			bool result = spaceManager.IsEmpty(expectedSpace);

			Assert.False(result);
		}

		[Test]
		public void IsEmpty_GivenNullSpace_ThrowArgumentNullException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;

			Assert.Throws<ArgumentNullException>(() => {
				spaceManager.IsEmpty(null);
			});
		}

		[Test]
		public void IsEmpty_GivenValidSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Assert.Throws<ArgumentException>(() => {
				spaceManager.IsEmpty(nonExistingSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenValidSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Assert.Throws<ArgumentException>(() => {
				spaceManager.IsEmpty(nonExistingSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.IsEmpty(invalidSpace);
			});
		}

		[Test]
		public void IsEmpty_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space invalidSpace = new Entities.Space(0, -1);

			Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.IsEmpty(invalidSpace);
			});
		}

		[Test]
		public void Move_GivenTwoValidSpacesWhereAnElementIsLocatedOnTheFirstSpaceAndTheSecondSpaceIsEmpty_MoveElementFromTheFirstSpaceToTheSecondSpace() {
			Element expected = new Element();
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = expected;
			fakeMap[0,1] = null;
			fakeMap[1,0] = null;
			fakeMap[1,1] = null;
			spaceManager.Map = fakeMap;

			spaceManager.Move(new Entities.Space(0, 0), new Entities.Space(1, 1));
			Element actual = spaceManager.Map[1,1];

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Move_GivenTwoValidSpacesWhereAnElementIsLocatedOnTheSecondSpace_ThrowInvalidOperationExceptionWithCorrectMessage() {
			Element element1 = new Element();
			Element element2 = new Element();
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = element1;
			fakeMap[0,1] = null;
			fakeMap[1,0] = null;
			fakeMap[1,1] = element2;
			spaceManager.Map = fakeMap;

			Exception expected = new InvalidOperationException("The space to move to is not empty. A space must be unoccupied in order to add an element to it. " +
				"Please remove the existing element from this space first before adding another element.");

			Exception actual = Assert.Throws<InvalidOperationException>(() => {
				spaceManager.Move(new Entities.Space(0, 0), new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenTwoValidSpacesWhereTheFirstSpaceIsEmpty_ThrowInvalidOperationExceptionWithCorrectMessage() {
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = null;
			fakeMap[0,1] = null;
			fakeMap[1,0] = null;
			fakeMap[1,1] = null;
			spaceManager.Map = fakeMap;

			Exception expected = new InvalidOperationException("The space to move from is empty. An element must exist on the space in order to move it");

			Exception actual = Assert.Throws<InvalidOperationException>(() => {
				spaceManager.Move(new Entities.Space(0, 0), new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenValidFromSpaceWhereRowDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Exception expected = new ArgumentException("The space to move from does not exist on map", "from");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				spaceManager.Move(nonExistingSpace, new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenValidFromSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Exception expected = new ArgumentException("The space to move from does not exist on map", "from");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				spaceManager.Move(nonExistingSpace, new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenValidToSpaceWhereRowDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Exception expected = new ArgumentException("The space to move to does not exist on map", "to");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				spaceManager.Move(new Entities.Space(0, 0), nonExistingSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenValidToSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Exception expected = new ArgumentException("The space to move to does not exist on map", "to");

			Exception actual = Assert.Throws<ArgumentException>(() => {
				spaceManager.Move(new Entities.Space(0, 0), nonExistingSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidFromSpaceWhereRowIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Exception expected = new InvalidSpaceException("The space to move from is invalid. Please verify neither the row or column " +
				"are negative and try again.");

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.Move(invalidSpace, new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidFromSpaceWhereColumnIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space invalidSpace = new Entities.Space(0, -1);

			Exception expected = new InvalidSpaceException("The space to move from is invalid. Please verify neither the row or column " +
				"are negative and try again.");

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.Move(invalidSpace, new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidToSpaceWhereRowIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Exception expected = new InvalidSpaceException("The space to move to is invalid. Please verify neither the row or column " +
				"are negative and try again.");

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.Move(new Entities.Space(0, 0), invalidSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenInvalidToSpaceWhereColumnIsNegative_ThrowInvalidSpaceExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space invalidSpace = new Entities.Space(0, -1);

			Exception expected = new InvalidSpaceException("The space to move to is invalid. Please verify neither the row or column " +
				"are negative and try again.");

			Exception actual = Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.Move(new Entities.Space(0, 0), invalidSpace);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenNullFromSpace_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;

			Exception expected = new ArgumentNullException("from", "A required argument was null");

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				spaceManager.Move(null, new Entities.Space(1, 1));
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void Move_GivenNullToSpace_ThrowArgumentNullExceptionWithCorrectMessage() {
			Element element = new Element();
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;

			Exception expected = new ArgumentNullException("to", "A required argument was null");

			Exception actual = Assert.Throws<ArgumentNullException>(() => {
				spaceManager.Move(new Entities.Space(0, 0), null);
			});
			Assert.AreEqual(expected.Message, actual.Message);
		}

		[Test]
		public void RemoveFrom_GivenValidSpaceWhereAnElementExistsOnThatSpace_RemoveElementFromThatSpaceAndReturnTheElement() {
			Element expected = new Element();
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = null;
			fakeMap[0,1] = expected;
			fakeMap[1,0] = null;
			fakeMap[1,1] = null;
			spaceManager.Map = fakeMap;

			Element actual = spaceManager.RemoveFrom(new Entities.Space(0, 1));

			Assert.IsNull(spaceManager.Map[0,1]);
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void RemoveFrom_GivenValidSpaceWhereAnElementDoesNotExistOnThatSpace_ThrowInvalidOperationException() {
			Element[,] fakeMap = new Element[2,2];
			fakeMap[0,0] = null;
			fakeMap[0,1] = null;
			fakeMap[1,0] = null;
			fakeMap[1,1] = null;
			spaceManager.Map = fakeMap;

			Assert.Throws<InvalidOperationException>(() => {
				spaceManager.RemoveFrom(new Entities.Space(0, 1));
			});
		}

		[Test]
		public void RemoveFrom_GivenValidSpaceWhereRowDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space nonExistingSpace = new Entities.Space(5, 1);

			Assert.Throws<ArgumentException>(() => {
				spaceManager.RemoveFrom(nonExistingSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenValidSpaceWhereColumnDoesNotExistOnMap_ThrowArgumentException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space nonExistingSpace = new Entities.Space(1, 5);

			Assert.Throws<ArgumentException>(() => {
				spaceManager.RemoveFrom(nonExistingSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenInvalidSpaceWhereRowIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space invalidSpace = new Entities.Space(-1, 0);

			Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.RemoveFrom(invalidSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenInvalidSpaceWhereColumnIsNegative_ThrowInvalidSpaceException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;
			Entities.Space invalidSpace = new Entities.Space(0, -1);

			Assert.Throws<InvalidSpaceException>(() => {
				spaceManager.RemoveFrom(invalidSpace);
			});
		}

		[Test]
		public void RemoveFrom_GivenNullSpace_ThrowArgumentNullException() {
			Element[,] fakeMap = new Element[2,2];
			spaceManager.Map = fakeMap;

			Assert.Throws<ArgumentNullException>(() => {
				spaceManager.RemoveFrom(null);
			});
		}
	}
}
