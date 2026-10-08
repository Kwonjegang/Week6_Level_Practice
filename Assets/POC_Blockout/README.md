# Week 6 POC Level Graybox

Scene: `Assets/Scenes/POC_Level_Blockout.unity`  
Generator: `Assets/Editor/POCLevelBuilder.cs`  
Play controls: `Assets/POC_Blockout/POCPlaytestController.cs`

This is a playable 3D blockout of the supplied 00–09 layout. The route follows the reference room graph: 00–01 begin in the lower-left, 02–05 turn into the upper wing, and the route turns back down into 06–09. The 08 family-photo discovery remains a short optional branch below 06. Scene geometry has no floating text labels.

## Play mode controls

- **WASD / arrow keys / gamepad left stick:** move relative to the camera.
- **Space / gamepad south button:** jump. The robot has a regular safe jump; the girl's jump is a very low hop.
- **Hold right mouse button and move mouse:** orbit the follow camera. It eases behind the active character.
- **Tab / gamepad right shoulder:** switch the controlled character while they are apart.
- **E / gamepad east button (B):** mount or dismount. Mounting switches control to the robot; dismounting switches to the girl. Dismount is blocked on the toxic bridge.
- **F / gamepad west button (X):** the girl opens G1 or H3 near its switch; the robot travels automatically from an A pad to its paired B pad.

The inactive character waits in place. The first play-mode focus is the robot. The two graybox characters use CharacterController capsules sized to their visible robot and girl proxies.

## Layout and implementation notes

- 00 is the quiet movement and safe-jump start; 01 teaches the robot's guaranteed A-to-B jump.
- 02 combines G1's child-only low route and the robot's A-to-B jump, so they split briefly and meet at B. The low roof is high enough for the smaller girl and blocks the robot; a separate crouch action is not required for this control pass.
- 03 repeats the robot jump at a wider, raised landing.
- 05 is a large toxic opening with four isolated, corroded platforms alternating right-left-right-left, supported by narrow pillars. The pair stays mounted there.
- In 06, the girl uses SW3 to open H3 and reveal the red cube. W1 remains as a visible exit block because destruction controls are outside this pass.

The prototype currently implements only movement, jumping, character focus, mount/dismount, robot A-to-B travel, and the two child-operated gates. It does not implement ability pickup/switching, barrier protection, wall destruction, follow behavior, animations, or interaction UI. Dimensions remain provisional at about 1 Unity unit per meter and need tuning against final character assets.

To regenerate the scene from Unity, use **Tools → POC → Build Week 6 Level Graybox**.
