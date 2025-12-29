# Tests Overview

This folder contains initial Unity Test Framework tests for the AR Navigation project, following the guidance in `kavia-docs/Testing-Strategy.md` and `kavia-docs/Test-Coverage-Map.md`.

Structure:
- Assets/Tests/Editor
  - ARNavigation.Tests.Editor.asmdef
  - OutdoorTouchEditModeTests.cs
  - IndoorTouchEditModeTests.cs
- Assets/Tests/PlayMode
  - ARNavigation.Tests.PlayMode.asmdef
  - ArrowPlacementPlayModeTests.cs
  - TouchIntegrationPlayModeTests.cs

EditMode tests cover pure logic:
- TouchMath.PlaceArrowForward: forward arrow placement computation on ground plane.
- IndoorSelectionLogic.IsTapOnSelectable: simple tap-to-selection bounds hit-test abstraction.

PlayMode tests cover minimal integration behaviors without Vuforia:
- Arrow placement using an actual Camera object.
- Tap raycast to ground plane collider to emulate touch interaction.

Running Tests:
- Open Unity with the project, open Window > General > Test Runner (or Test Framework).
- Run EditMode and PlayMode suites.
- In CI, use Unity Test Framework command line to run Editor and PlayMode tests.

Notes:
- These are foundational tests to create seams around `OutdoorTouch.cs` and `indoortouch.cs`. As code is refactored to expose pure logic, wire tests to those methods for deeper coverage.
