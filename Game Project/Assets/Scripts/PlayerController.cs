using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Vector3 velocity;
    Rigidbody myRigidbody;

	void Start ()
    {
        myRigidbody = GetComponent<Rigidbody>();
	}

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;        
    }

    public void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(heightCorrectedPoint);
    }
     
    void FixedUpdate() //Se tiver poucos fps não anda por cima das coisas
    {
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
    }
	
}
