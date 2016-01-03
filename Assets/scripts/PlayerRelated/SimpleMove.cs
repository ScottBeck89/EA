using UnityEngine;
using System.Collections;

public class SimpleMove : MonoBehaviour {

	private Vector2 input2D;
	private float dampingFactor = 0.95f;

	public float maxSpeed = 5f;						//Player's max speed
	public float jumpForce = 10f;
	[Range(0,3)]
	public float speedPercentage = 1.0f;
	public float jumpPercentage = 1.0f;

	// Use this for initialization
	void FixedUpdate ()
	{
        input2D = new Vector2( Input.GetAxisRaw( "Horizontal" ), Input.GetAxisRaw( "Vertical" ) * jumpForce );

        //If we have not pressed any of the buttons, slow us down but only if we are still moving.
        if ( input2D.magnitude == 0 && GetComponent<Rigidbody2D>().velocity.magnitude > 0 )
        {
            GetComponent<Rigidbody2D>().AddRelativeForce( -( GetComponent<Rigidbody2D>().velocity.normalized * dampingFactor ) );
        }
        else
        {
            //This gives an immediate feedback in movement.  Instant acceleration clamped to maxSpeed.
            GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude( input2D * maxSpeed, maxSpeed * speedPercentage );
        }

		/*input2D = new Vector2(Input.GetAxisRaw ("Horizontal"), 0);
		
		//If we have not pressed any of the buttons, slow us down but only if we are still moving.
		if(input2D.magnitude == 0 && GetComponent<Rigidbody2D>().velocity.magnitude > 0) 
		{
            GetComponent<Rigidbody2D>().AddRelativeForce( -( GetComponent<Rigidbody2D>().velocity.normalized * dampingFactor ) );
		} 
		else 
		{
			//This gives an immediate feedback in movement.  Instant acceleration clamped to maxSpeed.
            GetComponent<Rigidbody2D>().AddRelativeForce( Vector2.ClampMagnitude( input2D * maxSpeed, maxSpeed * speedPercentage ) );
		}

		if ( Input.GetAxisRaw ("Vertical") > 0 )
		{
            GetComponent<Rigidbody2D>().AddForce( Vector2.ClampMagnitude( Vector2.up * jumpForce, jumpForce * jumpPercentage ) );
		}*/
	}
}
