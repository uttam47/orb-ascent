using System;
using UnityEngine.UIElements;

namespace AnalyticalApproach.OrbAscent
{
    internal class CameraSettingsUIView : UIView
    {
        private CameraSettings _cameraSettings;

        private Button _invertYAxis;
        private Button _invertXaxis;
        private Slider _tppCamSensitivity;
        private EnumField _trackedCameraControlType;
        private Slider _trackedCameraSensitivity;
        private CameraEventsChannel _cameraEventChannel; 
       

        public CameraSettingsUIView(VisualElement visualElementRoot, CameraSettings cameraSettings) : base(visualElementRoot)
        {
            _cameraSettings = cameraSettings;
            _cameraEventChannel = GameEventManager.GetEventChannel<CameraEventsChannel>(); 
            Init();
        }

        private void Init()
        {
            _invertXaxis = root.Q<Button>("InvertXAxisButton");
            _invertYAxis = root.Q<Button>("InvertYAxisButton");
            _trackedCameraControlType = root.Q<EnumField>("TrackCameraControlType");

            _tppCamSensitivity = root.Q<Slider>("TPPCameraSensitivitySlider");
            _trackedCameraSensitivity = root.Q<Slider>("TrackedCameraSensitivitySlider");

            _trackedCameraSensitivity.SetValueWithoutNotify(_cameraSettings.trackedCamSpeed);
            _tppCamSensitivity.SetValueWithoutNotify(_cameraSettings.tppCamSpeed);
            _trackedCameraControlType.Init(_cameraSettings.trackedCameraControlType); 

            _tppCamSensitivity.RegisterValueChangedCallback(OnTppCamSensitivityChanged);
            _trackedCameraSensitivity.RegisterValueChangedCallback(OnTrackedCamSensitivityChanged); 
            _invertXaxis.clicked += InvertXAxisButtonClicked;
            _invertYAxis.clicked += InvertYAxisButtonClicked;
            _trackedCameraControlType.RegisterValueChangedCallback(OnTrackCamControlTypeChanged);

            _invertXaxis.RemoveFromClassList(TOGGLE_ON_CLASS);
            _invertYAxis.RemoveFromClassList(TOGGLE_ON_CLASS);
            _trackedCameraControlType.RemoveFromClassList(TOGGLE_ON_CLASS); 

            _invertXaxis.AddToClassList(_cameraSettings.invertXAxis == -1 ? TOGGLE_ON_CLASS: TOGGLE_OFF_CLASS); 
            _invertYAxis.AddToClassList(_cameraSettings.invertYAxis == -1 ? TOGGLE_ON_CLASS: TOGGLE_OFF_CLASS);

        }

        private void OnTrackCamControlTypeChanged(ChangeEvent<Enum> evt)
        {
            _cameraSettings.trackedCameraControlType = (TrackedCameraControlType)evt.newValue;
            _cameraEventChannel.RaiseCameraSettingsUpdated();
        }

        private void OnTrackedCamSensitivityChanged(ChangeEvent<float> evt)
        {
            _cameraSettings.trackedCamSpeed = evt.newValue;
        }

        private void OnTppCamSensitivityChanged(ChangeEvent<float> evt)
        {
            _cameraSettings.tppCamSpeed = evt.newValue; 
        }

        private void InvertYAxisButtonClicked()
        {
            _cameraSettings.invertYAxis *= -1;
            _invertYAxis.RemoveFromClassList(TOGGLE_OFF_CLASS); 
            _invertYAxis.RemoveFromClassList(TOGGLE_ON_CLASS); 
            _invertYAxis.AddToClassList(_cameraSettings.invertYAxis == -1 ? TOGGLE_ON_CLASS: TOGGLE_OFF_CLASS);
        }

        private void InvertXAxisButtonClicked()
        {
            _cameraSettings.invertXAxis *= -1;
            _invertXaxis.RemoveFromClassList(TOGGLE_ON_CLASS); 
            _invertXaxis.RemoveFromClassList(TOGGLE_OFF_CLASS); 
            _invertXaxis.AddToClassList(_cameraSettings.invertXAxis == -1 ? TOGGLE_ON_CLASS: TOGGLE_OFF_CLASS); 
        }


    }
}
