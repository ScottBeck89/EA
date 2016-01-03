using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MovementController : MonoBehaviour
{
    public MovementModel movementModel;

    public GameObject jumpEffect;

    private Vector3 StartingPosition;

    private float terminalVelocity;

    private float startJumpTime = 0f;

    private float jumpDeltaTime = 0f;

    private float jumpLeniency = .3f;

    private float startFallTime = 0f;

    private float fallDeltaTime = 0f;

    private float fallLeniency = .2f;

    private Boolean jumpPressed = false;

    #region Properties

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

    public MovementModel MovementModel
    {
        get
        {
            return movementModel;
        }
    }

    #endregion

    #region Public Methods

    public void resetCharacter()
    {
        transform.position = StartingPosition;
        movementModel.StopMovement();
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        StartingPosition = transform.position;
    }

    void Update()
    {
        if ( Input.GetKey("space") )
        {
            resetCharacter();
        }
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw( "Horizontal" ) + Input.GetAxisRaw( "XBOXLeftJoystickX" );
        float verticalInput = Input.GetAxisRaw( "Vertical" );

        if ( Input.GetButton( "XBOXAButton" ) )
        {
            verticalInput = 1f;
        }

        if ( movementModel == null )
        {
            print( "movement model is null!" );
            return;
        }

        jumpDeltaTime += Time.fixedDeltaTime;
        fallDeltaTime += Time.fixedDeltaTime;

        switch( movementModel.State )
        {
            case MovementState.STOPPED:
                {
                    if ( horizontalInput > movementModel.RunningThreshold )
                    {
                        movementModel.State = MovementState.ACCELERATING;
                    }
                    else if ( -horizontalInput > movementModel.RunningThreshold )
                    {
                        movementModel.State = MovementState.ACCELERATING;
                    }
                    else
                    {
                        movementModel.StopMovement();
                    }

                    if ( verticalInput > 0 && !jumpPressed )
                    {
                        startJumpTime = Time.time;
                        jumpDeltaTime = 0f;
                        jumpPressed = true;

                        movementModel.State = MovementState.JUMPING;
                    }
                    else if ( verticalInput == 0 && jumpPressed )
                    {
                        jumpPressed = false;
                    }
                    break;
                }
            case MovementState.ACCELERATING:
                {
                    if ( horizontalInput > 0  )
                    {
                        movementModel.ApplyHorizontalForce( 1 );
                    }
                    else if ( -horizontalInput > 0 )
                    {
                        movementModel.ApplyHorizontalForce( -1 );
                    }
                    else
                    {
                        movementModel.State = MovementState.STOPPED;
                    }

                    if ( verticalInput > 0 && !jumpPressed )
                    {
                        startJumpTime = Time.time;
                        jumpDeltaTime = 0f;
                        jumpPressed = true;

                        movementModel.State = MovementState.JUMPING;
                    }
                    else if ( verticalInput == 0 && jumpPressed )
                    {
                        jumpPressed = false;
                    }
                    break;
                }
            case MovementState.MOVING:
                {
                    if ( horizontalInput > movementModel.RunningThreshold )
                    {
                        movementModel.MoveHorizontally( 1 );
                    }
                    else if ( -horizontalInput > movementModel.RunningThreshold )
                    {
                        movementModel.MoveHorizontally( -1 );
                    }
                    else
                    {
                        movementModel.State = MovementState.STOPPED;
                    }

                    if ( verticalInput > 0 && !jumpPressed )
                    {
                        startJumpTime = Time.time;
                        jumpDeltaTime = 0f;
                        jumpPressed = true;

                        movementModel.State = MovementState.JUMPING;
                    }
                    else if ( verticalInput == 0 && jumpPressed )
                    {
                        jumpPressed = false;
                    }
                    break;
                }
            case MovementState.JUMPING:
                {
                    if ( jumpDeltaTime > jumpLeniency || verticalInput == 0 )
                    {
                        //movementModel.StopJumping();
                        movementModel.State = MovementState.JUMPED;
                    }

                    if ( horizontalInput > 0  )
                    {
                        movementModel.ApplyHorizontalForce( 1 );
                    }
                    else if ( -horizontalInput > 0 )
                    {
                        movementModel.ApplyHorizontalForce( -1 );
                    }

                    break;
                }
            case MovementState.JUMPED:
                {
                    if ( horizontalInput > 0 )
                    {
                        movementModel.ApplyHorizontalForce( 1 );
                    }
                    else if ( -horizontalInput > 0 )
                    {
                        movementModel.ApplyHorizontalForce( -1 );
                    }

                    break;
                }
            case MovementState.FALL_FORGIVENESS:
                {
                    if ( fallDeltaTime > fallLeniency )
                    {
                        movementModel.State = MovementState.FALLING;
                    }
                    else if ( verticalInput > 0 && !jumpPressed )
                    {
                        movementModel.State = MovementState.JUMPING;

                        startJumpTime = Time.time;
                        jumpDeltaTime = 0f;
                        jumpPressed = true;
                    }

                    if ( horizontalInput > movementModel.RunningThreshold )
                    {
                        movementModel.MoveHorizontally( 1 );
                    }
                    else if ( -horizontalInput > movementModel.RunningThreshold )
                    {
                        movementModel.MoveHorizontally( -1 );
                    }
                    break;
                }
            case MovementState.FALLING:
                {
                    if ( horizontalInput > 0 )
                    {
                        movementModel.ApplyHorizontalForce( 1 );
                    }
                    else if ( -horizontalInput > 0 )
                    {
                        movementModel.ApplyHorizontalForce( -1 );
                    }

                    if ( verticalInput > 0 && !jumpPressed )
                    {
                        startJumpTime = Time.time;
                        jumpDeltaTime = 0f;
                        jumpPressed = true;

                        movementModel.State = MovementState.JUMPING;
                    }
                    else if ( verticalInput == 0 && jumpPressed )
                    {
                        jumpPressed = false;
                    }

                    break;
                }
            default:
                {

                    break;
                }
        }
    }

    void OnCollisionEnter2D( Collision2D collision )
    {
        if ( collision.collider.tag == "floor" )
        {
            if ( collision.contacts[ 0 ].normal.y > 0.4f )
            {
                movementModel.State = MovementState.STOPPED;
            }
            else if ( collision.contacts[ 0 ].normal.y < -0.4f )
            {
                movementModel.State = MovementState.JUMPED;
            }
        }
    }

    void OnCollisionExit2D( Collision2D collision )
    {
        if ( collision.collider.tag == "floor" )
        {
            if ( movementModel.State == MovementState.MOVING || movementModel.State == MovementState.STOPPED )
            {
                movementModel.State = MovementState.FALL_FORGIVENESS;
                startFallTime = Time.time;
                fallDeltaTime = 0f;
            }
            else if ( movementModel.State == MovementState.JUMPING )
            {
                GameObject jumpGO = GameObject.Instantiate( jumpEffect, new Vector2( transform.position.x, transform.position.y - ( transform.localScale.y / 2 ) ), Quaternion.identity ) as GameObject;

                Destroy( jumpGO, 2.0f );
            }
        }
    }

    #endregion
}