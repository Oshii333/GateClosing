using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GateClosing
{
    public class Player : MonoBehaviour
    {
        public static Player instance;

        [SerializeField] InputActionAsset inputActions;
        [SerializeField] InputActionProperty look;
        [SerializeField] InputActionProperty move;
        [SerializeField] InputActionProperty attack;
        [SerializeField] InputActionProperty jump;

        [Header("Components")]
        [SerializeField] public FloatingCapsule floatingCapsule;
        [SerializeField] Animator animator;
        [SerializeField] Camera camera;

        [Header("Camera Rotation")]
        [SerializeField] Transform cameraPivot;
        [SerializeField] Transform armsPivot;
        [SerializeField] float slerp;
        Quaternion _armPivotRotation;
        [SerializeField] Vector2 turn;
        [SerializeField] float sensitivity = .5f;
        [SerializeField] Vector3 deltaMove;
        [SerializeField] float speed = 1;

        [Header("Movement")]
        [SerializeField] Vector3 movementInput;
        bool _jumping;

        [Header("Attacking")]
        [SerializeField] Collider playerCollider;
        [SerializeField] Collider hurtbox;
        [SerializeField] float attackStrength;
        [SerializeField] public bool attacking;
        [SerializeField] float attackTime;
        [SerializeField] float attackTimer;
        [SerializeField] AudioManager audioManager;

        private float tannoyTimer = Random.Range(2, 5);
        private AudioSource audioSource;

        public event Del HitSomeone;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            ColliderManager.Player.Add(playerCollider, this);
            Cursor.lockState = CursorLockMode.Locked;

        }
        public void OnEnable()
        {
            inputActions.Enable();
            look.action.Enable();
            move.action.Enable();
            attack.action.Enable();

            attack.action.started += Attack;
            jump.action.started += Jump;

            hurtbox.enabled = false;
        }

        public void OnDisable()
        {
            inputActions.Disable();
            look.action.Disable();
            move.action.Disable();
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if (floatingCapsule.isGrounded)
            {
                _jumping = true;
            }
        }

        public void Attack(InputAction.CallbackContext context)
        {
            animator.SetTrigger("Attack");
            attacking = true;
            hurtbox.enabled = true;
            attackTimer = attackTime;
        }

        void Update()
        {
            //turn += lookAction.action.ReadValue<Vector2>() * Time.smoothDeltaTime * sensitivity;
            turn.x += Input.GetAxis("Mouse X") * sensitivity;
            turn.y += Input.GetAxis("Mouse Y") * sensitivity;
            cameraPivot.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
            _armPivotRotation = Quaternion.Slerp(_armPivotRotation, cameraPivot.rotation, slerp);
            armsPivot.rotation = _armPivotRotation;

            movementInput = move.action.ReadValue<Vector2>();
            movementInput = new(movementInput.x, 0, movementInput.y);
            movementInput = camera.transform.TransformDirection(movementInput);
            movementInput = Vector3.ProjectOnPlane(movementInput, Vector3.up);
            floatingCapsule.Move(movementInput);

            if (attacking)
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer < 0)
                {
                    attacking = false;
                    hurtbox.enabled = false;
                }
            }

            if (tannoyTimer < 0)
            {
                tannoyTimer = Random.Range(10, 15);
                audioSource.clip = audioManager.GetTannoy();
                audioSource.transform.position = this.transform.position;
                audioSource.Play();
            }
            else
            {
                tannoyTimer -= Time.deltaTime;
            }


            floatingCapsule.Jump(_jumping);
            _jumping = false;
        }


        public void OnTriggerEnter(Collider other)
        {            
            if (attacking)
            {
                if (ColliderManager.Ragdolls.TryGetValue(other, out Ragdoll ragdoll))
                {
                    ragdoll.OnHit(camera.transform.forward * attackStrength);
                    HitSomeone?.Invoke();
                }
            }
        }
    }
}
