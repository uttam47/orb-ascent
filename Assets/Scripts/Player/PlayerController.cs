using Unity.Cinemachine;
using UnityEngine;

namespace AnalyticalApproach.OrbAscent
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        protected Vector3 mDir;
        protected Vector3 _moveDirection;
        protected Vector3 _turnDirection;
        protected AudioSource audioSource;
        protected DigitzedInputExpander analogMovementInput;
        protected PlayerEventChannel playerEventChannel;
        protected AudioEventChannel audioEventChannel; 
        protected bool jump;

        protected CharacterController controller;

        [SerializeField] protected float jumpSpeed = 8.0f;
        [SerializeField] protected float gravity = 20.0f;
        [SerializeField] protected float maxMovementSpeed = 3f;
        [SerializeField] protected float movementInputAcceleration = 3;
        [SerializeField] protected CameraSettings cameraSettings;
        [SerializeField] protected CinemachineOrbitalFollow cinemachineOrbitalFlow;
        [SerializeField] protected Animator animator;

        protected virtual void Awake()
        {
            controller = GetComponent<CharacterController>();
            audioSource = GetComponent<AudioSource>();
            playerEventChannel = GameEventManager.GetEventChannel<PlayerEventChannel>();
            audioEventChannel = GameEventManager.GetEventChannel<AudioEventChannel>();
            analogMovementInput = new DigitzedInputExpander(maxMovementSpeed, movementInputAcceleration);
        }


        public void Enable()
        {
            enabled = true;
            playerEventChannel.OnPlayerJump += Jump;
            playerEventChannel.OnPlayerMove += MoveInDirection;
            playerEventChannel.OnPlayerTurn += Look;
        }

        public void Disable(bool disableEventsOnly = false)
        {
            if (!disableEventsOnly)
            {
                enabled = false;
            }

            _moveDirection = Vector2.zero;
            _turnDirection = Vector2.zero; 

            playerEventChannel.OnPlayerJump -= Jump;
            playerEventChannel.OnPlayerMove -= MoveInDirection;
            playerEventChannel.OnPlayerTurn -= Look;
        }

        private void Update()
        {
            Move();
            Turn();
        }

        protected virtual void Turn()
        {
            if (!controller.enabled) return;
            if (controller.isGrounded)
            {
                Vector2 processedInput = cameraSettings.tppCamSpeed * _turnDirection * cameraSettings.axisWeigts * Time.deltaTime;
                transform.Rotate(0, -processedInput.x * cameraSettings.invertXAxis, 0) ;
                cinemachineOrbitalFlow.VerticalAxis.Value += processedInput.y * cameraSettings.invertXAxis;

                cinemachineOrbitalFlow.VerticalAxis.Value = Mathf.Clamp(
                        cinemachineOrbitalFlow.VerticalAxis.Value,
                        cinemachineOrbitalFlow.VerticalAxis.Range.x, 
                        cinemachineOrbitalFlow.VerticalAxis.Range.y);
            }
        }

        protected virtual void Move()
        {
            if (!controller.enabled) return;

            if (controller.isGrounded)
            {
                Vector2 analog = analogMovementInput.GetDecompressedInputValue(_moveDirection);

                mDir = (transform.right * analog.x + transform.forward * analog.y);
                if (jump)
                {
                    mDir.y = jumpSpeed;
                    jump = false;
                    audioEventChannel.RaisePlayAudioWithAudioSource(audioSource, AudioType.Jump); 
                }
            }

            mDir.y -= gravity * Time.deltaTime;

            controller.Move(mDir * Time.deltaTime);
        }

        public void MoveInDirection(Vector2 moveDirection)
        {
            _moveDirection = moveDirection;
            animator.enabled = ! (moveDirection.magnitude > 0); 
        }

        protected virtual void Fire() { }

        public void Look(Vector2 lookDelta)
        {
            _turnDirection = lookDelta;
        }

        public void Jump(bool value)
        {
            jump = value;
        }

        private void OnDestroy()
        {
            Disable();
        }
    }

}