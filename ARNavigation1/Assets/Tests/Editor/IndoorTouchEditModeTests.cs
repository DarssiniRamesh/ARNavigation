using NUnit.Framework;
using UnityEngine;

namespace ARNavigation.Tests.Editor
{
    // A minimal selector interface to abstract indoor object picking logic.
    public interface ISelectable
    {
        string Id { get; }
        Bounds Bounds { get; }
    }

    // PUBLIC_INTERFACE
    public static class IndoorSelectionLogic
    {
        /** This is a public function.
         * Determines if a screen tap hits a given selectable when projected as a simple AABB hit-test
         * in a fake orthographic top-down view used for unit testing logic.
         */
        public static bool IsTapOnSelectable(Vector2 screenTap, ISelectable selectable, float pixelsPerUnit = 100f)
        {
            // For unit tests, consider a simple mapping: world (x,z) -> screen (x,z) scaled.
            var min = new Vector2(selectable.Bounds.min.x, selectable.Bounds.min.z) * pixelsPerUnit;
            var max = new Vector2(selectable.Bounds.max.x, selectable.Bounds.max.z) * pixelsPerUnit;

            var hit = screenTap.x >= min.x && screenTap.x <= max.x &&
                      screenTap.y >= min.y && screenTap.y <= max.y;
            return hit;
        }
    }

    public class DummySelectable : ISelectable
    {
        public string Id { get; private set; }
        public Bounds Bounds { get; private set; }

        public DummySelectable(string id, Bounds bounds)
        {
            Id = id;
            Bounds = bounds;
        }
    }

    [TestFixture]
    public class IndoorTouchEditModeTests
    {
        [Test]
        public void IsTapOnSelectable_ReturnsTrue_WhenTapInsideBounds()
        {
            var sel = new DummySelectable("Room-HOD", new Bounds(center: new Vector3(2, 0, 3), size: new Vector3(2, 0, 2)));
            // bounds: x in [1,3], z in [2,4] -> scaled by 100 => x in [100,300], y in [200,400]
            var tap = new Vector2(150, 250);
            Assert.IsTrue(IndoorSelectionLogic.IsTapOnSelectable(tap, sel));
        }

        [Test]
        public void IsTapOnSelectable_ReturnsFalse_WhenTapOutsideBounds()
        {
            var sel = new DummySelectable("Room-101", new Bounds(center: new Vector3(2, 0, 3), size: new Vector3(2, 0, 2)));
            var tap = new Vector2(50, 50);
            Assert.IsFalse(IndoorSelectionLogic.IsTapOnSelectable(tap, sel));
        }

        [Test]
        public void IsTapOnSelectable_BorderInclusive()
        {
            var sel = new DummySelectable("Room-Edge", new Bounds(center: new Vector3(0, 0, 0), size: new Vector3(2, 0, 2)));
            // bounds world: x [-1,1], z [-1,1] -> screen [-100,100] each
            var tap = new Vector2(100, 0);
            Assert.IsTrue(IndoorSelectionLogic.IsTapOnSelectable(tap, sel));
        }
    }
}
