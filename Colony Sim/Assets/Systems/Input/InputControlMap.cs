// GENERATED AUTOMATICALLY FROM 'Assets/Data/Systems/InputControlMap.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace ColonySim
{
    public class @InputControlMap : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputControlMap()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputControlMap"",
    ""maps"": [
        {
            ""name"": ""Construction"",
            ""id"": ""be40afc0-1c04-4043-8850-c61864acbcf1"",
            ""actions"": [
                {
                    ""name"": ""Place Tile"",
                    ""type"": ""Button"",
                    ""id"": ""e88172f3-a259-4e12-afff-a0e6704db448"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(pressPoint=0.1,behavior=2)""
                },
                {
                    ""name"": ""Remove Tile"",
                    ""type"": ""Button"",
                    ""id"": ""65a92f88-7198-44ff-bad2-6f86623f65bf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Undo"",
                    ""type"": ""Button"",
                    ""id"": ""2f3fa273-4ab5-4bf4-b959-7c61916b7e3d"",
                    ""expectedControlType"": ""Key"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Redo"",
                    ""type"": ""Button"",
                    ""id"": ""3d4808e9-d94a-49ae-9ba8-33c0b382fd3e"",
                    ""expectedControlType"": ""Key"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Autoplace"",
                    ""type"": ""Button"",
                    ""id"": ""db84a68e-d46c-46ca-add1-0b26a4a32272"",
                    ""expectedControlType"": ""Key"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotate Left"",
                    ""type"": ""Button"",
                    ""id"": ""c39ebe12-5bef-4282-b5e5-2551bd8b91f9"",
                    ""expectedControlType"": ""Key"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RotateRight"",
                    ""type"": ""Button"",
                    ""id"": ""bc4dc336-7e08-4861-97b0-0c98c291af8b"",
                    ""expectedControlType"": ""Key"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cycle"",
                    ""type"": ""Value"",
                    ""id"": ""54c9e88c-f3f5-4d1a-bdab-f34234c0620a"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MousePointer"",
                    ""type"": ""Value"",
                    ""id"": ""3a80e523-63fb-482b-bfb1-b13d477f025c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Ctrl"",
                    ""type"": ""Value"",
                    ""id"": ""3cdbc944-8002-4fec-a82f-bdef89625975"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2fb9cfe7-e7bf-4f03-89ed-10d8c0e18a21"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Place Tile"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""65f9de4d-ad57-4265-bc76-34be4efb5cbd"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Remove Tile"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""039455bb-d6e7-415f-ab29-58e74e6661ad"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Undo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""083d5134-e518-47b7-ba90-b66095d4aa73"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Redo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""65ec6b17-759a-49f5-807f-30669473ac60"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Autoplace"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f2c291ef-735b-4ad7-96bb-a85ecbe1c35c"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""Rotate Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6d9fe62e-4f9d-4a59-b005-24c209f31b57"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""RotateRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keys"",
                    ""id"": ""f9c1c662-7e61-4b5b-a51e-deb875a9c45b"",
                    ""path"": ""1DAxis(whichSideWins=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cycle"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""84377bbf-d574-4df3-8f76-598e80382f09"",
                    ""path"": ""<Keyboard>/comma"",
                    ""interactions"": ""Press(pressPoint=0.1)"",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""Cycle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""6cffb115-9cef-405f-91f1-7737a37d4636"",
                    ""path"": ""<Keyboard>/period"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""Cycle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""fa0d7efe-f706-47e2-8ee5-fc4a2241676a"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""MousePointer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2fb756c9-b3be-4b38-b252-2807684532fd"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""Ctrl"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Camera"",
            ""id"": ""7a5569f2-6466-460c-91e3-923af0c8333d"",
            ""actions"": [
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""cc8f9b90-1213-40e3-861d-bf4986d9700c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""4c4eed1c-ca44-4158-9f58-6a9e04dc5fdc"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""5d8638a4-a247-402f-b164-111c6aa99714"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Move"",
                    ""id"": ""c5bcdfb3-7703-4044-b7ea-be80fed75370"",
                    ""path"": ""2DVector"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8c8d948f-09bd-412e-92d6-74171e9ead3d"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""18d51e40-3922-4d3b-bc2d-69234a4dba88"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""2c668a00-5735-40c9-bde5-ca1f79899261"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2dd80e7b-be4e-49ba-836e-a2df583bfb59"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""PC"",
            ""bindingGroup"": ""PC"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Construction
            m_Construction = asset.FindActionMap("Construction", throwIfNotFound: true);
            m_Construction_PlaceTile = m_Construction.FindAction("Place Tile", throwIfNotFound: true);
            m_Construction_RemoveTile = m_Construction.FindAction("Remove Tile", throwIfNotFound: true);
            m_Construction_Undo = m_Construction.FindAction("Undo", throwIfNotFound: true);
            m_Construction_Redo = m_Construction.FindAction("Redo", throwIfNotFound: true);
            m_Construction_Autoplace = m_Construction.FindAction("Autoplace", throwIfNotFound: true);
            m_Construction_RotateLeft = m_Construction.FindAction("Rotate Left", throwIfNotFound: true);
            m_Construction_RotateRight = m_Construction.FindAction("RotateRight", throwIfNotFound: true);
            m_Construction_Cycle = m_Construction.FindAction("Cycle", throwIfNotFound: true);
            m_Construction_MousePointer = m_Construction.FindAction("MousePointer", throwIfNotFound: true);
            m_Construction_Ctrl = m_Construction.FindAction("Ctrl", throwIfNotFound: true);
            // Camera
            m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
            m_Camera_Zoom = m_Camera.FindAction("Zoom", throwIfNotFound: true);
            m_Camera_Movement = m_Camera.FindAction("Movement", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // Construction
        private readonly InputActionMap m_Construction;
        private IConstructionActions m_ConstructionActionsCallbackInterface;
        private readonly InputAction m_Construction_PlaceTile;
        private readonly InputAction m_Construction_RemoveTile;
        private readonly InputAction m_Construction_Undo;
        private readonly InputAction m_Construction_Redo;
        private readonly InputAction m_Construction_Autoplace;
        private readonly InputAction m_Construction_RotateLeft;
        private readonly InputAction m_Construction_RotateRight;
        private readonly InputAction m_Construction_Cycle;
        private readonly InputAction m_Construction_MousePointer;
        private readonly InputAction m_Construction_Ctrl;
        public struct ConstructionActions
        {
            private @InputControlMap m_Wrapper;
            public ConstructionActions(@InputControlMap wrapper) { m_Wrapper = wrapper; }
            public InputAction @PlaceTile => m_Wrapper.m_Construction_PlaceTile;
            public InputAction @RemoveTile => m_Wrapper.m_Construction_RemoveTile;
            public InputAction @Undo => m_Wrapper.m_Construction_Undo;
            public InputAction @Redo => m_Wrapper.m_Construction_Redo;
            public InputAction @Autoplace => m_Wrapper.m_Construction_Autoplace;
            public InputAction @RotateLeft => m_Wrapper.m_Construction_RotateLeft;
            public InputAction @RotateRight => m_Wrapper.m_Construction_RotateRight;
            public InputAction @Cycle => m_Wrapper.m_Construction_Cycle;
            public InputAction @MousePointer => m_Wrapper.m_Construction_MousePointer;
            public InputAction @Ctrl => m_Wrapper.m_Construction_Ctrl;
            public InputActionMap Get() { return m_Wrapper.m_Construction; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(ConstructionActions set) { return set.Get(); }
            public void SetCallbacks(IConstructionActions instance)
            {
                if (m_Wrapper.m_ConstructionActionsCallbackInterface != null)
                {
                    @PlaceTile.started -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnPlaceTile;
                    @PlaceTile.performed -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnPlaceTile;
                    @PlaceTile.canceled -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnPlaceTile;
                    @RemoveTile.started -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnRemoveTile;
                    @RemoveTile.performed -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnRemoveTile;
                    @RemoveTile.canceled -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnRemoveTile;
                    @Undo.started -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnUndo;
                    @Undo.performed -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnUndo;
                    @Undo.canceled -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnUndo;
                    @Redo.started -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnRedo;
                    @Redo.performed -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnRedo;
                    @Redo.canceled -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnRedo;
                    @Autoplace.started -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnAutoplace;
                    @Autoplace.performed -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnAutoplace;
                    @Autoplace.canceled -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnAutoplace;
                    @RotateLeft.started -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnRotateLeft;
                    @RotateLeft.performed -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnRotateLeft;
                    @RotateLeft.canceled -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnRotateLeft;
                    @RotateRight.started -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnRotateRight;
                    @RotateRight.performed -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnRotateRight;
                    @RotateRight.canceled -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnRotateRight;
                    @Cycle.started -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnCycle;
                    @Cycle.performed -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnCycle;
                    @Cycle.canceled -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnCycle;
                    @MousePointer.started -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnMousePointer;
                    @MousePointer.performed -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnMousePointer;
                    @MousePointer.canceled -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnMousePointer;
                    @Ctrl.started -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnCtrl;
                    @Ctrl.performed -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnCtrl;
                    @Ctrl.canceled -= m_Wrapper.m_ConstructionActionsCallbackInterface.OnCtrl;
                }
                m_Wrapper.m_ConstructionActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @PlaceTile.started += instance.OnPlaceTile;
                    @PlaceTile.performed += instance.OnPlaceTile;
                    @PlaceTile.canceled += instance.OnPlaceTile;
                    @RemoveTile.started += instance.OnRemoveTile;
                    @RemoveTile.performed += instance.OnRemoveTile;
                    @RemoveTile.canceled += instance.OnRemoveTile;
                    @Undo.started += instance.OnUndo;
                    @Undo.performed += instance.OnUndo;
                    @Undo.canceled += instance.OnUndo;
                    @Redo.started += instance.OnRedo;
                    @Redo.performed += instance.OnRedo;
                    @Redo.canceled += instance.OnRedo;
                    @Autoplace.started += instance.OnAutoplace;
                    @Autoplace.performed += instance.OnAutoplace;
                    @Autoplace.canceled += instance.OnAutoplace;
                    @RotateLeft.started += instance.OnRotateLeft;
                    @RotateLeft.performed += instance.OnRotateLeft;
                    @RotateLeft.canceled += instance.OnRotateLeft;
                    @RotateRight.started += instance.OnRotateRight;
                    @RotateRight.performed += instance.OnRotateRight;
                    @RotateRight.canceled += instance.OnRotateRight;
                    @Cycle.started += instance.OnCycle;
                    @Cycle.performed += instance.OnCycle;
                    @Cycle.canceled += instance.OnCycle;
                    @MousePointer.started += instance.OnMousePointer;
                    @MousePointer.performed += instance.OnMousePointer;
                    @MousePointer.canceled += instance.OnMousePointer;
                    @Ctrl.started += instance.OnCtrl;
                    @Ctrl.performed += instance.OnCtrl;
                    @Ctrl.canceled += instance.OnCtrl;
                }
            }
        }
        public ConstructionActions @Construction => new ConstructionActions(this);

        // Camera
        private readonly InputActionMap m_Camera;
        private ICameraActions m_CameraActionsCallbackInterface;
        private readonly InputAction m_Camera_Zoom;
        private readonly InputAction m_Camera_Movement;
        public struct CameraActions
        {
            private @InputControlMap m_Wrapper;
            public CameraActions(@InputControlMap wrapper) { m_Wrapper = wrapper; }
            public InputAction @Zoom => m_Wrapper.m_Camera_Zoom;
            public InputAction @Movement => m_Wrapper.m_Camera_Movement;
            public InputActionMap Get() { return m_Wrapper.m_Camera; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
            public void SetCallbacks(ICameraActions instance)
            {
                if (m_Wrapper.m_CameraActionsCallbackInterface != null)
                {
                    @Zoom.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnZoom;
                    @Zoom.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnZoom;
                    @Zoom.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnZoom;
                    @Movement.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnMovement;
                    @Movement.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnMovement;
                    @Movement.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnMovement;
                }
                m_Wrapper.m_CameraActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Zoom.started += instance.OnZoom;
                    @Zoom.performed += instance.OnZoom;
                    @Zoom.canceled += instance.OnZoom;
                    @Movement.started += instance.OnMovement;
                    @Movement.performed += instance.OnMovement;
                    @Movement.canceled += instance.OnMovement;
                }
            }
        }
        public CameraActions @Camera => new CameraActions(this);
        private int m_PCSchemeIndex = -1;
        public InputControlScheme PCScheme
        {
            get
            {
                if (m_PCSchemeIndex == -1) m_PCSchemeIndex = asset.FindControlSchemeIndex("PC");
                return asset.controlSchemes[m_PCSchemeIndex];
            }
        }
        public interface IConstructionActions
        {
            void OnPlaceTile(InputAction.CallbackContext context);
            void OnRemoveTile(InputAction.CallbackContext context);
            void OnUndo(InputAction.CallbackContext context);
            void OnRedo(InputAction.CallbackContext context);
            void OnAutoplace(InputAction.CallbackContext context);
            void OnRotateLeft(InputAction.CallbackContext context);
            void OnRotateRight(InputAction.CallbackContext context);
            void OnCycle(InputAction.CallbackContext context);
            void OnMousePointer(InputAction.CallbackContext context);
            void OnCtrl(InputAction.CallbackContext context);
        }
        public interface ICameraActions
        {
            void OnZoom(InputAction.CallbackContext context);
            void OnMovement(InputAction.CallbackContext context);
        }
    }
}
