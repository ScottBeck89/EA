﻿using System;
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
    JUMPED,
    JUMPING_INTO_WALL,
    WALL_CLIMBING,
}
[Serializable]
[RequireComponent(typeof( Rigidbody2D ))]
public class MovementModel : MonoBehaviour
{
    private Rigidbody2D myRigidBody;

    private float absoluteMaxVelocity = 12.00f;

    private float terminalVelocity = 9.81f;

    private float horizontalTerminalVelocity = 10.00f;

    private float horizontalAcceleration = 20.00f;

    private float gravityScale = 10.00f;

    private float linearDrag = 0.1f;

    private float jumpForce = 5.00f;

    private float jumpMaxVelocity = 10.00f;

    private float runningThreshold = 0.05f;

    private MovementState state = MovementState.STOPPED;

    private Vector2 deltaForces = Vector2.zero;

    private int horizontalDirection = 1;

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

    public float RunningThreshold
    {
        get
        {
            return runningThreshold;
        }
        set
        {
            runningThreshold = value;
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
            state = value;
        }
    }

    public Rigidbody2D MyRigidbody
    {
        get
        {
            return myRigidBody;
        }
    }

    #endregion

    #region Public Methods

    public void StopMovement()
    {
        myRigidBody.velocity = Vector3.zero;
        state = MovementState.STOPPED;
    }

    public void StopJumping()
    {

    }

    public void MoveHorizontally( int direction )
    {
        horizontalDirection = direction;
        /*if ( state == MovementState.MOVING || state == MovementState.STOPPED )
        {
            deltaForces = new Vector2( horizontalTerminalVelocity * direction, deltaForces.y );
        }*/
    }

    public void ApplyHorizontalForce( int direction )
    {
        float forceValue = direction * horizontalAcceleration;
        deltaForces = new Vector2( deltaForces.x + forceValue, deltaForces.y );
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        myRigidBody = GetComponent<Rigidbody2D>();

        absoluteMaxVelocity = PlayerPrefs.GetFloat( "AbsoluteMaxVelocity" );
        terminalVelocity = PlayerPrefs.GetFloat( "TerminalVelocity" );
        horizontalTerminalVelocity = PlayerPrefs.GetFloat( "HorizontalTerminalVelocity" );
        horizontalAcceleration = PlayerPrefs.GetFloat( "HorizontalAcceleration" );
        gravityScale = PlayerPrefs.GetFloat( "GravityScale" );
        linearDrag = PlayerPrefs.GetFloat( "LinearDrag" );
    }

    private void FixedUpdate()
    {
        if ( state == MovementState.ACCELERATING )
        {
            myRigidBody.AddRelativeForce( deltaForces );
            if ( Mathf.Abs( myRigidBody.velocity.x ) >= horizontalTerminalVelocity )
            {
                state = MovementState.MOVING;
            }
        } 
        else if ( state == MovementState.MOVING )
        {
            myRigidBody.velocity = new Vector2( horizontalTerminalVelocity * horizontalDirection, myRigidBody.velocity.y );
        }
        else if ( state == MovementState.STOPPED )
        {
            myRigidBody.velocity = new Vector2( 0, myRigidBody.velocity.y );
        }
        else if ( state == MovementState.JUMPING )
        {
            myRigidBody.velocity = new Vector2( myRigidBody.velocity.x, jumpMaxVelocity );
            myRigidBody.AddRelativeForce( deltaForces );
        }
        else if ( state == MovementState.JUMPED || state == MovementState.FALLING )
        {
            myRigidBody.velocity = new Vector2( myRigidBody.velocity.x, myRigidBody.velocity.y );
            myRigidBody.AddRelativeForce( deltaForces );
        }

        float xVel = Mathf.Clamp( myRigidBody.velocity.x, -horizontalTerminalVelocity, horizontalTerminalVelocity );
        float yVel = Mathf.Clamp( myRigidBody.velocity.y, -terminalVelocity, terminalVelocity );

        myRigidBody.velocity = new Vector2( xVel, yVel );
        
        myRigidBody.velocity = Vector2.ClampMagnitude( myRigidBody.velocity, absoluteMaxVelocity );

        deltaForces = Vector2.zero;
    }

    #endregion
}