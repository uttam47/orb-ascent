using Unity.Cinemachine;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{

    public class CinemachineBrainTracker : MonoBehaviour
    {
        private CinemachineBrain _cinemachineBrain;
        private CameraEventsChannel _cameraEventsChannel;
        [SerializeField] private bool isBlending; 

        private void Awake()
        {

            _cinemachineBrain = GetComponent<CinemachineBrain>();
            _cameraEventsChannel = GameEventManager.GetEventChannel<CameraEventsChannel>(); 

        }

        private void Update()
        {
            if( _cinemachineBrain.IsBlending != isBlending)
            {
                isBlending = _cinemachineBrain.IsBlending;
                _cameraEventsChannel.RaiseCameraBlendStateChanged(isBlending); 

            }
        }
    }
}
