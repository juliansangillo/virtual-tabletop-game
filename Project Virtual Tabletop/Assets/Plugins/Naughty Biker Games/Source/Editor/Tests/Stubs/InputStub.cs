using UnityEngine;
using NaughtyBikerGames.SDK.Adapters.Interfaces;

namespace NaughtyBikerGames.SDK.Editor.Tests.Stubs {
	public class InputStub : IInput {
		public Vector3 MousePosition {
			get {
				return new Vector3(0, 0, 0);
			}
		}
	}
}