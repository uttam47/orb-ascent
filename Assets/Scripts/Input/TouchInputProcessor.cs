using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace AnalyticalApproach.OrbAscent
{
    public class TouchInputProcessor : InputProcessor
    {
        private List<RectTransform> maskingObjects;
        private Player_ActionMap.PlayableInputTouchActions _playableInputTouch;
        private Dictionary<int, bool> _touchesStartedOnMaskedObject; 

        protected override void Awake()
        {
            base.Awake();
            _touchesStartedOnMaskedObject = new Dictionary<int, bool>();    
            maskingObjects = new List<RectTransform>();

            RectTransform[] allRects = FindObjectsOfType<RectTransform>(true);

            foreach (RectTransform rect in allRects)
            {
                if (ShouldBeMasked(rect))
                {
                    maskingObjects.Add(rect);
                }
            }
        }

        private void OnEnable()
        {
            _playableInputTouch.Enable();
        }

        private void OnDisable()
        {
            _playableInputTouch.Disable();
        }


        #region MASKING_LOGIC

        private bool ShouldBeMasked(RectTransform rect)
        {
            return rect.gameObject.CompareTag("TouchMask");
        }

        private bool IsTouchOnMaskedObject(Vector2 touchPosition)
        {
            foreach (RectTransform rect in maskingObjects)
            {
                if (!rect.gameObject.activeInHierarchy)
                {
                    continue;
                }
                if (IsTouchInsideRect(rect, touchPosition))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsTouchInsideRect(RectTransform rect, Vector2 touchPosition)
        {
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, touchPosition, null, out localPoint);

            return rect.rect.Contains(localPoint);
        }
        #endregion


        public override void SetInputActionMap(Player_ActionMap inputActions)
        {
            base.inputActions = inputActions;
            SetPlayerTouchEventActions(base.inputActions.PlayableInputTouch);
        }


        #region PLAYBLE_INPUTS_TOUCH

        private void SetPlayerTouchEventActions(Player_ActionMap.PlayableInputTouchActions playableInputTouch)
        {
            _playableInputTouch = playableInputTouch;

            _playableInputTouch.SelectAction1.performed += OnTouchPerformed;
            _playableInputTouch.SelectAction2.performed += OnTouchPerformed;
            _playableInputTouch.SelectAction3.performed += OnTouchPerformed;

            _playableInputTouch.Move.performed += OnMovePerforemdViaTouch;
            _playableInputTouch.Move.canceled += OnMovePerforemdViaTouch;
        }

        private void OnTouchPerformed(InputAction.CallbackContext context)
        {
            TouchState touchState = context.ReadValue<TouchState>();

            if(touchState.phase == TouchPhase.Began)
            {
                _touchesStartedOnMaskedObject[touchState.touchId] = IsTouchOnMaskedObject(touchState.position);
            }

            bool takeAction = _touchesStartedOnMaskedObject.ContainsKey(touchState.touchId) && !_touchesStartedOnMaskedObject[touchState.touchId]; 

            if (takeAction)
            {
                Vector2 delta = touchState.delta;
                playerEventChannel.RaisePlayerTurnEvent(delta);
                cameraEventsChannel.RaiseMoveTrackCamera(touchState.position); 
            }

            if (TouchPhase.Ended == touchState.phase || TouchPhase.Canceled == touchState.phase)
            {
                OnTouchEnded(touchState, takeAction);
            }
        }

        private void OnTouchEnded(TouchState touchState, bool takeAction)
        {

            if (takeAction)
            {
                selectionEventChannel.RaiseSelectActionPerformed(touchState.position);
                playerEventChannel.RaisePlayerTurnEvent(Vector2.zero);
                cameraEventsChannel.RaiseStopTrackCamera(); 
            }

            if (_touchesStartedOnMaskedObject.ContainsKey(touchState.touchId))
            {
                _touchesStartedOnMaskedObject.Remove(touchState.touchId);
            }
        }

        private void OnMovePerforemdViaTouch(InputAction.CallbackContext context)
        {
            Vector2 moveDirection = context.ReadValue<Vector2>();
            playerEventChannel.RaisePlayerMoveEvent(moveDirection);

        }

        #endregion


    }
}