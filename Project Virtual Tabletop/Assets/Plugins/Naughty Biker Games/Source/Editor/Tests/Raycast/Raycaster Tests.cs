using NUnit.Framework;
using UnityEngine;
using NaughtyBikerGames.SDK.Editor.Tests.Stubs;
using NaughtyBikerGames.SDK.Raycast;

namespace NaughtyBikerGames.SDK.Editor.Tests.Raycast {
	public class RaycasterTests {
        private Raycaster raycaster;
        private RaycastHitStub raycastHitStub;

        [SetUp]
        public void SetUp() {
            raycastHitStub = new RaycastHitStub();
            raycaster = new Raycaster(new InputStub(), new MainCameraStub(), new PhysicsStub(raycastHitStub));
        }

		[Test]
		public void CastRayForTag_AGameObjectWithTagExistsOnRay_ReturnTheGameObject() {
			const string someExistingTag = "Foo";
            
            GameObject expected = new GameObject();
            raycastHitStub.GameObject = expected;

            GameObject actual = raycaster.CastRayForTag(someExistingTag);

            Assert.AreEqual(expected, actual);
		}

        [Test]
		public void CastRayForTag_NoGameObjectWithTagExistsOnRay_ReturnNull() {
			const string someNonExistingTag = "Bar";
            raycastHitStub.GameObject = null;

            GameObject actual = raycaster.CastRayForTag(someNonExistingTag);

            Assert.AreEqual(null, actual);
		}
	}
}
