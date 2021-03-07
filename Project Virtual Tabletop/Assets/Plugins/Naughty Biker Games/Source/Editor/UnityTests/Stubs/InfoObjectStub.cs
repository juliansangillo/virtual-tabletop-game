using UnityEngine;
using NaughtyBikerGames.SDK.InfoObjects.Interfaces;

namespace NaughtyBikerGames.SDK.Editor.UnityTests.Stubs {
	public class InfoObjectStub : MonoBehaviour, IInfoObject {
		public int Id { get; set; } = 0;
		public int CallCount { get; set; } = 0;

		public void FireAll() {
			this.CallCount++;
		}
	}
}