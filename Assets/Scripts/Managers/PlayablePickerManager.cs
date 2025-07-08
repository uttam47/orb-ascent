using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace AnalyticalApproach.OrbAscent
{
    public class PlayablePickerManager : MonoBehaviour
    {
        public List<PlaybleUnit> selectedObjects;
        private SelectionEventChannel _selectionEventChannel;
        private CameraEventsChannel _cameraEventsChannel;
        private PlayerEventChannel _playerEventChannel;
        private bool _pickingAllowed;
        private int selectionLayerMask;
        private Camera _mainCamera; 

        [SerializeField] private PlaybleUnit startingUnit;

        void Awake()
        {
            selectionLayerMask = LayerMask.GetMask("Playables"); 
            selectedObjects = new List<PlaybleUnit>();
            _selectionEventChannel = GameEventManager.GetEventChannel<SelectionEventChannel>();
            _cameraEventsChannel = GameEventManager.GetEventChannel<CameraEventsChannel>();
            _playerEventChannel = GameEventManager.GetEventChannel<PlayerEventChannel>();
            _pickingAllowed = true;
            _mainCamera = Camera.main; 
        }

        private void Start()
        {
            StartCoroutine(SelectDefaultUnit()); 
        }

        private IEnumerator SelectDefaultUnit()
        {
            yield return new WaitForEndOfFrame();
            if (startingUnit != null)
            {
                SelectUnit(startingUnit.gameObject);
                _playerEventChannel.RaiseGameModeUpdated(GameMode.PlayableSelected);
            }
        }

        private void OnEnable()
        {
            _selectionEventChannel.OnSelectActionPerformed += Select;
            _selectionEventChannel.OnSelectActionPerformedWithScreenPoint += Select;
            _selectionEventChannel.OnDeselectActionPerformed += Deselect;
            _cameraEventsChannel.OnCameraBlendStateChanged += OnCameraBlendingStateChanged;
        }

        private void OnDisable()
        {
            _selectionEventChannel.OnSelectActionPerformed -= Select;
            _selectionEventChannel.OnSelectActionPerformedWithScreenPoint -= Select;
            _selectionEventChannel.OnDeselectActionPerformed -= Deselect;
            _cameraEventsChannel.OnCameraBlendStateChanged -= OnCameraBlendingStateChanged;
        }

        private void Select(Vector2 vector)
        {
            Ray ray = _mainCamera.ScreenPointToRay(vector);
            Debug.DrawLine(ray.origin, ray.direction * 1000, Color.red, 30); 
            SelectUsingRay(ray);
        }

        private void Select()
        {
            Ray ray = new Ray(_mainCamera.transform.position, _mainCamera.transform.forward);
            SelectUsingRay(ray);
        }

        private void Deselect()
        {
            selectedObjects.RemoveAll(ClearPlaybleUnit);
        }

        private void OnCameraBlendingStateChanged(bool value)
        {
            _pickingAllowed = !value;
        }

        private bool ClearPlaybleUnit(PlaybleUnit playbleUnit)
        {
            if (playbleUnit.CanBeDeselected())
            {
                //NOTE: We currently have single object selected all the time, so it's okay 
                // send it as the base object. If there are more than on object, then there collective
                // center must be sent for the camera position. 
                _cameraEventsChannel.RaiseTrackedCameraPlacement(playbleUnit.transform.position);
                playbleUnit.OnDeselected();
                _playerEventChannel.RaiseGameModeUpdated(GameMode.InspectMode); 
                return true;
            }

            return false;
        }

        private void SelectUsingRay(Ray ray)
        {
            if (selectedObjects.Count > 0 || !_pickingAllowed)
            {
                return;
            }

            RaycastHit hit;
            bool hitSomething = Physics.Raycast(ray, out hit, 100.0f, selectionLayerMask, QueryTriggerInteraction.Ignore);
            if (hitSomething)
            {
                SelectUnit(hit.transform.gameObject);
            }
        }

        private void SelectUnit(GameObject playbleUnitObject)
        {
            foreach (PlaybleUnit playbleUnit in selectedObjects)
            {
                playbleUnit.GetComponent<PlaybleUnit>().OnDeselected();
            }

            PlaybleUnit selectable = playbleUnitObject.GetComponent<PlaybleUnit>();

            if (!selectable.CanBeSelected())
            {
                return;
            }

            selectedObjects.Clear();
            selectable.OnSelected();
            selectedObjects.Add(selectable);
            _playerEventChannel.RaiseGameModeUpdated(GameMode.PlayableSelected); 
        }
    }
}