using UnityEngine;
using Unity.Cinemachine;
using Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Splines;
using Unity.Mathematics;

namespace AnalyticalApproach.OrbAscent
{
    public class TrackedCameraController : MonoBehaviour
    {
        private const float DAMP_VALUE = 100f;
        
        private PriorityQueue<float, Transform> _priorityDistanceQueue;
        private List<Transform> _players;
        private Transform _mainCamera;
        private Transform _closestPlayer;
        private Vector2 _touchStartedPosition = Vector2.zero; 
        private Vector2 _moveDelta;
        [SerializeField]private CameraSettings cameraSettings;  

        private PlayerEventChannel _playerEventChannel;
        private CameraEventsChannel _cameraEventChannel;
        
        [SerializeField] private CinemachineCamera targetCamera;
        [SerializeField] private CinemachineSplineDolly trackedDolly;
        [SerializeField] private Transform lookTarget;
        [SerializeField] private Transform upAnchor;
        [SerializeField] private Transform downAnchor;
        [SerializeField] private float currentDistance;

        public void Awake()
        {
            _priorityDistanceQueue = new PriorityQueue<float, Transform>();
            _mainCamera = Camera.main.transform;

            _playerEventChannel = GameEventManager.GetEventChannel<PlayerEventChannel>();
            _cameraEventChannel = GameEventManager.GetEventChannel<CameraEventsChannel>();

            _cameraEventChannel.OnCameraBlendStateChanged += OnCameraBlendingStateChanged;
            _cameraEventChannel.PlaceTrackedCameraNearestToPosition += OnSetCameraPosition;
            _playerEventChannel.OnGameModeUpdate += OnGameModeUpdated;

            currentDistance =trackedDolly.CameraPosition; 
            FindPlayables();
            SetAnchors();
        }

        private void SetAnchors()
        {
            upAnchor.gameObject.SetActive(false);
            downAnchor.gameObject.SetActive(false);

            upAnchor.SetParent(null);
            downAnchor.SetParent(null);
        }

        private void OnGameModeUpdated(GameMode mode)
        {
            if (mode == GameMode.InspectMode)
            {
                gameObject.SetActive(true);
            }
            else if (mode == GameMode.PlayableSelected)
            {
                gameObject.SetActive(false);
            }
        }

        private void FindPlayables()
        {
            _players = FindObjectsOfType<PlayerController>()
                    .Select(pc => pc.transform)
                    .ToList();
        }

        private void OnSetCameraPosition(Vector3 position)
        {
            position = trackedDolly.Spline.transform.InverseTransformPoint(position);
            SplineUtility.GetNearestPoint(trackedDolly.Spline.Spline, (float3)position, out float3 nearestPointOnSpline, out float normalizedPathPosition);
            currentDistance = normalizedPathPosition;
        }

        private void OnCameraBlendingStateChanged(bool value) { }

        private void OnEnable()
        {
            _playerEventChannel.OnPlayerMove += OnMove;
            _cameraEventChannel.OnMoveTrackCamera += MoveThroughTouch;
            _cameraEventChannel.OnStopTrackCamera += StopCameraMovement;
        }

        private void OnDisable()
        {
            _closestPlayer = null;

            _playerEventChannel.OnPlayerMove -= OnMove;
            _cameraEventChannel.OnMoveTrackCamera -= MoveThroughTouch;
            _cameraEventChannel.OnStopTrackCamera -= StopCameraMovement;
            _moveDelta = Vector2.zero;
        }

        private void StopCameraMovement()
        {
            _moveDelta = Vector2.zero;
            _touchStartedPosition = Vector2.zero;   
        }

        private void MoveThroughTouch(Vector2 currentTouchPosition)
        {
            if(cameraSettings.trackedCameraControlType != TrackedCameraControlType.Touch)
            {
                return; 
            }

            if (_touchStartedPosition == Vector2.zero)
            {
                _touchStartedPosition = currentTouchPosition;
            }

            Vector2 delta = currentTouchPosition - _touchStartedPosition;
            Vector2 normalizedDelta = delta / 150f; //This 150 is same as Joystick movement range. 
            _moveDelta = Vector2.ClampMagnitude(normalizedDelta, 1f);
        }

        private void OnMove(Vector2 moveDelta)
        {
            if (cameraSettings.trackedCameraControlType != TrackedCameraControlType.Joystick)
            {
                return;
            }
            _moveDelta = moveDelta;
        }

        private void LateUpdate()
        {
            MoveThroughJoyStick();
            SetNewTarget();
        }

        private void SetNewTarget()
        {
            Transform nearestPlayer = FindClosestPlayer();

            if (_closestPlayer != nearestPlayer)
            {
                lookTarget.position = nearestPlayer.position;
                _closestPlayer = nearestPlayer;
            }
        }

        private Transform FindClosestPlayer()
        {
            _priorityDistanceQueue.Clear();

            PriorityQueue<Transform, float> priorityQueue = new PriorityQueue<Transform, float>();

            foreach (Transform target in _players)
            {
                float distance = Vector3.Distance(_mainCamera.position, target.position);
                priorityQueue.Enqueue(target, distance);
            }

            return priorityQueue.Dequeue();
        }

        private void MoveThroughJoyStick()
        {
            if (!enabled)
            {
                return;
            }

            currentDistance += -_moveDelta.x * cameraSettings.trackedCamSpeed * Time.deltaTime / DAMP_VALUE;

            if (!trackedDolly.Spline.Spline.Closed)
            {
                currentDistance = Mathf.Clamp(currentDistance, 0, 1);
            }
            else
            {
                currentDistance = Mathf.Repeat(currentDistance, 1);
            }

            trackedDolly.CameraPosition = currentDistance;

            Vector3 newPosition = trackedDolly.Spline.transform.position;
            newPosition.y += _moveDelta.y * cameraSettings.trackedCamSpeed * Time.deltaTime;

            newPosition.y = Mathf.Clamp(newPosition.y, downAnchor.position.y, upAnchor.position.y);
            trackedDolly.Spline.transform.position = newPosition;
        }

        private void OnDestroy()
        {
            _cameraEventChannel.OnCameraBlendStateChanged -= OnCameraBlendingStateChanged;
            _cameraEventChannel.PlaceTrackedCameraNearestToPosition -= OnSetCameraPosition;
            _playerEventChannel.OnGameModeUpdate -= OnGameModeUpdated;
        }
    }
}
