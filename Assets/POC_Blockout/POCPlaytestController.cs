using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Minimal POC controls: move, jump, switch character, mount, and use nearby interactions.
/// </summary>
public sealed class POCPlaytestController : MonoBehaviour
{
    [SerializeField] private Transform girl;
    [SerializeField] private Transform robot;
    [SerializeField] private Camera followCamera;

    private CharacterController girlBody;
    private CharacterController robotBody;
    private Transform activeCharacter;
    private bool girlIsCarried;
    private bool robotIsJumping;
    private float girlVerticalSpeed;
    private float robotVerticalSpeed;
    private float cameraYaw = 90f;
    private float cameraPitch = 16f;
    private Vector3 cameraVelocity;

    // Small runtime InputAction set keeps all play controls in one place.
    private InputActionMap controls;
    private InputAction move;
    private InputAction jump;
    private InputAction interact;
    private InputAction switchCharacter;
    private InputAction mount;
    private InputAction orbitHold;
    private InputAction lookDelta;

    private Transform switchG1;
    private Transform switchH3;
    private GameObject gateG1;
    private GameObject gateH3;
    private Transform[] robotJumpStarts;
    private Transform[] robotJumpLandings;

    public void Configure(Transform girlRoot, Transform robotRoot, Camera camera)
    {
        // AddComponent calls Awake before the builder can pass these scene references.
        girl = girlRoot;
        robot = robotRoot;
        followCamera = camera;
        girlBody = girl != null ? girl.GetComponent<CharacterController>() : null;
        robotBody = robot != null ? robot.GetComponent<CharacterController>() : null;
        activeCharacter = robot;
    }

    private void Awake()
    {
        girlBody = girl != null ? girl.GetComponent<CharacterController>() : null;
        robotBody = robot != null ? robot.GetComponent<CharacterController>() : null;
        followCamera = followCamera != null ? followCamera : Camera.main;

        switchG1 = FindTransform("SW1 - child opens G1 pedestal");
        switchH3 = FindTransform("SW3 - child opens H3 pedestal");
        gateG1 = GameObject.Find("G1 - closed main route gate");
        gateH3 = GameObject.Find("06 H3 gate");
        robotJumpStarts = new[]
        {
            FindTransform("01 A - F/X auto jump pad"),
            FindTransform("02 A - robot start pad"),
            FindTransform("03 A - application launch pad")
        };
        robotJumpLandings = new[]
        {
            FindTransform("01 B - auto landing pad"),
            FindTransform("02 B - robot reunion pad"),
            FindTransform("03 B - elevated landing pad")
        };

        // Robot movement is the main play mode; the girl waits beside it until dismounted.
        activeCharacter = robot;
        CreateInputActions();
    }

    private void OnEnable()
    {
        controls?.Enable();
    }

    private void OnDisable()
    {
        controls?.Disable();
    }

    private void OnDestroy()
    {
        controls?.Dispose();
    }

    private void Update()
    {
        if (girl == null || robot == null)
            return;

        if (!girlIsCarried && switchCharacter.WasPressedThisFrame())
            SelectCharacter(activeCharacter == robot ? girl : robot);

        if (mount.WasPressedThisFrame())
            ToggleCarry();

        if (interact.WasPressedThisFrame())
            TryInteract();

        MoveActiveCharacter();
    }

    private void LateUpdate()
    {
        if (followCamera == null || activeCharacter == null)
            return;

        // Hold the right mouse button and move the mouse to orbit behind the active character.
        if (orbitHold.IsPressed())
        {
            Vector2 delta = lookDelta.ReadValue<Vector2>();
            cameraYaw += delta.x * 0.12f;
            cameraPitch = Mathf.Clamp(cameraPitch - delta.y * 0.08f, 8f, 38f);
        }

        // Keep camera yaw in world space so steering does not rotate the movement basis.
        Vector3 behind = Quaternion.AngleAxis(cameraYaw, Vector3.up) * Vector3.back;
        float pitch = cameraPitch * Mathf.Deg2Rad;
        Vector3 offset = behind * (3.0f * Mathf.Cos(pitch))
                       + Vector3.up * (0.9f + 3.0f * Mathf.Sin(pitch));
        Vector3 lookAt = activeCharacter.position + Vector3.up * (girlIsCarried ? 1.9f : 1.0f);

        // SmoothDamp gives the camera a brief follow lag after turning or changing character.
        followCamera.transform.position = Vector3.SmoothDamp(
            followCamera.transform.position, lookAt + offset, ref cameraVelocity, 0.18f);
        followCamera.transform.LookAt(lookAt);
    }

    private void CreateInputActions()
    {
        controls = new InputActionMap("POC Controls");

        move = controls.AddAction("Move", InputActionType.Value);
        move.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        move.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");
        move.AddBinding("<Gamepad>/leftStick");

        jump = controls.AddAction("Jump", InputActionType.Button);
        jump.AddBinding("<Keyboard>/space");
        jump.AddBinding("<Gamepad>/buttonSouth");

        interact = controls.AddAction("Interact", InputActionType.Button);
        interact.AddBinding("<Keyboard>/f");
        interact.AddBinding("<Gamepad>/buttonWest");

        switchCharacter = controls.AddAction("Switch Character", InputActionType.Button);
        switchCharacter.AddBinding("<Keyboard>/tab");
        switchCharacter.AddBinding("<Gamepad>/rightShoulder");

        mount = controls.AddAction("Mount", InputActionType.Button);
        mount.AddBinding("<Keyboard>/e");
        mount.AddBinding("<Gamepad>/buttonEast");

        orbitHold = controls.AddAction("Orbit Hold", InputActionType.Button);
        orbitHold.AddBinding("<Mouse>/rightButton");
        lookDelta = controls.AddAction("Look Delta", InputActionType.Value);
        lookDelta.AddBinding("<Mouse>/delta");
    }

    private void MoveActiveCharacter()
    {
        bool controllingRobot = activeCharacter == robot;
        CharacterController body = controllingRobot ? robotBody : girlBody;
        if (body == null || !body.enabled)
            return;

        Vector2 axes = move.ReadValue<Vector2>();
        Vector3 forward = followCamera != null
            ? Vector3.ProjectOnPlane(followCamera.transform.forward, Vector3.up).normalized
            : Vector3.right;
        Vector3 right = followCamera != null
            ? Vector3.ProjectOnPlane(followCamera.transform.right, Vector3.up).normalized
            : Vector3.back;
        Vector3 direction = Vector3.ClampMagnitude(forward * axes.y + right * axes.x, 1f);

        if (direction.sqrMagnitude > 0.01f)
            activeCharacter.rotation = Quaternion.LookRotation(direction, Vector3.up);

        ref float verticalSpeed = ref (controllingRobot ? ref robotVerticalSpeed : ref girlVerticalSpeed);
        if (body.isGrounded && verticalSpeed < 0f)
            verticalSpeed = -1.5f;

        // The girl's jump is a small hop; the robot can make the ordinary safe tutorial jump.
        if (jump.WasPressedThisFrame() && body.isGrounded)
            verticalSpeed = controllingRobot ? 4.2f : 2.6f;

        verticalSpeed -= 18f * Time.deltaTime;
        float speed = controllingRobot ? 4.0f : 2.7f;
        body.Move((direction * speed + Vector3.up * verticalSpeed) * Time.deltaTime);
    }

    private void ToggleCarry()
    {
        if (!girlIsCarried)
        {
            if (Vector3.Distance(girl.position, robot.position) > 2.4f)
                return;

            girlBody.enabled = false;
            girl.SetParent(robot, false);
            girl.localPosition = Vector3.up * 1.72f;
            girl.localRotation = Quaternion.identity;
            girlIsCarried = true;
            SelectCharacter(robot);
            return;
        }

        // Keep the pair together while crossing the toxic bridge.
        bool atToxicBridge = robot.position.x >= 51f && robot.position.x <= 64f
                          && robot.position.z >= 6.4f && robot.position.z <= 17.6f;
        if (atToxicBridge)
            return;

        Transform levelRoot = transform.parent;
        girl.SetParent(levelRoot, true);
        girl.position = robot.position + robot.right * 0.9f;
        girl.rotation = robot.rotation;
        girlBody.enabled = true;
        girlIsCarried = false;
        SelectCharacter(girl);
    }

    private void SelectCharacter(Transform character)
    {
        if (girlIsCarried && character != robot)
            return;

        activeCharacter = character;
        cameraVelocity = Vector3.zero; // The camera eases to the newly controlled character.
    }

    private void TryInteract()
    {
        if (activeCharacter == girl && !girlIsCarried)
        {
            if (IsNear(switchG1, girl.position))
                OpenGate(gateG1, "SW1 - child opens G1 button");
            if (IsNear(switchH3, girl.position))
                OpenGate(gateH3, "SW3 - child opens H3 button");
        }

        if (activeCharacter == robot && !robotIsJumping)
            TryRobotAutoJump();
    }

    private void TryRobotAutoJump()
    {
        for (int i = 0; i < robotJumpStarts.Length; i++)
        {
            Transform start = robotJumpStarts[i];
            Transform landing = robotJumpLandings[i];
            if (start != null && landing != null && IsNear(start, robot.position, 1.7f))
            {
                StartCoroutine(RobotAutoJump(landing));
                return;
            }
        }
    }

    private IEnumerator RobotAutoJump(Transform landing)
    {
        robotIsJumping = true;
        robotBody.enabled = false;
        Vector3 start = robot.position;
        Vector3 end = landing.position;
        end.y = landing.position.y - 0.07f; // Align robot feet with the actual B platform surface.

        // The planned A-to-B move is guaranteed: one interaction carries the robot to B.
        float horizontalDistance = Vector2.Distance(new Vector2(start.x, start.z), new Vector2(end.x, end.z));
        float travelTime = Mathf.Max(0.85f, horizontalDistance / 6.2f);
        float elapsed = 0f;
        while (elapsed < travelTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / travelTime);
            robot.position = Vector3.Lerp(start, end, t) + Vector3.up * (Mathf.Sin(t * Mathf.PI) * 1.8f);
            yield return null;
        }

        robot.position = end;
        robotBody.enabled = true;
        robotIsJumping = false;
    }

    private static bool IsNear(Transform point, Vector3 position, float distance = 2.2f)
    {
        return point != null && Vector3.Distance(point.position, position) <= distance;
    }

    private static void OpenGate(GameObject gate, string buttonName)
    {
        if (gate == null || !gate.activeSelf)
            return;

        gate.SetActive(false);
        GameObject button = GameObject.Find(buttonName);
        Renderer buttonRenderer = button != null ? button.GetComponent<Renderer>() : null;
        if (buttonRenderer != null)
            buttonRenderer.material.color = new Color(0.25f, 0.8f, 0.4f);
    }

    private static Transform FindTransform(string objectName)
    {
        GameObject found = GameObject.Find(objectName);
        return found != null ? found.transform : null;
    }
}

