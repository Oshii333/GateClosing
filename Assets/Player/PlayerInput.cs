using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GateClosing
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] InputActionAsset inputActions;
        [SerializeField] InputActionProperty look;
        [SerializeField] InputActionProperty move;
        [SerializeField] InputActionProperty attack;

        [Header("Components")]
        [SerializeField] FloatingCapsule floatingCapsule;
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

        [Header("Attacking")]
        [SerializeField] Collider hurtbox;
        [SerializeField] float attackStrength;
        [SerializeField] bool attacking;
        [SerializeField] float attackTime;
        [SerializeField] float attackTimer;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

        }
        public void OnEnable()
        {
            inputActions.Enable();
            look.action.Enable();
            move.action.Enable();
            attack.action.Enable();

            attack.action.started += Attack;

            hurtbox.enabled = false;
        }

        public void OnDisable()
        {
            inputActions.Disable();
            look.action.Disable();
            move.action.Disable();
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

        }


        public void OnTriggerEnter(Collider other)
        {
            if (ColliderManager.Ragdolls.TryGetValue(other, out Ragdoll ragdoll))
            {
                ragdoll.OnHit(camera.transform.forward * attackStrength);
            }
        }
    }
}
