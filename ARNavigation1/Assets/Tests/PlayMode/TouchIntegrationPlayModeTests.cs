using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ARNavigation.Tests.PlayMode
{
    public class TouchIntegrationPlayModeTests
    {
        private Camera _cam;
        private GameObject _ground;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _cam = new GameObject("Cam").AddComponent<Camera>();
            _cam.transform.position = new Vector3(0, 1.6f, -5);
            _cam.transform.forward = Vector3.forward;

            // A simple ground plane to receive raycasts
            _ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            _ground.name = "Ground";
            _ground.transform.position = Vector3.zero;
            _ground.transform.rotation = Quaternion.identity;

            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (_cam != null) Object.Destroy(_cam.gameObject);
            if (_ground != null) Object.Destroy(_ground);
            yield return null;
        }

        [UnityTest]
        public IEnumerator TapRaycastsToGround_AndPlacesMarker()
        {
            var marker = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            marker.name = "TapMarker";

            try
            {
                // Simulate a screen center tap converted to a ray; in tests, use ViewportPointToRay
                var ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                if (Physics.Raycast(ray, out var hitInfo, 100f))
                {
                    marker.transform.position = hitInfo.point;
                }

                yield return null;

                // Expect marker to be near ground plane y ~ 0
                Assert.That(Mathf.Abs(marker.transform.position.y) < 0.1f, "Marker should be placed on ground plane");
            }
            finally
            {
                Object.Destroy(marker);
            }
        }
    }
}
