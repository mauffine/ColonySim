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
        public bool Stamp { get => _stamp; set => _stamp = value; }
        [SerializeField]
        private bool _stamp = false;
        #endregion

        #region Main Camera Control
        public Camera controlled;
        public static float MinimumCameraBound = 3;
        public static float MaximumCameraBound = 20;

        private bool movingCamera;

        private void Awake()
        {
            instance = this;
        }

        public override void Init()
        {
            this.Verbose("<color=blue>[Camera System Init]</color>");
            var distance = Vector3.Distance(FOVCamera.transform.position, Background.transform.position);
            initHeightAtDist = FrustumHeightAtDistance(distance);
            desiredZ = FOVCamera.transform.position.z;
            base.Init();
        }

        public override void OnInitialized()
        {
            InputSystem sys = InputSystem.Get;
            sys.CameraActions.SetCallbacks(this);
            sys.CameraActions.Movement.performed += OnMovement;
            sys.CameraActions.Movement.canceled += OnMovementCancel;
            sys.CameraActions.Enable();
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
            movingCamera = true;
            StartCoroutine(OnMovementContinue());
        }
        IEnumerator OnMovementContinue()
        {
            Vector2 velocity = Vector2.zero;
            while (movingCamera)
            {
                Vector2 targetPosition = (Vector2)Camera.main.transform.position + InputSystem.Get.CameraActions.Movement.ReadValue<Vector2>();
                Vector2 movement = Vector2.SmoothDamp(Camera.main.transform.position, targetPosition, ref velocity, Instance_CameraMoveMultiplier());
                Camera.main.transform.position = new Vector3(movement.x, movement.y, Camera.main.transform.position.z);
                yield return null;
            }
        }

        public void OnMovementCancel(InputAction.CallbackContext context)
        {
            movingCamera = false;
        }

        public static float CameraZoomMultiplier()
        {
            return instance.Instance_CameraZoomMultipler();
        }

        private float Instance_CameraZoomMultipler()
        {
            return Camera.main.orthographicSize * 0.5f;
        }

        private float Instance_CameraMoveMultiplier()
        {
            return Camera.main.orthographicSize * 0.1f;
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
