using System;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    public class SelectionEventChannel : EventChannel
    {
        public event Action OnSelectActionPerformed;
        public void RaiseSelectActionPerformed()
        {
            OnSelectActionPerformed?.Invoke();
        }

        public event Action<Vector2> OnSelectActionPerformedWithScreenPoint; 
        public void RaiseSelectActionPerformed(Vector2 screenPoint)
        {
            OnSelectActionPerformedWithScreenPoint?.Invoke(screenPoint); 
        }

        public event Action OnDeselectActionPerformed;
        public void RaiseDeselectActionPerformed()
        {
            OnDeselectActionPerformed?.Invoke();
        }

        public override void ResetEvents()
        {
            OnDeselectActionPerformed = null;
            OnSelectActionPerformed = null;
            OnSelectActionPerformedWithScreenPoint = null; 
        }
    }
}
