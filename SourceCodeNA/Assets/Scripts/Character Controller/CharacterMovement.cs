using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    [SerializeField] float movementSpeed = 5;
    [SerializeField] float rotationSpeed = 360;

    Rigidbody rig;

    Matrix4x4 isoMatrix;
    Vector3 movementVector;
    Vector3 toIso;


    private void Awake()
    {
        rig = GetComponent<Rigidbody>();

        isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    }

    private void Update()
    {
        GetCharacterMovements();
        Look();
    }

    private void FixedUpdate()
    {
        //rig.velocity = movementVector;
        rig.MovePosition(Time.deltaTime * movementSpeed * transform.forward * movementVector.normalized.magnitude + transform.position);        
    }

    void GetCharacterMovements()
    {
        movementVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    void Look()
    {
        if (movementVector != Vector3.zero)
        {
            var rotation = Quaternion.LookRotation(ToIso(movementVector), Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime); 
        }
    }

    Vector3 ToIso(Vector3 input)
    {
        isoMatrix.MultiplyPoint3x4(input);
        return input;
    }
    

}
