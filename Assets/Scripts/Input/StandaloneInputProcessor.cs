using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AnalyticalApproach.OrbAscent
{
    public class StandaloneInputProcessor: InputProcessor
    {
        Player_ActionMap.StrategyInputsActions _strategyActions; 
        Player_ActionMap.PlayableInputStandaloneActions _playbleActions; 

        public override void SetInputActionMap(Player_ActionMap inputActions)
        {
            base.inputActions = inputActions;

            SetPlayerStandaloneEventActions(base.inputActions.PlayableInputStandalone);
            SetStrategyEventActions(base.inputActions.StrategyInputs);
        }

        private void OnEnable()
        {
            _strategyActions.Enable();
            _playbleActions.Enable(); 
        }

        private void OnDisable()
        {
            _strategyActions.Disable();
            _playbleActions.Disable();
        }

        #region STRATEGY_INPUTS
        private void SetStrategyEventActions(Player_ActionMap.StrategyInputsActions strategyActions)
        {
            _strategyActions = strategyActions; 
            _strategyActions.Select.performed += OnSelectActionPerformed;
            _strategyActions.Deselect.performed += OnDeslectActionPerformed;
        }

        private void OnSelectActionPerformed(InputAction.CallbackContext context)
        {
            selectionEventChannel.RaiseSelectActionPerformed();
        }

        private void OnDeslectActionPerformed(InputAction.CallbackContext context)
        {
            selectionEventChannel.RaiseDeselectActionPerformed();
        }

        #endregion

        #region PLAYBLE_INPUTS_STANDALONE

        private void SetPlayerStandaloneEventActions(Player_ActionMap.PlayableInputStandaloneActions playbleActions)
        {
            _playbleActions = playbleActions; 
            _playbleActions.Jump.performed += OnJumpActionPerformed;
            
            _playbleActions.Move.performed += OnMoveActionPerformed;
            _playbleActions.Move.canceled += OnMoveCancelPerformed;
            
            _playbleActions.Look.performed += OnTurnActionPerformed;
            _playbleActions.Look.canceled += OnTurnActionPerformed;
        }

        private void OnMoveCancelPerformed(InputAction.CallbackContext context)
        {
            playerEventChannel.RaisePlayerMoveEvent(Vector3.zero);
        }

        private void OnTurnActionPerformed(InputAction.CallbackContext context)
        {
            Vector2 turnDelta = context.ReadValue<Vector2>();
            playerEventChannel.RaisePlayerTurnEvent(turnDelta);
        }

        private void OnMoveActionPerformed(InputAction.CallbackContext context)
        {
            Vector2 moveDirection = context.ReadValue<Vector2>();
            playerEventChannel.RaisePlayerMoveEvent(moveDirection);
        }

        private void OnJumpActionPerformed(InputAction.CallbackContext context)
        {
            playerEventChannel.RaisePlayerJumpEvent(true);
        }

        #endregion
    }
}
