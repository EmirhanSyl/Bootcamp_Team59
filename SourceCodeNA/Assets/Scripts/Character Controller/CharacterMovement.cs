using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    [SerializeField] float movementSpeed = 5;
    [SerializeField] float sprintSpeed = 8;
    [SerializeField] float rotationSpeed = 360;
    [SerializeField] float movementSpeedWhileAttack = 2.5f;

    private float currentSpeed;
    private float speedParamOnAnimator;
    private float attackDuration;
    private float comboTimer;
    private float attackAnimIndex;
    private float timeWithoutAction;

    private bool isMoving;
    private bool isAttack;
    private bool combo;

    Rigidbody rig;
    Animator animator;

    Vector3 movementVector;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        currentSpeed = movementSpeed;
    }

    private void Update()
    {
        GetCharacterMovements();
        Look();
        Attack();

        animator.SetFloat("Speed", speedParamOnAnimator);
    }

    private void FixedUpdate()
    {
        rig.MovePosition(Time.deltaTime * currentSpeed * transform.forward * movementVector.normalized.magnitude + transform.position);        
    }

    void GetCharacterMovements()
    {
        movementVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(KeyCode.LeftShift) && !isAttack)
        {
            currentSpeed = sprintSpeed;
            
            if (isMoving) StartCoroutine(ChangeFloatValueSmoothly(speedParamOnAnimator, 1f, 0.1f));
        }
        else if(!isAttack)
        {
            currentSpeed = movementSpeed;            
            if(isMoving) StartCoroutine(ChangeFloatValueSmoothly(speedParamOnAnimator, 0.5f, 0.1f));
        }
    }

    void Look()
    {
        if (movementVector != Vector3.zero)
        {
            var rotation = Quaternion.LookRotation(movementVector.ToIso(), Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

            isMoving = true;
        }
        else
        {
            isMoving = false;            
            StartCoroutine(ChangeFloatValueSmoothly(speedParamOnAnimator, 0f, 0.1f));
        }
    }

    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttack)
        {
            StartCoroutine(Attacking());
            comboTimer = 0;
        }
        else if (isAttack)
        {
            comboTimer += Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.Space) && comboTimer > attackDuration * 70 / 100)
            {
                combo = true;
                attackAnimIndex = 1;
                animator.SetFloat("AttackAnim", attackAnimIndex);
            }            
        }
        else if (!isAttack)
        {
            timeWithoutAction += Time.deltaTime;

            if (timeWithoutAction > 0.5f && combo)
            {
                combo = false;
            }
        }
    }

    IEnumerator ChangeFloatValueSmoothly(float v_start, float v_end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            speedParamOnAnimator = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        speedParamOnAnimator = v_end;
    }

    IEnumerator Attacking()
    {
        isAttack = true;
        currentSpeed = movementSpeedWhileAttack;
        animator.SetTrigger("Attack");
        attackDuration = animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(attackDuration);

        if (!combo)
        {
            attackAnimIndex = 0;
            animator.SetFloat("AttackAnim", attackAnimIndex);
        }
        currentSpeed = movementSpeed;
        isAttack = false;
    }

}

public static class IsoMovementHelper
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
