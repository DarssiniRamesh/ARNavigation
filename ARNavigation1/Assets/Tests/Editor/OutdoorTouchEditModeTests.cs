using System;
using NUnit.Framework;
using UnityEngine;

namespace ARNavigation.Tests.Editor
{
    // PUBLIC_INTERFACE
    public static class TouchMath
    {
        /** This is a public function.
         * Utility to compute a forward placement position for an arrow from a camera pose,
         * at a fixed distance on the ground (y=groundY).
         */
        public static Vector3 PlaceArrowForward(Vector3 camPosition, Vector3 camForward, float distance, float groundY = 0f)
        {
            // Project forward direction onto XZ plane to avoid pitching into sky.
            var forwardXZ = new Vector3(camForward.x, 0f, camForward.z);
            if (forwardXZ.sqrMagnitude < 1e-6f)
            {
                // Edge case: forward is almost vertical; default to world forward
                forwardXZ = Vector3.forward;
            }
            forwardXZ.Normalize();
            var target = camPosition + forwardXZ * distance;
            target.y = groundY;
            return target;
        }
    }

    [TestFixture]
    public class OutdoorTouchEditModeTests
    {
        [Test]
        public void PlaceArrowForward_ComputesFlatForward_OnGround()
        {
            var camPos = new Vector3(0, 1.6f, 0);
            var camFwd = new Vector3(0, 0, 1); // looking forward along +Z
            var pos = TouchMath.PlaceArrowForward(camPos, camFwd, 2f, 0f);

            Assert.That(pos.y, Is.EqualTo(0f).Within(1e-5f), "Arrow should be clamped to ground Y");
            Assert.That(pos.z, Is.GreaterThan(1.99f).And.LessThan(2.01f), "Should be approximately 2 units ahead on Z");
            Assert.That(pos.x, Is.EqualTo(0f).Within(1e-5f));
        }

        [Test]
        public void PlaceArrowForward_HandlesNearlyVerticalForward_DefaultsToWorldForward()
        {
            var camPos = new Vector3(1, 1.6f, 1);
            var camFwd = new Vector3(0, 1, 0); // straight up (degenerate for XZ projection)
            var pos = TouchMath.PlaceArrowForward(camPos, camFwd, 3f, 0f);

            Assert.That(pos.y, Is.EqualTo(0f).Within(1e-5f));
            // default to +Z
            Assert.That(pos.z, Is.GreaterThan(3.99f).And.LessThan(4.01f));
            Assert.That(pos.x, Is.EqualTo(1f).Within(1e-5f));
        }

        [Test]
        public void PlaceArrowForward_NormalizesForward_OnXZPlane()
        {
            var camPos = Vector3.zero;
            var camFwd = new Vector3(10, 0, 0.001f); // almost along +X
            var pos = TouchMath.PlaceArrowForward(camPos, camFwd, 5f, 0f);

            Assert.That(pos.y, Is.EqualTo(0f).Within(1e-5f));
            Assert.That(pos.x, Is.GreaterThan(4.9f).And.LessThan(5.1f));
        }
    }
}
