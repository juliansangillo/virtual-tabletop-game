using System;
using System.Collections;
using NUnit.Framework;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using UnityEngine;
using UnityEngine.TestTools;
using NaughtyBikerGames.SDK.Editor.UnityTests;
using NaughtyBikerGames.SDK.Interpolation.Interfaces;
using NaughtyBikerGames.SDK.Raycast.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities;
using NaughtyBikerGames.ProjectVirtualTabletop.Constants;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities.Components;
using NaughtyBikerGames.ProjectVirtualTabletop.GridManagement.Interfaces;
using NaughtyBikerGames.ProjectVirtualTabletop.Entities.Components.Interfaces;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.UnityTests.Entities.Components {
	public class MoveTokenTests : MonobehaviourTests {
		GameObject tileObject1;
		GameObject tileObject2;
        GameObject tokenObject;
		Token token;
		ISelectEffect selectEffect1;
		ISelectEffect selectEffect2;
		MoveToken moveToken;

		IGridManager gridManager;
		ILerpable lerpable;
		IRaycastable raycastable;

		[SetUp]
		public void SetUp() {
			gridManager = Substitute.For<IGridManager>();
			lerpable = Substitute.For<ILerpable>();
			raycastable = Substitute.For<IRaycastable>();
			selectEffect1 = Substitute.For<ISelectEffect>();
			selectEffect2 = Substitute.For<ISelectEffect>();

			tileObject1 = new GameObject();
			tileObject1.transform.position = new Vector3(1, 0, 1);
			GridSpaceMono gridSpace1 = tileObject1.AddComponent<GridSpaceMono>();
			gridSpace1.Row = 0;
			gridSpace1.Col = 0;
			gridSpace1.SelectEffect = selectEffect1;

			tileObject2 = new GameObject();
			tileObject2.transform.position = new Vector3(2, 0, 2);
			GridSpaceMono gridSpace2 = tileObject2.AddComponent<GridSpaceMono>();
			gridSpace2.Row = 1;
			gridSpace2.Col = 1;
			gridSpace2.SelectEffect = selectEffect2;

			token = new Token(gridSpace1.Space);

			tokenObject = new GameObject();
			tokenObject.transform.position = new Vector3(1, 0, 1);
			moveToken = tokenObject.AddComponent<MoveToken>();
			moveToken.CurrentSpace = gridSpace1;
			moveToken.MaxHeight = 1;
			moveToken.LerpDuration = 0f;

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
		public IEnumerator Start_InitializesInitialSelectEffectToCorrectValue() {
			yield return null;

			Assert.AreEqual(selectEffect1, moveToken.InitialSelectEffect);
		}

		[UnityTest]
		public IEnumerator Start_InitializesCurrentSelectEffectToCorrectValue() {
			yield return null;

			Assert.AreEqual(selectEffect1, moveToken.CurrentSelectEffect);
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
		public IEnumerator OnMouseDown_WhenTriggered_CurrentSelectEffectIsPlayed() {
			yield return null;

			moveToken.OnMouseDown();

			yield return null;

			selectEffect1.Received().Play();
		}

		[UnityTest]
		public IEnumerator OnMouseDrag_WhenTriggered_RaycastableIsCalledOnceWithCorrectArgument() {
			string expectedTag = AppConstants.GRID_SPACE_TAG;

			yield return null;

			moveToken.OnMouseDrag();

			yield return null;

			raycastable.Received().CastRayForTag(expectedTag);
		}

		[UnityTest]
		public IEnumerator OnMouseDrag_CastRayForTagReturnsNullGameObject_DontUpdatePosition() {
			raycastable.CastRayForTag(AppConstants.GRID_SPACE_TAG).ReturnsNull();
			
			yield return null;

			moveToken.OnMouseDrag();

			yield return null;

			Assert.AreEqual(tileObject1.transform.position, tokenObject.transform.position);
		}

		[UnityTest]
		public IEnumerator OnMouseDrag_CastRayForTagReturnsATileAndTheTileIsNotEmpty_DontUpdatePosition() {
			raycastable.CastRayForTag(AppConstants.GRID_SPACE_TAG).Returns(tileObject2);
			gridManager.IsEmpty(tileObject2.GetComponent<GridSpaceMono>().Space).Returns(false);

			yield return null;

			moveToken.OnMouseDrag();

			yield return null;

			Assert.AreEqual(tileObject1.transform.position, tokenObject.transform.position);
		}

		[UnityTest]
		public IEnumerator OnMouseDrag_CastRayForTagReturnsATileAndTheTileIsEmpty_UpdatePositionToNewTile() {
			Vector3 expected = new Vector3(tileObject2.transform.position.x, tokenObject.transform.position.y, tileObject2.transform.position.z);
			
			raycastable.CastRayForTag(AppConstants.GRID_SPACE_TAG).Returns(tileObject2);
			gridManager.IsEmpty(tileObject2.GetComponent<GridSpaceMono>().Space).Returns(true);

			yield return null;

			moveToken.OnMouseDrag();

			yield return null;

			Assert.AreEqual(expected, tokenObject.transform.position);
		}

		[UnityTest]
		public IEnumerator OnMouseDrag_CastRayForTagReturnsATile_CurrentSelectEffectShouldBeUpdated() {
			raycastable.CastRayForTag(AppConstants.GRID_SPACE_TAG).Returns(tileObject2);
			gridManager.IsEmpty(tileObject2.GetComponent<GridSpaceMono>().Space).Returns(true);

			yield return null;

			moveToken.OnMouseDrag();

			yield return null;

			Assert.AreEqual(selectEffect2, moveToken.CurrentSelectEffect);
		}

		[UnityTest]
		public IEnumerator OnMouseDrag_CastRayForTagReturnsATile_StopCurrentSelectEffectAndPlaySelectEffectForNewSpace() {
			raycastable.CastRayForTag(AppConstants.GRID_SPACE_TAG).Returns(tileObject2);
			gridManager.IsEmpty(tileObject2.GetComponent<GridSpaceMono>().Space).Returns(true);
			
			yield return null;

			moveToken.OnMouseDrag();

			yield return null;

			selectEffect1.Received().Stop();
			selectEffect2.Received().Play();
		}

		[UnityTest]
		public IEnumerator OnMouseUp_WhenTriggered_RaycastableIsCalledOnceWithCorrectArgument() {
			string expectedTag = AppConstants.GRID_SPACE_TAG;

			yield return null;

			moveToken.OnMouseUp();

			yield return null;

			raycastable.Received().CastRayForTag(expectedTag);
		}

		[UnityTest]
		public IEnumerator OnMouseUp_GivenTokenWasMovedAndCastRayForTagReturnedNullGameObject_DontCallLerpAndTheTokenPositionSnapsBackToTheInitialPosition() {
			tokenObject.transform.position = new Vector3(2, 1, 2);
			Vector3 expected = new Vector3(1, 0, 1);

			raycastable.CastRayForTag(AppConstants.GRID_SPACE_TAG).ReturnsNull();

			yield return null;

			moveToken.OnMouseUp();

			yield return null;

			lerpable.DidNotReceive().Lerp(Arg.Any<Vector3>(), Arg.Any<Vector3>(), Arg.Any<float>(), Arg.Any<Action<Vector3>>());
			Assert.AreEqual(expected, tokenObject.transform.position);
		}

		[UnityTest]
		public IEnumerator OnMouseUp_GivenTokenWasMovedAndCastRayForTagReturnedNullGameObject_StopTheCurrentSelectEffectAndTheCurrentSelectEffectSnapsBackToTheInitialSelectEffect() {
			tokenObject.transform.position = new Vector3(2, 1, 2);
			moveToken.CurrentSelectEffect = selectEffect2;

			raycastable.CastRayForTag(AppConstants.GRID_SPACE_TAG).ReturnsNull();

			yield return null;

			moveToken.OnMouseUp();

			yield return null;

			selectEffect2.Received().Stop();
			Assert.AreEqual(selectEffect1, moveToken.CurrentSelectEffect);
		}

		[UnityTest]
		public IEnumerator OnMouseUp_GivenTokenWasNotMovedAndCastRayForTagReturnedCurrentTile_DontCallLerpAndTheTokenPositionSnapsBackToTheInitialPosition() {
			tokenObject.transform.position = new Vector3(1, 1, 1);
			Vector3 expected = new Vector3(1, 0, 1);

			raycastable.CastRayForTag(AppConstants.GRID_SPACE_TAG).Returns(tileObject1);

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

			raycastable.CastRayForTag(AppConstants.GRID_SPACE_TAG).Returns(tileObject2);
			gridManager.IsEmpty(tileObject2.GetComponent<GridSpaceMono>().Space).Returns(false);

			yield return null;

			moveToken.OnMouseUp();

			yield return null;

			lerpable.DidNotReceive().Lerp(Arg.Any<Vector3>(), Arg.Any<Vector3>(), Arg.Any<float>(), Arg.Any<Action<Vector3>>());
			Assert.AreEqual(expected, tokenObject.transform.position);
		}

		[UnityTest]
		public IEnumerator OnMouseUp_GivenTokenWasMovedAndCastRayForTagReturnedTileThatIsNotEmpty_StopTheCurrentSelectEffectAndTheCurrentSelectEffectSnapsBackToTheInitialSelectEffect() {
			tokenObject.transform.position = new Vector3(2, 1, 2);
			moveToken.CurrentSelectEffect = selectEffect2;

			raycastable.CastRayForTag(AppConstants.GRID_SPACE_TAG).Returns(tileObject2);
			gridManager.IsEmpty(tileObject2.GetComponent<GridSpaceMono>().Space).Returns(false);
			
			yield return null;

			moveToken.OnMouseUp();

			yield return null;

			selectEffect2.Received().Stop();
			Assert.AreEqual(selectEffect1, moveToken.CurrentSelectEffect);
		}

		[UnityTest]
		public IEnumerator OnMouseUp_GivenTokenWasMovedAndCastRayForTagReturnedTileThatIsEmpty_MoveTheTokenToTheNewSpaceAndCallLerpOnTheCorrectArguments() {
			tokenObject.transform.position = new Vector3(2, 1, 2);
			Vector3 expectedSource = tokenObject.transform.position;
			Vector3 expectedDestination = new Vector3(expectedSource.x, 0, expectedSource.z);
			float expectedLerpDuration = 0f;

			raycastable.CastRayForTag(AppConstants.GRID_SPACE_TAG).Returns(tileObject2);
			gridManager.IsEmpty(tileObject2.GetComponent<GridSpaceMono>().Space).Returns(true);

			yield return null;

			moveToken.OnMouseUp();

			yield return null;

			gridManager.Received().Move(tileObject1.GetComponent<GridSpaceMono>().Space, tileObject2.GetComponent<GridSpaceMono>().Space);
			Assert.AreEqual(tileObject2.GetComponent<GridSpaceMono>().Space, token.CurrentSpace);
			lerpable.Received().Lerp(expectedSource, expectedDestination, expectedLerpDuration, Arg.Any<Action<Vector3>>());
		}

		[UnityTest]
		public IEnumerator OnMouseUp_GivenTokenWasMovedAndCastRayForTagReturnedTileThatIsEmpty_StopTheCurrentSelectEffectAndUpdateTheInitialSelectEffect() {
			tokenObject.transform.position = new Vector3(2, 1, 2);
			moveToken.CurrentSelectEffect = selectEffect2;
			
			raycastable.CastRayForTag(AppConstants.GRID_SPACE_TAG).Returns(tileObject2);
			gridManager.IsEmpty(tileObject2.GetComponent<GridSpaceMono>().Space).Returns(true);
			
			yield return null;

			moveToken.OnMouseUp();

			yield return null;

			selectEffect2.Received().Stop();
			Assert.AreEqual(selectEffect2, moveToken.InitialSelectEffect);
		}
	}
}
