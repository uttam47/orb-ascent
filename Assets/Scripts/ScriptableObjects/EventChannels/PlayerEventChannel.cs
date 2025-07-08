using System;
using UnityEngine;
using UnityEngine.Events;

namespace AnalyticalApproach.OrbAscent
{
    public class PlayerEventChannel : EventChannel
    {
        public event UnityAction<Vector2> OnPlayerMove; 
        public void RaisePlayerMoveEvent(Vector2 moveDirection)
        {
            OnPlayerMove?.Invoke(moveDirection); 
        }

        public event UnityAction<Vector2> OnPlayerTurn;
        public void RaisePlayerTurnEvent(Vector2 turnDelta)
        {
            OnPlayerTurn?.Invoke(turnDelta); 
        }

        public event UnityAction<bool> OnPlayerJump; 
        public void RaisePlayerJumpEvent(bool value)
        {
            OnPlayerJump?.Invoke(value); 
        }

        public event Action<GameMode> OnGameModeUpdate;
        public void RaiseGameModeUpdated(GameMode gameModeScreen)
        {
            OnGameModeUpdate?.Invoke(gameModeScreen);
        }

        public override void ResetEvents()
        {
            OnPlayerJump = null;
            OnPlayerMove = null;
            OnPlayerTurn = null;
        }
    }
}
