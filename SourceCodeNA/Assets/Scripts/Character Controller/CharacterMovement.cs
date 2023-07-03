using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static bool Skeleton;

    public bool isAttack;

    public float movementSpeed = 5;
    public float sprintSpeed = 8;
    [SerializeField] float rotationSpeed = 360;
    [SerializeField] float movementSpeedWhileAttack = 2.5f;
    [SerializeField] float deathDownSpeed = 0.5f;

    [SerializeField] private float attackDuration;
    [SerializeField] private float minWeaponDamage;
    [SerializeField] private float maxWeaponDamage;

    [SerializeField] private GameObject cameraPivot;

    private float currentSpeed;
    private float speedParamOnAnimator;
    private float attackTimer;
    private float camRotTimer;
    private float comboTimer;
    private float attackAnimIndex;
    private float timeWithoutAction;

    private bool isMoveLocked;
    private bool isMoving;
    private bool combo;
    private bool isBuried;
    private bool BuryControl;
    private bool BuryDownControl;

    Rigidbody rig;
    public Animator animator;
    CapsuleCollider coll;
    PlayerCombat playerCombat;

    public Vector3 movementVector;
    public Vector3 goUpVec;
    Vector3 goDownVec;

    float rot = 45;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider>();
        playerCombat = GetComponent<PlayerCombat>();

        currentSpeed = movementSpeed;
    }

    private void Update()
    {
        isMoveLocked = playerCombat.isAttackingEnemy ?  true :  false;
        isMoveLocked = PlayerHealth.dead ? true : isMoveLocked;
        
        if (!isMoveLocked)
        {
            Look();
            Attack();
            GetCharacterMovements();
            animator.SetFloat("Speed", speedParamOnAnimator);
            StopCoroutine(Dead());
        }
        if (PlayerHealth.dead)
        {
            StartCoroutine(Dead());
        }

        if(Skeleton && PlayerHealth.health <= 0)
        {
            //transform.position = 
        }

        if (camRotTimer < 1)
        {
            camRotTimer += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Q) && camRotTimer >= 1)
        {
            cameraPivot.transform.DORotate(cameraPivot.transform.eulerAngles + new Vector3(0, 90, 0), 0.5f);
            //cameraPivot.transform.Rotate(cameraPivot.transform.rotation.x, cameraPivot.transform.rotation.y + 90, cameraPivot.transform.rotation.z, Space.World);
            IsoMovementHelper._isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, rot + 90, 0));
            rot += 90;
            camRotTimer = 0;
        }
    }

    private void FixedUpdate()
    {
        if (!isMoveLocked)
        {
            rig.MovePosition(Time.deltaTime * currentSpeed * transform.forward * movementVector.normalized.magnitude + transform.position);        
        }
    }

    void GetCharacterMovements()
    {
        movementVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(KeyCode.LeftShift) && !isAttack)
        {
            DOVirtual.Float(currentSpeed, sprintSpeed, 0.4f, (speed) => currentSpeed = speed);

            if (isMoving) StartCoroutine(ChangeFloatValueSmoothly(speedParamOnAnimator, 1f, 0.1f));
        }
        else if(!isAttack)
        {
            DOVirtual.Float(currentSpeed, movementSpeed, 0.4f, (speed) => currentSpeed = speed);
            if (isMoving) StartCoroutine(ChangeFloatValueSmoothly(speedParamOnAnimator, 0.5f, 0.1f));
        }

        if(PlayerHealth.isHitted)
        {
            DOVirtual.Float(currentSpeed, movementSpeedWhileAttack, 0.4f, (speed) => currentSpeed = speed);
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
        if (Input.GetMouseButtonDown(0) && !isAttack && !PlayerHealth.isHitted)
        {
            //StartCoroutine(Attacking());
            animator.SetTrigger("Attack");
            attackAnimIndex = (int)Mathf.Repeat(attackAnimIndex + 1, 2);
            animator.SetFloat("AttackAnim", attackAnimIndex);
            attackDuration = animator.GetCurrentAnimatorStateInfo(0).length;
            isAttack = true;
        }
        else if (isAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackDuration)
            {
                attackTimer = 0;
                isAttack = false;
            }
        }
    }

    IEnumerator Dead()
    {

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 1f);

        if (!isBuried)
        {
            rig.useGravity = false;
            coll.isTrigger = true;
            isBuried = true;
        }
        if (!BuryDownControl)
        {
            goDownVec = new Vector3(transform.position.x, transform.position.y - 2, transform.position.z);
            transform.DOMove(goDownVec, 4f);
            BuryDownControl = true;
        }
        //transform.position = Vector3.Lerp(transform.position, goDownVec, deathDownSpeed * Time.deltaTime);

        yield return new WaitForSeconds(4f);

        transform.GetChild(0).gameObject.SetActive(false);
        animator = transform.GetChild(1).gameObject.GetComponent<Animator>();

        if (isBuried)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            isBuried = false;
        }
        if (!BuryControl)
        {
            goUpVec = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            transform.DOMove(goUpVec, 2f);
            BuryControl = true;
        }
        //transform.position = Vector3.Lerp(transform.position, goDownVec, deathDownSpeed * Time.deltaTime);

        yield return new WaitForSeconds(2);
        rig.useGravity = true;
        coll.isTrigger = false;
        PlayerHealth.dead = false;
        PlayerHealth.health = 20;
        movementSpeed = 4;
        sprintSpeed = 6;
        Skeleton = true;
    }

    public float WeaponDamage()
    {
        float weaponDamage = Random.Range(minWeaponDamage, maxWeaponDamage);
        return weaponDamage;
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
        DOVirtual.Float(currentSpeed, movementSpeedWhileAttack, 0.4f, (speed) => currentSpeed = speed);
        animator.SetTrigger("Attack");
        attackDuration = animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(attackDuration + 0.1f);

        attackAnimIndex = (int)Mathf.Repeat(attackAnimIndex + 1, 2);
        animator.SetFloat("AttackAnim", attackAnimIndex);
        DOVirtual.Float(currentSpeed, movementSpeed, 0.4f, (speed) => currentSpeed = speed);
        isAttack = false;
    }

}

public static class IsoMovementHelper
{
    public static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
