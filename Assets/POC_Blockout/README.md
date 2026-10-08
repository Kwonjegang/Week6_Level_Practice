# Week 6 POC Level Graybox

Scene: `Assets/Scenes/POC_Level_Blockout.unity`  
Generator: `Assets/Editor/POCLevelBuilder.cs`  
Unity version: `6000.0.55f1`

This is a spatial blockout of the approved 00–09 layout. It keeps the supplied room order and marks the intended wall gates, child-only crawl routes, A/B automatic-jump pads, ability pickups, toxic open gap, alternating platforms, switches, red ability alcove, required W1 wall, optional W2/photo branch, and rejoin/exit spaces.

## How to view

Open `POC_Level_Blockout.unity`. The orthographic overview camera frames the whole layout so the route and blockout geometry are easy to inspect.

To regenerate the scene from the Editor script, use **Tools → POC → Build Week 6 Level Graybox**.

## Scope and assumptions

- The map is authored in 3D on the XZ plane; required progression travels along +X.
- 1 Unity unit is provisionally treated as about 1 meter. The project has no character movement or collision metrics yet, so dimensions and jump distances are blockout estimates.
- Floors, walls, gates, and support pillars use simple primitive geometry. The included scene has no player controller, character switching, automatic jump, barrier, switch/gate logic, destructible-wall behavior, or gameplay UI. Colored props and labels mark intended interactions for layout review.
- In 05, four separate slabs alternate right/left relative to forward travel and each is supported by a narrow rusted pillar. No walkable side bypass is provided.
- 02's low duct bypasses G1 for the child and reaches the B-side waiting nook. 06's short crawl route reaches SW3 and returns to the main room; H3 protects the red ability item in a side alcove.
- The existing sample scene is retained; the graybox is a separate scene and is added to the build scene list.

Before replacing the placeholders with the real movement system, validate the room dimensions, duct clearance, ordinary jump range, and camera behavior against the actual character controller.
