using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static ColonySim.InputControlMap;
using ColonySim.LoggingUtility;
using ILogger = ColonySim.LoggingUtility.ILogger;

namespace ColonySim.Systems
{
    public class CameraSystem : System, ICameraActions, ILogger
    {
        #region Static
        private static CameraSystem instance;
        public static CameraSystem Get => instance;
        public List<string> Logs { get; set; } = new List<string>();
        public LoggingLevel LoggingLevel { get => _loggingLevel; set => _loggingLevel = value; }
        [SerializeField]
        private LoggingLevel _loggingLevel = LoggingLevel.Warning;
        public LoggingPriority LoggingPriority { get => _loggingPriority; set => _loggingPriority = value; }
        [SerializeField]
        private LoggingPriority _loggingPriority = LoggingPriority.AlwaysShow;
        public bool Stamp { get => _stamp; set => _stamp = value; }
        public string LoggingPrefix => "<color=purple>[CAMERA]</color>";
        [SerializeField]
        private bool _stamp = false;
        #endregion

        #region Main Camera Control
        public Camera controlled;
        public static float MinimumCameraBound = 3;
        public static float MaximumCameraBound = 20;

        private bool _runningCameraMove = false;
        private bool cameraMoveActuated = false;

        [SerializeField]
        private float cameraDamp = 10.0f;
        [SerializeField]
        private float cameraZoom = 1.0f;
        [SerializeField]
        private float cameraMovement = 1.0f;

        private void Awake()
        {
            instance = this;
        }

        public override void Init()
        {
            this.Notice("> Camera System Init <");
            instance = this;
            var distance = Vector3.Distance(FOVCamera.transform.position, Background.transform.position);
            initHeightAtDist = FrustumHeightAtDistance(distance);
            desiredZ = FOVCamera.transform.position.z;
            base.Init();
        }

        public override void OnInitialized()
        {
            InputSystem.CameraActions.SetCallbacks(this);
            InputSystem.CameraActions.Movement.performed += OnMovement;
            InputSystem.CameraActions.Movement.canceled += OnMovementCancel;
            InputSystem.CameraActions.Enable();
            base.OnInitialized();
        }

        public void OnZoom(InputAction.CallbackContext Context)
        {
            if (InputSystem.AllowMouseEvent)
            {
                float value = Context.ReadValue<Vector2>().y * 0.00025f;
                Camera.main.orthographicSize -= Instance_CameraZoomMultipler() * value;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, MinimumCameraBound, MaximumCameraBound);

                desiredZ += CameraSystem.CameraZoomMultiplier() * value;
                desiredZ = Mathf.Clamp(desiredZ, -CameraSystem.MaximumCameraBound, -CameraSystem.MinimumCameraBound);

                FOVCamera.transform.position = new Vector3(FOVCamera.transform.position.x, FOVCamera.transform.position.y, desiredZ);
                var currDistance = Vector3.Distance(FOVCamera.transform.position, Background.transform.position);
                FOVCamera.fieldOfView = FOVForHeightAndDistance(initHeightAtDist, currDistance);

                if (!backgroundZoom)
                {
                    backgroundZoom = true;
                    StartCoroutine(BackgroundZoom());
                }
            }
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            cameraMoveActuated = true;
            if (!_runningCameraMove)
            {
                StartCoroutine(OnMovementContinue());
            }
        }
        IEnumerator OnMovementContinue()
        {
            this.Verbose("CameraMovementBegin");
            Vector2 targetPosition = Camera.main.transform.position;
            Vector2 velocity = Vector2.zero;
            _runningCameraMove = true;
            while (_runningCameraMove)
            {
                Vector2 cameraPos = Camera.main.transform.position;
                if (cameraMoveActuated)
                {
                    targetPosition += InputSystem.CameraActions.Movement.ReadValue<Vector2>() * Time.deltaTime * Instance_CameraMoveMultiplier();
                }

                Vector2 movement = Vector2.SmoothDamp(cameraPos, targetPosition, ref velocity, Time.deltaTime * cameraDamp);
                Camera.main.transform.position = new Vector3(movement.x, movement.y, Camera.main.transform.position.z);

                if (!cameraMoveActuated && (Vector2)Camera.main.transform.position == targetPosition)
                {
                    this.Verbose("CameraMovementEnd");
                    _runningCameraMove = false;
                }
                yield return null;
            }
        }

        public void OnMovementCancel(InputAction.CallbackContext context)
        {
            cameraMoveActuated = false;
        }

        public static float CameraZoomMultiplier()
        {
            return instance.Instance_CameraZoomMultipler();
        }

        private float Instance_CameraZoomMultipler()
        {
            return Camera.main.orthographicSize * cameraZoom;
        }

        private float Instance_CameraMoveMultiplier()
        {
            return Camera.main.orthographicSize * cameraMovement;
        }
        #endregion

        #region Background Camera

        public Camera FOVCamera;
        public GameObject Background;

        private float initHeightAtDist;
        private float desiredZ;
        private float dampingVelocity;
        private bool backgroundZoom = false;

        private float FrustumHeightAtDistance(float distance)
        {
            return 2.0f * distance * Mathf.Tan(FOVCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        }

        private float FOVForHeightAndDistance(float height, float distance)
        {
            return 2.0f * Mathf.Atan(height * 0.5f / distance) * Mathf.Rad2Deg;
        }

        private IEnumerator BackgroundZoom()
        {
            while (backgroundZoom)
            {
                if (Mathf.Abs(FOVCamera.transform.position.z - desiredZ) < 1)
                {
                    backgroundZoom = false;
                    break;
                }
                float newZ = Mathf.SmoothDamp(FOVCamera.transform.position.z, desiredZ, ref dampingVelocity, 0.1f);
                FOVCamera.transform.position = new Vector3(FOVCamera.transform.position.x, FOVCamera.transform.position.y, newZ);
                var currDistance = Vector3.Distance(FOVCamera.transform.position, Background.transform.position);
                FOVCamera.fieldOfView = FOVForHeightAndDistance(initHeightAtDist, currDistance);
                yield return null;
            }
            yield break;
        }

        #endregion
    }
}
