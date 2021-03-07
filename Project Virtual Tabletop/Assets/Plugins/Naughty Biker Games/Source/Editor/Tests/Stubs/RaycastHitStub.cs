using UnityEngine;
using NaughtyBikerGames.SDK.Adapters.Interfaces;

namespace NaughtyBikerGames.SDK.Editor.Tests.Stubs {
	public class RaycastHitStub : IRaycastHit {
		public GameObject GameObject { get; set; }

		public bool IsAHit() {
			return GameObject != null;
		}
	}
}