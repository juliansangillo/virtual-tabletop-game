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
	}
}
