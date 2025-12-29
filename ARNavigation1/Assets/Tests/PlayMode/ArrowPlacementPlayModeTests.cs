using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ARNavigation.Tests.PlayMode
{
    public class ArrowPlacementPlayModeTests
    {
        private Camera _cam;

        [UnitySetUp]
        public IEnumerator SetUp()
        {
            var camGO = new GameObject("TestCamera");
            _cam = camGO.AddComponent<Camera>();
            _cam.transform.position = new Vector3(0, 1.6f, 0);
            _cam.transform.forward = Vector3.forward;
            yield return null;
        }

        [UnityTearDown]
        public IEnumerator TearDown()
        {
            if (_cam != null)
            {
                Object.Destroy(_cam.gameObject);
            }
            yield return null;
        }

        [UnityTest]
        public IEnumerator ArrowIsPlacedForwardOnGround()
        {
            // Simulate arrow placement logic similar to OutdoorTouch's expected behavior:
            var arrow = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            arrow.name = "NavArrow";
            try
            {
                var distance = 2f;
                var forwardXZ = new Vector3(_cam.transform.forward.x, 0f, _cam.transform.forward.z).normalized;
                if (forwardXZ.sqrMagnitude < 1e-6f)
                    forwardXZ = Vector3.forward;
                var target = _cam.transform.position + forwardXZ * distance;
                target.y = 0f;

                arrow.transform.position = target;

                yield return null;

                Assert.That(Mathf.Abs(arrow.transform.position.y - 0f) < 1e-5f, "Arrow should be on ground Y=0");
                Assert.That(arrow.transform.position.z, Is.GreaterThan(1.99f).And.LessThan(2.01f));
                Assert.That(Mathf.Abs(arrow.transform.position.x - 0f) < 1e-5f);
            }
            finally
            {
                Object.Destroy(arrow);
            }
        }
    }
}
