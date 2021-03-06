using System.Collections;
using System.Collections.Generic;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities.Components;
using NaughtyBikerGames.ProjectVirtualTabletop.GridManagement.Interfaces;
using NaughtyBikerGames.SDK.Editor.UnityTests;
using NaughtyBikerGames.SDK.Interpolation.Interfaces;
using NaughtyBikerGames.SDK.Raycast.Interfaces;
using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using UnityEngine.TestTools;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using ModestTree.Util;
using System;
using NSubstitute.ReturnsExtensions;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.UnityTests.Entities.Components {
	public class MoveTokenTests : MonobehaviourTests {
		GameObject tileObject1;
		GameObject tileObject2;
        GameObject tokenObject;
		Token token;
		MoveToken moveToken;

		IGridManager gridManager;
		ILerpable lerpable;
		IRaycastable raycastable;

		[SetUp]
		public void SetUp() {
			tileObject1 = new GameObject();
			tileObject1.transform.position = new Vector3(1, 0, 1);
			GridSpaceMono gridSpace1 = tileObject1.AddComponent<GridSpaceMono>();
			gridSpace1.Row = 0;
			gridSpace1.Col = 0;

			tileObject2 = new GameObject();
			tileObject2.transform.position = new Vector3(2, 0, 2);
			GridSpaceMono gridSpace2 = tileObject2.AddComponent<GridSpaceMono>();
			gridSpace2.Row = 1;
			gridSpace2.Col = 1;

			token = new Token(gridSpace1.Space);

			tokenObject = new GameObject();
			tokenObject.transform.position = new Vector3(1, 0, 1);
			moveToken = tokenObject.AddComponent<MoveToken>();
			moveToken.CurrentSpace = gridSpace1;
			moveToken.MaxHeight = 1;
			moveToken.LerpDuration = 0f;

			gridManager = Substitute.For<IGridManager>();
			lerpable = Substitute.For<ILerpable>();
			raycastable = Substitute.For<IRaycastable>();

			moveToken.Construct(gridManager, lerpable, raycastable);

			gridManager.GetElementOn(gridSpace1.Space).Returns(token);
		}

		[UnityTest]
		public IEnumerator Start_InitializesTokenAndInitialPositionToCorrectValues() {
			yield return null;

			Assert.AreEqual(token, moveToken.Token);
			Assert.AreEqual(tokenObject.transform.position, moveToken.InitialPosition);
		}

		[UnityTest]
		public IEnumerator OnMouseDown_WhenTriggered_LerpableIsCalledOnceWithCorrectArguments() {
			Vector3 expectedSource = tokenObject.transform.position;
			Vector3 expectedDestination = new Vector3(expectedSource.x, 1, expectedSource.z);
			float expectedLerpDuration = 0f;

			yield return null;

			moveToken.OnMouseDown();

			yield return null;

			lerpable.Received().Lerp(expectedSource, expectedDestination, expectedLerpDuration, Arg.Any<Action<Vector3>>());
		}

		[UnityTest]
		public IEnumerator OnMouseDrag_WhenTriggered_RaycastableIsCalledOnceWithCorrectArgument() {
			string expectedTag = "Tile";

			yield return null;

			moveToken.OnMouseDrag();

			yield return null;

			raycastable.Received().CastRayForTag(expectedTag);
		}

		[UnityTest]
		public IEnumerator OnMouseDrag_CastRayForTagReturnsNullGameObject_DontCallLerp() {
			raycastable.CastRayForTag("Tile").ReturnsNull();
			
			yield return null;

			moveToken.OnMouseDrag();

			yield return null;

			lerpable.DidNotReceive().Lerp(Arg.Any<Vector3>(), Arg.Any<Vector3>(), Arg.Any<float>(), Arg.Any<Action<Vector3>>());
		}

		[UnityTest]
		public IEnumerator OnMouseDrag_CastRayForTagReturnsATileAndTheTokenIsAlreadyLocatedAtThatTile_DontCallLerp() {
			raycastable.CastRayForTag("Tile").Returns(tileObject1);

			yield return null;

			moveToken.OnMouseDrag();

			yield return null;

			lerpable.DidNotReceive().Lerp(Arg.Any<Vector3>(), Arg.Any<Vector3>(), Arg.Any<float>(), Arg.Any<Action<Vector3>>());
		}

		[UnityTest]
		public IEnumerator OnMouseDrag_CastRayForTagReturnsATileAndTheTokenIsNotLocatedAtThatTile_CallLerpToMoveToNewTile() {
			Vector3 expectedSource = tokenObject.transform.position;
			Vector3 expectedDestination = new Vector3(tileObject2.transform.position.x, expectedSource.y, tileObject2.transform.position.z);
			float expectedLerpDuration = 0f;
			
			raycastable.CastRayForTag("Tile").Returns(tileObject2);

			yield return null;

			moveToken.OnMouseDrag();

			yield return null;

			lerpable.Received().Lerp(expectedSource, expectedDestination, expectedLerpDuration, Arg.Any<Action<Vector3>>());
		}

		[UnityTest]
		public IEnumerator OnMouseUp_WhenTriggered_RaycastableIsCalledOnceWithCorrectArgument() {
			string expectedTag = "Tile";

			yield return null;

			moveToken.OnMouseUp();

			yield return null;

			raycastable.Received().CastRayForTag(expectedTag);
		}

		[UnityTest]
		public IEnumerator OnMouseUp_GivenTokenWasMovedAndCastRayForTagReturnedNullGameObject_DontCallLerpAndTheTokenPositionSnapsBackToTheInitialPosition() {
			tokenObject.transform.position = new Vector3(2, 1, 2);
			Vector3 expected = new Vector3(1, 0, 1);

			raycastable.CastRayForTag("Tile").ReturnsNull();

			yield return null;

			moveToken.OnMouseUp();

			yield return null;

			lerpable.DidNotReceive().Lerp(Arg.Any<Vector3>(), Arg.Any<Vector3>(), Arg.Any<float>(), Arg.Any<Action<Vector3>>());
			Assert.AreEqual(expected, tokenObject.transform.position);
		}

		[UnityTest]
		public IEnumerator OnMouseUp_GivenTokenWasMovedAndCastRayForTagReturnedTileThatIsNotEmpty_DontCallLerpAndTheTokenPositionSnapsBackToTheInitialPosition() {
			tokenObject.transform.position = new Vector3(2, 1, 2);
			Vector3 expected = new Vector3(1, 0, 1);

			raycastable.CastRayForTag("Tile").Returns(tileObject1);
			gridManager.IsEmpty(tileObject1.GetComponent<GridSpaceMono>().Space).Returns(false);

			yield return null;

			moveToken.OnMouseUp();

			yield return null;

			lerpable.DidNotReceive().Lerp(Arg.Any<Vector3>(), Arg.Any<Vector3>(), Arg.Any<float>(), Arg.Any<Action<Vector3>>());
			Assert.AreEqual(expected, tokenObject.transform.position);
		}

		[UnityTest]
		public IEnumerator OnMouseUp_GivenTokenWasMovedAndCastRayForTagReturnedTileThatIsEmpty_MoveTheTokenToTheNewSpaceAndCallLerpOnTheCorrectArguments() {
			tokenObject.transform.position = new Vector3(2, 1, 2);
			Vector3 expectedSource = tokenObject.transform.position;
			Vector3 expectedDestination = new Vector3(expectedSource.x, 0, expectedSource.z);
			float expectedLerpDuration = 0f;

			raycastable.CastRayForTag("Tile").Returns(tileObject2);
			gridManager.IsEmpty(tileObject2.GetComponent<GridSpaceMono>().Space).Returns(true);

			yield return null;

			moveToken.OnMouseUp();

			yield return null;

			gridManager.Received().Move(tileObject1.GetComponent<GridSpaceMono>().Space, tileObject2.GetComponent<GridSpaceMono>().Space);
			Assert.AreEqual(tileObject2.GetComponent<GridSpaceMono>().Space, token.CurrentSpace);
			lerpable.Received().Lerp(expectedSource, expectedDestination, expectedLerpDuration, Arg.Any<Action<Vector3>>());
		}
	}
}
