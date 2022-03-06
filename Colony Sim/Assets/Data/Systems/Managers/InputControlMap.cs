// GENERATED AUTOMATICALLY FROM 'Assets/Data/Systems/Input/InputControlMap.inputactions'

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
                    ""interactions"": """"
                },
                {
                    ""name"": ""Remove Tile"",
                    ""type"": ""Button"",
                    ""id"": ""65a92f88-7198-44ff-bad2-6f86623f65bf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Undo"",
                    ""type"": ""Button"",
                    ""id"": ""d0344953-aede-4d3a-871e-5efad76f42a9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Redo"",
                    ""type"": ""Button"",
                    ""id"": ""4fd7dd68-7a2d-4c1e-9b52-7f7902f869b5"",
                    ""expectedControlType"": ""Button"",
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
                    ""id"": ""5165c723-e8e1-48da-96e8-634f8a7b3caf"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Undo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4a6c4641-f3a3-493e-bf16-a05580c519d3"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Redo"",
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
        },
        {
            ""name"": ""Mouse"",
            ""id"": ""bfa2a425-74aa-4eb2-b5f8-3875c28669a6"",
            ""actions"": [
                {
                    ""name"": ""MousePointer"",
                    ""type"": ""Value"",
                    ""id"": ""6fb1e2cb-08ba-4ffc-8845-803749292175"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d8b8d7f8-c676-42c9-983b-e22d5449a39d"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";PC"",
                    ""action"": ""MousePointer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Player"",
            ""id"": ""3751751d-245b-4c29-8f5a-506a942b14c0"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""828f1480-a9a1-4191-830a-0d40f2c64190"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""32e419f2-9a6a-40e1-8bb9-61e70ef9ec99"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""33b0ccab-e49c-47f3-bb24-35c4f60b204c"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""PC"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fed2fdc0-e5eb-44af-ad3c-8467c89b905c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
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
            // Camera
            m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
            m_Camera_Zoom = m_Camera.FindAction("Zoom", throwIfNotFound: true);
            m_Camera_Movement = m_Camera.FindAction("Movement", throwIfNotFound: true);
            // Mouse
            m_Mouse = asset.FindActionMap("Mouse", throwIfNotFound: true);
            m_Mouse_MousePointer = m_Mouse.FindAction("MousePointer", throwIfNotFound: true);
            // Player
            m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
            m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
            m_Player_Select = m_Player.FindAction("Select", throwIfNotFound: true);
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
        public struct ConstructionActions
        {
            private @InputControlMap m_Wrapper;
            public ConstructionActions(@InputControlMap wrapper) { m_Wrapper = wrapper; }
            public InputAction @PlaceTile => m_Wrapper.m_Construction_PlaceTile;
            public InputAction @RemoveTile => m_Wrapper.m_Construction_RemoveTile;
            public InputAction @Undo => m_Wrapper.m_Construction_Undo;
            public InputAction @Redo => m_Wrapper.m_Construction_Redo;
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

        // Mouse
        private readonly InputActionMap m_Mouse;
        private IMouseActions m_MouseActionsCallbackInterface;
        private readonly InputAction m_Mouse_MousePointer;
        public struct MouseActions
        {
            private @InputControlMap m_Wrapper;
            public MouseActions(@InputControlMap wrapper) { m_Wrapper = wrapper; }
            public InputAction @MousePointer => m_Wrapper.m_Mouse_MousePointer;
            public InputActionMap Get() { return m_Wrapper.m_Mouse; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MouseActions set) { return set.Get(); }
            public void SetCallbacks(IMouseActions instance)
            {
                if (m_Wrapper.m_MouseActionsCallbackInterface != null)
                {
                    @MousePointer.started -= m_Wrapper.m_MouseActionsCallbackInterface.OnMousePointer;
                    @MousePointer.performed -= m_Wrapper.m_MouseActionsCallbackInterface.OnMousePointer;
                    @MousePointer.canceled -= m_Wrapper.m_MouseActionsCallbackInterface.OnMousePointer;
                }
                m_Wrapper.m_MouseActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @MousePointer.started += instance.OnMousePointer;
                    @MousePointer.performed += instance.OnMousePointer;
                    @MousePointer.canceled += instance.OnMousePointer;
                }
            }
        }
        public MouseActions @Mouse => new MouseActions(this);

        // Player
        private readonly InputActionMap m_Player;
        private IPlayerActions m_PlayerActionsCallbackInterface;
        private readonly InputAction m_Player_Move;
        private readonly InputAction m_Player_Select;
        public struct PlayerActions
        {
            private @InputControlMap m_Wrapper;
            public PlayerActions(@InputControlMap wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_Player_Move;
            public InputAction @Select => m_Wrapper.m_Player_Select;
            public InputActionMap Get() { return m_Wrapper.m_Player; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
            public void SetCallbacks(IPlayerActions instance)
            {
                if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
                {
                    @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                    @Select.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                    @Select.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                    @Select.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnSelect;
                }
                m_Wrapper.m_PlayerActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @Select.started += instance.OnSelect;
                    @Select.performed += instance.OnSelect;
                    @Select.canceled += instance.OnSelect;
                }
            }
        }
        public PlayerActions @Player => new PlayerActions(this);
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
        }
        public interface ICameraActions
        {
            void OnZoom(InputAction.CallbackContext context);
            void OnMovement(InputAction.CallbackContext context);
        }
        public interface IMouseActions
        {
            void OnMousePointer(InputAction.CallbackContext context);
        }
        public interface IPlayerActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnSelect(InputAction.CallbackContext context);
        }
    }
}
