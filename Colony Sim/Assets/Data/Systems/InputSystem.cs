using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace ColonySim.Systems
{
    public class InputSystem : System
    {
        private static InputSystem instance;
        public static InputSystem Get => instance;
        [SerializeField] private InputActionAsset InputAsset;
        private InputControlMap inputActions;
        private void Awake()
        {
            instance = this;
        }
        public override void Init()
        {
            inputActions = new InputControlMap();
            Initialized = true;
            base.Init();
        }
        public override void OnInitialized()
        {

        }

        public InputControlMap.ConstructionActions ConstructionActions => inputActions.Construction;
        public InputControlMap.CameraActions CameraActions => inputActions.Camera;

        public static bool AllowMouseEvent
        {
            get
            {
                return true;
            }
        }
    }
}
