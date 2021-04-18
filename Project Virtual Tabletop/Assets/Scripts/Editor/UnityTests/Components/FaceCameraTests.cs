using System.Collections;
using NaughtyBikerGames.ProjectVirtualTabletop.Components;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace NaughtyBikerGames.ProjectVirtualTabletop.Editor.UnityTests.Components {
	public class FaceCameraTests {
        Camera camera;
        FaceCamera objectToFaceCamera;

        [SetUp]
        public void SetUp() {
            camera = new GameObject().AddComponent<Camera>();
            camera.transform.position = new Vector3(0, 1, 1);
            camera.transform.rotation = Quaternion.Euler(45f, 30f, 0);

            objectToFaceCamera = new GameObject().AddComponent<FaceCamera>();
            objectToFaceCamera.transform.position = new Vector3(0, 0, 0);
            objectToFaceCamera.TargetCamera = camera;
        }

        [UnityTest]
        public IEnumerator Update_WhenCalled_ObjectLooksAtCamera() {
            yield return null;
            
            Assert.AreEqual(45f, Mathf.Round(objectToFaceCamera.transform.rotation.eulerAngles.x));
            Assert.AreEqual(30f, Mathf.Round(objectToFaceCamera.transform.rotation.eulerAngles.y));
            Assert.AreEqual(0, Mathf.Round(objectToFaceCamera.transform.rotation.eulerAngles.z));
        }
	}
}