using System;
using UnityEngine;
using NaughtyBikerGames.SDK.Adapters.Interfaces;

namespace NaughtyBikerGames.SDK.Editor.Tests.Stubs {
	public class PhysicsStub : IPhysics {
		RaycastHitStub raycastHitStub;

		public PhysicsStub(RaycastHitStub raycastHitStub) {
			this.raycastHitStub = raycastHitStub;
		}

		public IRaycastHit RaycastAllFirstOrDefault(Ray ray) {
			throw new NotImplementedException();
		}

		public IRaycastHit RaycastAllFirstOrDefault(Ray ray, Func<RaycastHit, bool> predicate) {
			return raycastHitStub;
		}
	}
}