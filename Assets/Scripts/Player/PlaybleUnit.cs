using UnityEngine;
using System;
using System.Collections;

namespace AnalyticalApproach.OrbAscent
{
    public class PlaybleUnit : MonoBehaviour, ISelectable
    {
        [Range(1, 2)]
        [SerializeField] private float onSelectScaleReduction;
        [SerializeField] private Animator animator;
        [SerializeField] private float blendDelayTime;
        [SerializeField] private GameObject myCam;
        [SerializeField] private Transform visuals;

        private PlayerStaticFader _playerStaticFader;
        private PlayerController _playerController;
        private PlayerColorController _playerColorController;
        private AudioSource _audioSource; 

        private UIEventChannel _uiEventChannel;
        private AudioEventChannel _audioEventChannel; 
        private PlayerEventChannel _playerEventChannel; 

        private Vector3 _originalScale;
        private bool _selected;
        private int _deselectionLayerMask;
        private int _selectionLayerMask;

        private void Awake()
        {
            _selectionLayerMask = LayerMask.GetMask("Playables");
            _deselectionLayerMask = LayerMask.GetMask("BuildingBlock") | _selectionLayerMask;
            _uiEventChannel = GameEventManager.GetEventChannel<UIEventChannel>();
            _audioEventChannel = GameEventManager.GetEventChannel<AudioEventChannel>();
            _playerEventChannel = GameEventManager.GetEventChannel<PlayerEventChannel>();

            _playerStaticFader = GetComponentInChildren<PlayerStaticFader>();
            _playerColorController = GetComponentInChildren<PlayerColorController>();
            _playerController = GetComponent<PlayerController>();
            _audioSource = GetComponent<AudioSource>();

            _playerController.enabled = false;
            animator.enabled = false; 

            myCam.SetActive(false);

            _playerEventChannel.OnGameModeUpdate += OnPlayerModeUpdate; 
        }

        private void OnPlayerModeUpdate(GameMode mode)
        {
            if(mode == GameMode.PlayableSelected && _selected)
            {
                _playerColorController.SetPlayerColor(PlayableColorState.Playing);
            }
            else if(mode == GameMode.PlayableSelected && !_selected)
            {
                _playerColorController.SetPlayerColor(PlayableColorState.NonSelectable);    
            }
            else if(mode == GameMode.InspectMode)
            {
                _playerColorController.SetPlayerColor(PlayableColorState.Selectable); 
            }
        }

        public void OnSelected()
        {
            _audioEventChannel.RaisePlayAudioWithAudioSource(_audioSource, AudioType.ZoomIn); 
            _selected = true;
            _originalScale = transform.localScale;
            Vector3 scale = _originalScale / onSelectScaleReduction;
            _playerStaticFader.Show(!_selected);
            visuals.SetParent(null); 
            AlignWithCamera();
            visuals.SetParent(transform); 
            myCam.SetActive(_selected);
            StartCoroutine(EnablePlayerController());
            StartCoroutine(SetScale(scale, transform));
        }

        private void AlignWithCamera()
        {
           Vector3 point =   Math3DUtils.ClosestPointOnPlane(Camera.main.transform.position, transform.up, transform.position);
            Vector3 dir = transform.position - point;
            transform.forward = dir.normalized;
        }

        public void OnDeselected()
        {
            _audioEventChannel.RaisePlayAudioWithAudioSource(_audioSource, AudioType.ZoomOut); 
            visuals.localRotation = Quaternion.identity; 
            _selected = false;
            animator.enabled = false; 
            _playerStaticFader.Show(!_selected);
            myCam.SetActive(_selected);
            _playerController.Disable();
            StartCoroutine(ReverRotation());
            StartCoroutine(RevertPosition());
            StartCoroutine(SetScale(Vector3.one, transform));
        }

        private IEnumerator EnablePlayerController()
        {
            yield return new WaitForSeconds(blendDelayTime);
            _playerController.Enable();
            animator.enabled = true; 
        }

        private IEnumerator ReverRotation()
        {
            Quaternion x = transform.rotation;
            float elapsedTime = 0f;

            while (elapsedTime < blendDelayTime)
            {
                elapsedTime += Time.deltaTime;
                transform.rotation = Quaternion.Slerp(x, Quaternion.identity, elapsedTime / blendDelayTime);
                yield return null;
            }

            transform.rotation = Quaternion.identity;
        }


        private IEnumerator SetScale(Vector3 scaleToSet, Transform targetTransform, Action OnComplete = null)
        {
            Vector3 currScale = targetTransform.localScale;
            float elapsedTime = 0f;

            while (elapsedTime < blendDelayTime)
            {
                elapsedTime += Time.deltaTime;
                targetTransform.localScale = Vector3.Slerp(currScale, scaleToSet, elapsedTime / blendDelayTime);
                yield return null;
            }

            OnComplete?.Invoke();

            targetTransform.localScale = scaleToSet;
        }

        private IEnumerator RevertPosition()
        {
            Vector3 x = transform.position;

            Vector3 position = new Vector3(
                (int)Math.Round(x.x, 0),
                (int)Math.Round(x.y, 0),
                (int)Math.Round(x.z, 0));

            float elapsedTime = 0.0f;

            while (elapsedTime < blendDelayTime)
            {
                elapsedTime += Time.deltaTime;
                transform.position = Vector3.Slerp(transform.position, position, elapsedTime / blendDelayTime);
                yield return null;
            }

            transform.position = position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DeathCube"))
            {
                _uiEventChannel.RaisePushUIScreen(ScreenType.LevelFailedScreen);
                _audioEventChannel.RaisePlayAudioWithAudioSource(_audioSource, AudioType.LevelFailed,true); 
                Despose();
            }
        }

        public void Despose(bool hide = false)
        {
            _playerController.Disable(true); 
            if(hide)
            {
                StartCoroutine(SetScale(Vector3.zero, visuals,() => {
                    _uiEventChannel.RaisePushUIScreen(ScreenType.LevelWonScreen);
                    Destroy(this);
                }));
                _audioEventChannel.RaisePlayAudioWithAudioSource(_audioSource, AudioType.LevelWon, true);
            }
            else
            {
                Destroy(this);
            }
        }


        public bool CanBeDeselected()
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, -transform.up);

            bool isGrounded = Physics.Raycast(ray, out hit, 1f, _deselectionLayerMask, QueryTriggerInteraction.Ignore);

            if(!isGrounded)
            {
                _audioEventChannel.RaisePlayAudioWithAudioSource(_audioSource, AudioType.InvalidSelection);
            }
            return isGrounded;
        }

        public bool CanBeSelected()
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, transform.up);

            bool hitSomething = Physics.Raycast(ray, out hit, 1f, _selectionLayerMask, QueryTriggerInteraction.Ignore);

            if (hitSomething)
            {
                _audioEventChannel.RaisePlayAudioWithAudioSource(_audioSource, AudioType.InvalidSelection);
            }
            return !hitSomething;
        }

        private void OnDestroy()
        {
            _playerEventChannel.OnGameModeUpdate -= OnPlayerModeUpdate;
        }
    }
}