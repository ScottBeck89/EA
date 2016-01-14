using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public enum MovementState
{
    STOPPED,
    ACCELERATING,
    MOVING,
    FALL_FORGIVENESS,
    FALLING,
    JUMPING,
    WALL_LAUNCH,
    HUGGING_WALL,
}

[Serializable]
[RequireComponent(typeof( Rigidbody2D ), typeof(MovementStateManager))]
public class MovementModel : MonoBehaviour
{
    #region Public Members

    public MovementStateManager manager;

    #endregion

    #region Private Members

    private Rigidbody2D myRigidBody;

    private float absoluteMaxVelocity = 42.00f;

    private float terminalVelocity = 25f;

    private float minimumFallingVelocity = -1.00f;

    private float horizontalTerminalVelocity = 20.00f;

    private float horizontalJumpVelocity = 20.00f;

    private float horizontalAcceleration = 100.00f;

    private float gravityScale = 5.00f;

    private float linearDrag = 1.00f;

    private float jumpForce = 25.00f;

    private float horizontalJumpForce = 22.00f;

    private float jumpMaxVelocity = 10.00f;

    private float inputThreshold = 0.025f;

    private float jumpLeniency = 0.3f;

    private float fallLeniency = 0.1f;//possibly make it distance instead?

    private MovementState state = MovementState.STOPPED;

    private MovementState previousState = MovementState.STOPPED;

    private Vector2 deltaForces = Vector2.zero;

    private int horizontalDirection = 1;

    private Boolean jumpingEnabled = true;

    private float wallHangFactor = 5f;

    private Boolean parabolicJump = false;

    private Boolean transferingVelocity = false;

    private Vector3 velocityToTransfer = Vector3.zero;

    private float angleOfTransfer = 0f;

    private Boolean justWallLaunched = false;

    private Boolean justLaunched = false;

    private float launchForce = 0f;

    private Boolean midAirJumpingEnabled = false;

    private float deleteMeHeight = 0f;

    private float deleteMestartingHeight = 0f;

    #endregion

    #region Properties

    public float AbsoluteMaxVelocity
    {
        get
        {
            return absoluteMaxVelocity;
        }
        set
        {
            absoluteMaxVelocity = value;
        }
    }

    public float TerminalVelocity
    {
        get
        {
            return terminalVelocity;
        }
        set
        {
            terminalVelocity = value;
        }
    }

    public float HorizontalTerminalVelocity
    {
        get
        {
            return horizontalTerminalVelocity;
        }
        set
        {
            horizontalTerminalVelocity = value;
        }
    }

    public float HorizontalJumpVelocity
    {
        get
        {
            return horizontalJumpVelocity;
        }
        set
        {
            horizontalJumpVelocity = value;
        }
    }

    public float HorizontalAcceleration
    {
        get
        {
            return horizontalAcceleration;
        }
        set
        {
            horizontalAcceleration = value;
        }
    }

    public float GravityScale
    {
        get
        {
            return gravityScale;
        }
        set
        {
            gravityScale = value;
            myRigidBody.gravityScale = value;
        }
    }

    public float LinearDrag
    {
        get
        {
            return linearDrag;
        }
        set
        {
            linearDrag = value;
            myRigidBody.drag = value;
        }
    }

    public float JumpForce
    {
        get
        {
            return jumpForce;
        }
        set
        {
            jumpForce = value;
        }
    }

    public float HorizontalJumpForce
    {
        get
        {
            return horizontalJumpForce;
        }
        set
        {
            horizontalJumpForce = value;
        }
    }

    public float JumpMaxVelocity
    {
        get
        {
            return jumpMaxVelocity;
        }
        set
        {
            jumpMaxVelocity = value;
        }
    }

    public float InputThreshold
    {
        get
        {
            return inputThreshold;
        }
        set
        {
            inputThreshold = value;
        }
    }

    public float JumpLeniency
    {
        get
        {
            return jumpLeniency;
        }
        set
        {
            jumpLeniency = value;
        }
    }

    public float FallLeniency
    {
        get
        {
            return fallLeniency;
        }
        set
        {
            fallLeniency = value;
        }
    }

    public MovementState State
    {
        get
        {
            return state;
        }
        set
        {
            previousState = state;
            state = value;
        }
    }

    public MovementState PreviousState
    {
        get
        {
            return previousState;
        }
        set
        {
            previousState = value;
        }
    }

    public Rigidbody2D MyRigidbody
    {
        get
        {
            return myRigidBody;
        }
    }

    public Boolean JumpingEnabled
    {
        get
        {
            return jumpingEnabled;
        }
    }

    public float WallHangFactor
    {
        get
        {
            return wallHangFactor;
        }
        set
        {
            wallHangFactor = Mathf.Clamp( value, 0.25f, 1.0f );
            minimumFallingVelocity = -terminalVelocity * wallHangFactor;
        }
    }

    public Boolean ParabolicJump
    {
        get
        {
            return parabolicJump;
        }
        set
        {
            parabolicJump = value;
        }
    }

    public Boolean MidAirJumpingEnabled
    {
        get
        {
            return midAirJumpingEnabled;
        }
        set
        {
            midAirJumpingEnabled = value;
        }
    }

    #endregion

    #region Public Methods

    public void StopMovement()
    {
        myRigidBody.velocity = Vector3.zero;
        jumpingEnabled = true;
    }

    public void StopHorizontalMovement()
    {
        myRigidBody.velocity = Vector3.zero;
    }

    public void Landed()
    {
        justWallLaunched = false;
    }

    public void EnableJumping()
    {
        jumpingEnabled = true;
    }

    public void DisableJumping()
    {
        jumpingEnabled = false;
    }

    public void WallJump()
    {
        justWallLaunched = true;
    }

    public void VerticalLaunch( float force )
    {
        justLaunched = true;
        launchForce = force;
    }

    public void MoveHorizontally( int direction )
    {
        horizontalDirection = direction;
    }

    public void ApplyHorizontalForce( int direction )
    {
        float forceValue = direction * horizontalAcceleration;
        deltaForces = new Vector2( deltaForces.x + forceValue, deltaForces.y );
    }

    public void ApplyVelocityTransfer( Vector2 velocity, float angle )
    {
        transferingVelocity = true;
        velocityToTransfer = velocity;
        angleOfTransfer = angle;
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();

        if ( PlayerPrefs.GetString( "UseCustomSettings" ) == "true" )
        {
            absoluteMaxVelocity = PlayerPrefs.GetFloat( "AbsoluteMaxVelocity" );
            terminalVelocity = PlayerPrefs.GetFloat( "TerminalVelocity" );
            horizontalTerminalVelocity = PlayerPrefs.GetFloat( "HorizontalTerminalVelocity" );
            horizontalAcceleration = PlayerPrefs.GetFloat( "HorizontalAcceleration" );
            gravityScale = PlayerPrefs.GetFloat( "GravityScale" );
            linearDrag = PlayerPrefs.GetFloat( "LinearDrag" );
            jumpForce = PlayerPrefs.GetFloat( "JumpForce" );
            horizontalJumpForce = PlayerPrefs.GetFloat( "HorizontalJumpForce" );
            jumpMaxVelocity = PlayerPrefs.GetFloat( "JumpMaxVelocity" );
            horizontalJumpVelocity = PlayerPrefs.GetFloat( "HorizontalJumpVelocity" );

            Boolean.TryParse( PlayerPrefs.GetString( "ParabolicJump" ), out parabolicJump );
        }
    }

    private void FixedUpdate()
    {
        if ( state == MovementState.ACCELERATING )
        {
            myRigidBody.AddRelativeForce( deltaForces );
            /*if ( Mathf.Abs( myRigidBody.velocity.x ) >= horizontalTerminalVelocity )
            {
                State = MovementState.MOVING;
            }*/
        } 
        else if ( state == MovementState.MOVING )
        {
            myRigidBody.velocity = new Vector2( horizontalTerminalVelocity * horizontalDirection, myRigidBody.velocity.y );
        }
        else if ( state == MovementState.FALL_FORGIVENESS )
        {
            myRigidBody.AddRelativeForce( deltaForces );
        }
        else if ( state == MovementState.STOPPED )
        {
            myRigidBody.velocity = new Vector2( 0, myRigidBody.velocity.y );

            deleteMeHeight = 0f;
        }
        else if ( state == MovementState.JUMPING )
        {
            if ( parabolicJump )
            {
                float y = Mathf.Clamp( myRigidBody.velocity.y, 0, terminalVelocity );
                myRigidBody.velocity = new Vector2( myRigidBody.velocity.x, y );
                myRigidBody.AddRelativeForce( new Vector2( 0, jumpForce ), ForceMode2D.Impulse );
            }
            else
            {
                myRigidBody.velocity = new Vector2( myRigidBody.velocity.x, jumpMaxVelocity );
            }

            myRigidBody.AddRelativeForce( deltaForces );
        }
        else if ( state == MovementState.WALL_LAUNCH )
        {
            if ( justWallLaunched )
            {
                float y = Mathf.Clamp( myRigidBody.velocity.y, 0, terminalVelocity );
                myRigidBody.velocity = new Vector2( myRigidBody.velocity.x, y );
                myRigidBody.AddRelativeForce( new Vector2( horizontalJumpForce * Mathf.Round( manager.WallHugDirection ), jumpForce ), ForceMode2D.Impulse );

                justWallLaunched = false;
            }

            myRigidBody.AddRelativeForce( deltaForces );
        }
        else if ( state == MovementState.FALLING )
        {
            if ( justLaunched )
            {
                deleteMestartingHeight = transform.position.y;
                myRigidBody.velocity = new Vector2( myRigidBody.velocity.x, 0 );
                myRigidBody.AddRelativeForce( new Vector2( 0, launchForce ), ForceMode2D.Impulse );
                justLaunched = false;
            }

            float offSet = transform.position.y - deleteMestartingHeight;

            if ( offSet > deleteMeHeight )
            {
                deleteMeHeight = offSet;
                Debug.Log( "Max Height: " + deleteMeHeight );
            }

            myRigidBody.AddRelativeForce( deltaForces );
        }
        else if ( state == MovementState.HUGGING_WALL )
        {
            float y = myRigidBody.velocity.y;

            if ( transferingVelocity && ( ( angleOfTransfer <= 150 && angleOfTransfer >= 105 ) || ( angleOfTransfer >= 30 && angleOfTransfer <= 75 ) ) )
            {
                float transferPercent = 0f;
                if ( angleOfTransfer >= 120 )
                {
                    transferPercent = 1f / ( Mathf.Round( ( 150 - angleOfTransfer ) / 15 ) );
                }
                else
                {
                    transferPercent = 1f / ( Mathf.Round( ( angleOfTransfer - 30 ) / 15 ) );
                }
                y += Mathf.Abs( velocityToTransfer.x * Mathf.Clamp( transferPercent, 0f, 3f ) );
                transferingVelocity = false;
            }

            if ( myRigidBody.velocity.y < minimumFallingVelocity )
            {
                y = Mathf.Lerp( myRigidBody.velocity.y, minimumFallingVelocity, gravityScale * Time.fixedDeltaTime );
            }
            myRigidBody.velocity = new Vector2( 0, y );
            myRigidBody.AddRelativeForce( deltaForces );
        }

        float xVel = myRigidBody.velocity.x;
        float yVel = myRigidBody.velocity.y;

        if ( state == MovementState.WALL_LAUNCH )
        {
            if ( horizontalJumpForce > horizontalTerminalVelocity )
            {
                xVel = Mathf.Clamp( myRigidBody.velocity.x, -horizontalJumpForce, horizontalJumpForce );
            }
            else
            {
                xVel = Mathf.Clamp( myRigidBody.velocity.x, -horizontalTerminalVelocity, horizontalTerminalVelocity );
            }

            if ( jumpForce > terminalVelocity )
            {
                yVel = Mathf.Clamp( myRigidBody.velocity.y, -jumpForce, jumpForce );
            }
            else
            {
                yVel = Mathf.Clamp( myRigidBody.velocity.y, -terminalVelocity, terminalVelocity );
            }
        }
        else
        {
            xVel = Mathf.Clamp( myRigidBody.velocity.x, -horizontalTerminalVelocity, horizontalTerminalVelocity );
            yVel = Mathf.Clamp( myRigidBody.velocity.y, -terminalVelocity, 1000f );
        }

        myRigidBody.velocity = new Vector2( xVel, yVel );

        //myRigidBody.velocity = Vector2.ClampMagnitude( myRigidBody.velocity, absoluteMaxVelocity );

        deltaForces = Vector2.zero;
    }

    #endregion
}