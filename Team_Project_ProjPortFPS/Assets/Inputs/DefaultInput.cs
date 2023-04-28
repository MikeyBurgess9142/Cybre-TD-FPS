//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.0
//     from Assets/Inputs/Default Input.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @DefaultInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @DefaultInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Default Input"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""25a1a5a4-f6a8-4ea5-a8bf-f8797cc3bd54"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""f64bc19c-00fe-464b-bf15-359bb5086d13"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""PassThrough"",
                    ""id"": ""8739ecf4-8d93-4573-9dd5-8a9af5f88c7f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""2f200d7a-9727-4043-bda9-b5285923366e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""8eb5ac34-25d2-4bba-96fd-4d28e0e1dd5e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Prone"",
                    ""type"": ""PassThrough"",
                    ""id"": ""b3d9fd47-ed2e-483b-8a81-f972baef4ad2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""5417d6e6-f4d0-4886-805e-3b9c2b31dd5e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint Released"",
                    ""type"": ""Button"",
                    ""id"": ""d2372922-b18e-4a61-b2a0-d277ffa73894"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Lean Left  Pressed"",
                    ""type"": ""Button"",
                    ""id"": ""4cc887fc-1ba6-4a4c-99aa-15d568fb5fa5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Lean Left Released"",
                    ""type"": ""Button"",
                    ""id"": ""2da901e8-2737-40e1-b882-f99d16b7935d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Lean Right Pressed"",
                    ""type"": ""Button"",
                    ""id"": ""ef538f26-21ea-45f5-9c1d-4b7fb16bfd64"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Lean Right Released"",
                    ""type"": ""Button"",
                    ""id"": ""fa2ccfc1-3602-4990-9ad8-af2d08bbba57"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Mouse Scroll Wheel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""c0caca30-9df8-46ab-b247-4f3f5f72cb4e"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ce68a780-d97b-45c6-8907-8c613a0641c5"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf7022b4-3a8e-48f8-afc8-dd59b3c14bf5"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""4b79ecaf-6538-4554-b95c-c504e23c14eb"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""2c3dbf64-e43c-44cb-b115-8cf02a860810"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d2c89995-bdda-4b3a-9c05-fa2e568c7552"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""649f9650-01e9-499c-adc7-2148c0d34af4"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""25f77dc0-9f92-4266-b2e8-d976a7a5ab55"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0a7f62c6-b8a3-4683-8108-3c0be7653204"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d58fd547-3991-4573-8b71-be8dd7c5d1f7"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e788315e-cf1a-4731-a443-61caaab0012e"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""61c409bb-4c49-4aab-8ffd-9e3ba5e96091"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": ""Tap,Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3522c66b-c5e9-42c2-ac21-9679fe5ecae4"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": ""Tap"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d571d15f-083d-42fd-88a7-77726350f331"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Prone"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7049999d-1e60-4f5b-9a48-e5d1eb00720d"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0612e992-8a7c-42ad-9642-89f6f00c8723"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""95687e9b-7912-4850-b03f-3817920499ce"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint Released"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9cd65a75-3ef9-420d-948c-26afb57475d1"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint Released"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be0e3edd-6e63-4fec-8aa5-ddb5bef18e3d"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean Left  Pressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d8b44356-47a8-4a85-b466-d9aef6e6fa94"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean Right Pressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""86182693-f648-4681-9f8a-4091705fb455"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean Right Released"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""14e125c5-0a41-48ef-b21b-448555040e9a"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lean Left Released"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e096157-8b6d-4c1d-86f6-a244f1fbcb38"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse Scroll Wheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Weapon"",
            ""id"": ""26644887-cf3d-4160-a620-f7dc088bf3ff"",
            ""actions"": [
                {
                    ""name"": ""AimPressed"",
                    ""type"": ""Button"",
                    ""id"": ""0395b840-2ada-4310-b18a-d0a6e43abb94"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AimReleased"",
                    ""type"": ""Button"",
                    ""id"": ""2c2438bd-d334-4345-886a-67812c761e8f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""84814e67-2c27-4870-a4fd-d9dfe57901b4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3549392c-67e5-4ef0-b3ab-8e75da51ecb6"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f224b4e5-e345-47fd-a6b0-10e9ec0cfa35"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimPressed"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7240ff7e-a671-4b3e-a142-c50b496f2cd2"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimReleased"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""917d8a92-dc8e-487c-ad6a-515b022910ea"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimReleased"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Camera = m_Player.FindAction("Camera", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Crouch = m_Player.FindAction("Crouch", throwIfNotFound: true);
        m_Player_Prone = m_Player.FindAction("Prone", throwIfNotFound: true);
        m_Player_Sprint = m_Player.FindAction("Sprint", throwIfNotFound: true);
        m_Player_SprintReleased = m_Player.FindAction("Sprint Released", throwIfNotFound: true);
        m_Player_LeanLeftPressed = m_Player.FindAction("Lean Left  Pressed", throwIfNotFound: true);
        m_Player_LeanLeftReleased = m_Player.FindAction("Lean Left Released", throwIfNotFound: true);
        m_Player_LeanRightPressed = m_Player.FindAction("Lean Right Pressed", throwIfNotFound: true);
        m_Player_LeanRightReleased = m_Player.FindAction("Lean Right Released", throwIfNotFound: true);
        m_Player_MouseScrollWheel = m_Player.FindAction("Mouse Scroll Wheel", throwIfNotFound: true);
        // Weapon
        m_Weapon = asset.FindActionMap("Weapon", throwIfNotFound: true);
        m_Weapon_AimPressed = m_Weapon.FindAction("AimPressed", throwIfNotFound: true);
        m_Weapon_AimReleased = m_Weapon.FindAction("AimReleased", throwIfNotFound: true);
        m_Weapon_Shoot = m_Weapon.FindAction("Shoot", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Camera;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Crouch;
    private readonly InputAction m_Player_Prone;
    private readonly InputAction m_Player_Sprint;
    private readonly InputAction m_Player_SprintReleased;
    private readonly InputAction m_Player_LeanLeftPressed;
    private readonly InputAction m_Player_LeanLeftReleased;
    private readonly InputAction m_Player_LeanRightPressed;
    private readonly InputAction m_Player_LeanRightReleased;
    private readonly InputAction m_Player_MouseScrollWheel;
    public struct PlayerActions
    {
        private @DefaultInput m_Wrapper;
        public PlayerActions(@DefaultInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Camera => m_Wrapper.m_Player_Camera;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Crouch => m_Wrapper.m_Player_Crouch;
        public InputAction @Prone => m_Wrapper.m_Player_Prone;
        public InputAction @Sprint => m_Wrapper.m_Player_Sprint;
        public InputAction @SprintReleased => m_Wrapper.m_Player_SprintReleased;
        public InputAction @LeanLeftPressed => m_Wrapper.m_Player_LeanLeftPressed;
        public InputAction @LeanLeftReleased => m_Wrapper.m_Player_LeanLeftReleased;
        public InputAction @LeanRightPressed => m_Wrapper.m_Player_LeanRightPressed;
        public InputAction @LeanRightReleased => m_Wrapper.m_Player_LeanRightReleased;
        public InputAction @MouseScrollWheel => m_Wrapper.m_Player_MouseScrollWheel;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Camera.started += instance.OnCamera;
            @Camera.performed += instance.OnCamera;
            @Camera.canceled += instance.OnCamera;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Crouch.started += instance.OnCrouch;
            @Crouch.performed += instance.OnCrouch;
            @Crouch.canceled += instance.OnCrouch;
            @Prone.started += instance.OnProne;
            @Prone.performed += instance.OnProne;
            @Prone.canceled += instance.OnProne;
            @Sprint.started += instance.OnSprint;
            @Sprint.performed += instance.OnSprint;
            @Sprint.canceled += instance.OnSprint;
            @SprintReleased.started += instance.OnSprintReleased;
            @SprintReleased.performed += instance.OnSprintReleased;
            @SprintReleased.canceled += instance.OnSprintReleased;
            @LeanLeftPressed.started += instance.OnLeanLeftPressed;
            @LeanLeftPressed.performed += instance.OnLeanLeftPressed;
            @LeanLeftPressed.canceled += instance.OnLeanLeftPressed;
            @LeanLeftReleased.started += instance.OnLeanLeftReleased;
            @LeanLeftReleased.performed += instance.OnLeanLeftReleased;
            @LeanLeftReleased.canceled += instance.OnLeanLeftReleased;
            @LeanRightPressed.started += instance.OnLeanRightPressed;
            @LeanRightPressed.performed += instance.OnLeanRightPressed;
            @LeanRightPressed.canceled += instance.OnLeanRightPressed;
            @LeanRightReleased.started += instance.OnLeanRightReleased;
            @LeanRightReleased.performed += instance.OnLeanRightReleased;
            @LeanRightReleased.canceled += instance.OnLeanRightReleased;
            @MouseScrollWheel.started += instance.OnMouseScrollWheel;
            @MouseScrollWheel.performed += instance.OnMouseScrollWheel;
            @MouseScrollWheel.canceled += instance.OnMouseScrollWheel;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Camera.started -= instance.OnCamera;
            @Camera.performed -= instance.OnCamera;
            @Camera.canceled -= instance.OnCamera;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Crouch.started -= instance.OnCrouch;
            @Crouch.performed -= instance.OnCrouch;
            @Crouch.canceled -= instance.OnCrouch;
            @Prone.started -= instance.OnProne;
            @Prone.performed -= instance.OnProne;
            @Prone.canceled -= instance.OnProne;
            @Sprint.started -= instance.OnSprint;
            @Sprint.performed -= instance.OnSprint;
            @Sprint.canceled -= instance.OnSprint;
            @SprintReleased.started -= instance.OnSprintReleased;
            @SprintReleased.performed -= instance.OnSprintReleased;
            @SprintReleased.canceled -= instance.OnSprintReleased;
            @LeanLeftPressed.started -= instance.OnLeanLeftPressed;
            @LeanLeftPressed.performed -= instance.OnLeanLeftPressed;
            @LeanLeftPressed.canceled -= instance.OnLeanLeftPressed;
            @LeanLeftReleased.started -= instance.OnLeanLeftReleased;
            @LeanLeftReleased.performed -= instance.OnLeanLeftReleased;
            @LeanLeftReleased.canceled -= instance.OnLeanLeftReleased;
            @LeanRightPressed.started -= instance.OnLeanRightPressed;
            @LeanRightPressed.performed -= instance.OnLeanRightPressed;
            @LeanRightPressed.canceled -= instance.OnLeanRightPressed;
            @LeanRightReleased.started -= instance.OnLeanRightReleased;
            @LeanRightReleased.performed -= instance.OnLeanRightReleased;
            @LeanRightReleased.canceled -= instance.OnLeanRightReleased;
            @MouseScrollWheel.started -= instance.OnMouseScrollWheel;
            @MouseScrollWheel.performed -= instance.OnMouseScrollWheel;
            @MouseScrollWheel.canceled -= instance.OnMouseScrollWheel;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Weapon
    private readonly InputActionMap m_Weapon;
    private List<IWeaponActions> m_WeaponActionsCallbackInterfaces = new List<IWeaponActions>();
    private readonly InputAction m_Weapon_AimPressed;
    private readonly InputAction m_Weapon_AimReleased;
    private readonly InputAction m_Weapon_Shoot;
    public struct WeaponActions
    {
        private @DefaultInput m_Wrapper;
        public WeaponActions(@DefaultInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @AimPressed => m_Wrapper.m_Weapon_AimPressed;
        public InputAction @AimReleased => m_Wrapper.m_Weapon_AimReleased;
        public InputAction @Shoot => m_Wrapper.m_Weapon_Shoot;
        public InputActionMap Get() { return m_Wrapper.m_Weapon; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(WeaponActions set) { return set.Get(); }
        public void AddCallbacks(IWeaponActions instance)
        {
            if (instance == null || m_Wrapper.m_WeaponActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_WeaponActionsCallbackInterfaces.Add(instance);
            @AimPressed.started += instance.OnAimPressed;
            @AimPressed.performed += instance.OnAimPressed;
            @AimPressed.canceled += instance.OnAimPressed;
            @AimReleased.started += instance.OnAimReleased;
            @AimReleased.performed += instance.OnAimReleased;
            @AimReleased.canceled += instance.OnAimReleased;
            @Shoot.started += instance.OnShoot;
            @Shoot.performed += instance.OnShoot;
            @Shoot.canceled += instance.OnShoot;
        }

        private void UnregisterCallbacks(IWeaponActions instance)
        {
            @AimPressed.started -= instance.OnAimPressed;
            @AimPressed.performed -= instance.OnAimPressed;
            @AimPressed.canceled -= instance.OnAimPressed;
            @AimReleased.started -= instance.OnAimReleased;
            @AimReleased.performed -= instance.OnAimReleased;
            @AimReleased.canceled -= instance.OnAimReleased;
            @Shoot.started -= instance.OnShoot;
            @Shoot.performed -= instance.OnShoot;
            @Shoot.canceled -= instance.OnShoot;
        }

        public void RemoveCallbacks(IWeaponActions instance)
        {
            if (m_Wrapper.m_WeaponActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IWeaponActions instance)
        {
            foreach (var item in m_Wrapper.m_WeaponActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_WeaponActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public WeaponActions @Weapon => new WeaponActions(this);
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnProne(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnSprintReleased(InputAction.CallbackContext context);
        void OnLeanLeftPressed(InputAction.CallbackContext context);
        void OnLeanLeftReleased(InputAction.CallbackContext context);
        void OnLeanRightPressed(InputAction.CallbackContext context);
        void OnLeanRightReleased(InputAction.CallbackContext context);
        void OnMouseScrollWheel(InputAction.CallbackContext context);
    }
    public interface IWeaponActions
    {
        void OnAimPressed(InputAction.CallbackContext context);
        void OnAimReleased(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
    }
}
