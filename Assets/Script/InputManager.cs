using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Events;

namespace PunchHero
{
    [DefaultExecutionOrder(-1)]
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;
        public event UnityAction<Vector2> OnTouchPressed;

        private TouchControl touchControl;

        private void Awake()
        {
            touchControl = new TouchControl();
            

            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance == this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
        private void OnEnable()
        {
            touchControl.Enable();

            //EnhancedTouchSupport.Enable();
        }
        private void OnDisable()
        {
            touchControl.Disable();

            //EnhancedTouchSupport.Disable();
        }
        private void Start()
        {
            touchControl.Touch.TouchPress.started += ctx => TouchInput_started(ctx);
            //UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
        }

        private void TouchInput_started(InputAction.CallbackContext context)
        {
            //Debug.Log("position: " + touchControl.Touch.TouchPosition.ReadValue<Vector2>());  

            OnTouchPressed?.Invoke(touchControl.Touch.TouchPosition.ReadValue<Vector2>());
        }
        //private void FingerDown(Finger finger)
        //{
        //    OnTouchPressed?.Invoke(finger.screenPosition);
        //}
    }
}
