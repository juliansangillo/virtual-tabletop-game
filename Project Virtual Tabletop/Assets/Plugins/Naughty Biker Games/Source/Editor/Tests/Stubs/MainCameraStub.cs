using UnityEngine;
using NaughtyBikerGames.SDK.Adapters.Interfaces;

namespace NaughtyBikerGames.SDK.Editor.Tests.Stubs {
	public class MainCameraStub : IMainCamera {
		public Ray ScreenPointToRay(Vector3 pos) {
			return new Ray();
		}
	}
}