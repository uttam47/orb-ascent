using System;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    public class CameraEventsChannel : EventChannel
    {
        public event Action<bool> OnCameraBlendStateChanged; 
        public void RaiseCameraBlendStateChanged(bool value)
        {
            OnCameraBlendStateChanged?.Invoke(value);   
        }

        public event Action<Vector3> PlaceTrackedCameraNearestToPosition; 
        public void RaiseTrackedCameraPlacement(Vector3 lookAtPosition)
        {
            PlaceTrackedCameraNearestToPosition?.Invoke(lookAtPosition);   
        }

        public event Action<Vector2> OnMoveTrackCamera; 
        public void RaiseMoveTrackCamera(Vector2 moveDelta)
        {
            OnMoveTrackCamera?.Invoke(moveDelta);
        }

        public event Action OnStopTrackCamera; 
        public void RaiseStopTrackCamera()
        {
            OnStopTrackCamera?.Invoke();
        }

        public event Action OnCameraSettingsUpdated;

        public void RaiseCameraSettingsUpdated()
        {
            OnCameraSettingsUpdated?.Invoke();  
        }

        public override void ResetEvents()
        {
            OnCameraBlendStateChanged = null;
            PlaceTrackedCameraNearestToPosition = null; 
        }
    }
}