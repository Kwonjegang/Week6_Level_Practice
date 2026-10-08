# Blockout Review: decisions and verification points

## Confirmed layout intent

- Keep the supplied 00–09 progression and optional 08 family-photo branch.
- Preserve the reference's bent room arrangement: 00–01 sit in the lower-left, the route turns north into the 02–05 wing, then turns south into 06–09. 08 branches below 06. This is a connected room layout, not a straight row of rooms.
- 00 is a quiet start with basic movement and a safe jump. 01 teaches the guaranteed robot A-to-B interaction jump across an 8.4 m gap. In 02, the robot uses a diagonal A-to-B transfer of about 7.8 m after the girl opens G1. 03 changes the landing height to apply the jump again.
- In 02 the girl opens G1, crosses the child-sized low route to B, and waits. The robot reaches A, interacts, automatically travels to B, and meets her. 03 applies the same robot jump to a raised landing.
- 04 prepares the barrier; 05 is a large toxic opening with four isolated platforms in a right-left-right-left sequence, each held by a narrow corroded pillar. The pair stays mounted across it.
- In 06, the girl uses SW3 to open H3 and reveal the red cube. W1 remains the required exit block, and 08 is optional.

## Jump spacing in the current layout

- 01 A and B centers are 8.4 m apart; the opposing decks leave a 4.8 m opening between their edges. This makes the F/X transfer the intended way across.
- 02 A and B are offset across the room, about 7.8 m apart diagonally. Both pads remain inside the safe approach and landing decks.
- 03 remains a shorter raised-landing application beat.

## Implemented for this playtest pass

- Camera-relative WASD/arrow/gamepad movement, jump, third-person follow, delayed camera follow, and right-mouse orbit.
- Robot and girl character focus switch while apart; E/B mounts or dismounts them. The inactive character waits. The girl is smaller and can pass the 02/06 low route, while the robot is blocked by its roof.
- The robot uses F/X at A to travel safely to the paired B pad; the travel time scales with horizontal distance. Raised landings calculate their arrival height from the platform surface. The girl's jump is a very low hop; the robot also has a safe short jump for 00.
- The girl uses F/X near SW1 and SW3 to remove the G1/H3 gate panel and change the button color.
- The layout is grouped into the supplied lower-left, upper-wing, lower-right, and optional lower-branch arrangement. No in-scene text labels are generated.

## Deliberately outside this pass

Ability pickup and switching, barrier protection, toxic damage, wall destruction at W1, follow command, animations, prompts, and UI are not implemented. W1 therefore remains a solid stop after the 06 room. The runtime follow camera is shortened to fit the small rooms, and its world yaw no longer rotates the movement basis when the character turns. The editor preview camera follows the starting character rather than framing the entire layout.

## Checks to perform against final assets

1. Confirm 1 Unity unit is close to 1 meter and adjust capsule, speed, and jump height to the final models.
2. Confirm the girl's visible pose fits beneath the low roof while her CharacterController can pass; the blockout uses a 1.1 m capsule and a 1.25 m clearance.
3. Walk both connector turns in play mode and inspect that their floors meet the adjacent door openings without gaps.
4. Test the toxic bridge after barrier logic exists. This pass prevents dismounting there, but does not provide barrier protection or catch/fail behavior.
5. Confirm A-to-B pads align with the final movement camera and control scheme.
