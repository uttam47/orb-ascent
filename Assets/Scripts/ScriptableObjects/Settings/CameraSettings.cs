using UnityEngine;


namespace AnalyticalApproach.OrbAscent
{
    [CreateAssetMenu(fileName = nameof(CameraSettings), menuName = "OrbAscent/" + nameof(CameraSettings))]
    public class CameraSettings : Settings
    {
        public float tppCamSpeed;
        public float trackedCamSpeed;
        public int invertXAxis;
        public int invertYAxis;
        public Vector2 axisWeigts;
        public TrackedCameraControlType trackedCameraControlType; 
    }

}