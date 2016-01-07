using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MovementController : MonoBehaviour
{
    public MovementModel movementModel;

    public GameObject jumpEffect;

    public GameObject temp;

    private Vector3 StartingPosition;

    private float terminalVelocity;

    private float startJumpTime = 0f;

    private float jumpDeltaTime = 0f;

    private float jumpLeniency = .3f;

    private float startFallTime = 0f;

    private float fallDeltaTime = 0f;

    private float fallLeniency = .2f;

    private Boolean jumpPressed = false;

    private Boolean editMode = false;

    private float wallHugDirection = 0f;

    private Collider2D currentFloor = null;

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

    public void editModeToggle()
    {
        editMode = !editMode;
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
                    if ( horizontalInput > movementModel.InputThreshold )
                    {
                        movementModel.State = MovementState.ACCELERATING;
                    }
                    else if ( -horizontalInput > movementModel.InputThreshold )
                    {
                        movementModel.State = MovementState.ACCELERATING;
                    }
                    else
                    {
                        movementModel.StopMovement();
                    }

                    if ( verticalInput > movementModel.InputThreshold && !jumpPressed )
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
                    if ( horizontalInput > movementModel.InputThreshold )
                    {
                        movementModel.ApplyHorizontalForce( 1 );
                    }
                    else if ( -horizontalInput > movementModel.InputThreshold )
                    {
                        movementModel.ApplyHorizontalForce( -1 );
                    }
                    else
                    {
                        movementModel.State = MovementState.STOPPED;
                    }

                    if ( verticalInput > movementModel.InputThreshold && !jumpPressed )
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
                    if ( horizontalInput > movementModel.InputThreshold )
                    {
                        movementModel.MoveHorizontally( 1 );
                    }
                    else if ( -horizontalInput > movementModel.InputThreshold )
                    {
                        movementModel.MoveHorizontally( -1 );
                    }
                    else
                    {
                        movementModel.State = MovementState.STOPPED;
                    }

                    if ( verticalInput > movementModel.InputThreshold && !jumpPressed )
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
                        movementModel.State = MovementState.JUMPED;
                    }

                    if ( horizontalInput > movementModel.InputThreshold )
                    {
                        movementModel.ApplyHorizontalForce( 1 );
                    }
                    else if ( -horizontalInput > movementModel.InputThreshold )
                    {
                        movementModel.ApplyHorizontalForce( -1 );
                    }

                    break;
                }
            case MovementState.JUMPED:
                {
                    if ( horizontalInput > movementModel.InputThreshold )
                    {
                        movementModel.ApplyHorizontalForce( 1 );
                    }
                    else if ( -horizontalInput > movementModel.InputThreshold )
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
                    else if ( verticalInput > movementModel.InputThreshold && !jumpPressed )
                    {
                        movementModel.State = MovementState.JUMPING;

                        startJumpTime = Time.time;
                        jumpDeltaTime = 0f;
                        jumpPressed = true;
                    }

                    if ( horizontalInput > movementModel.InputThreshold )
                    {
                        movementModel.MoveHorizontally( 1 );
                    }
                    else if ( -horizontalInput > movementModel.InputThreshold )
                    {
                        movementModel.MoveHorizontally( -1 );
                    }
                    break;
                }
            case MovementState.FALLING:
                {
                    if ( horizontalInput > movementModel.InputThreshold )
                    {
                        movementModel.ApplyHorizontalForce( 1 );
                    }
                    else if ( -horizontalInput > movementModel.InputThreshold )
                    {
                        movementModel.ApplyHorizontalForce( -1 );
                    }

                    if ( verticalInput > movementModel.InputThreshold && !jumpPressed )
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
            case MovementState.HITTING_WALL:
                {
                    if ( movementModel.PreviousState != MovementState.JUMPING || verticalInput == 0 || jumpDeltaTime > jumpLeniency )
                    {
                        movementModel.State = MovementState.HUGGING_WALL;
                        break;
                    }

                    if ( horizontalInput > movementModel.InputThreshold && wallHugDirection > 0f )
                    {
                        //move off the wall
                        movementModel.State = MovementState.FALL_FORGIVENESS;
                    }
                    else if ( horizontalInput < movementModel.InputThreshold && wallHugDirection < 0f )
                    {
                        //move off the wall
                        movementModel.State = MovementState.FALL_FORGIVENESS;
                    }
                    else if ( Mathf.Abs( horizontalInput ) > movementModel.InputThreshold )
                    {
                        movementModel.WallHangFactor = 0.75f;
                    }
                    else
                    {
                        movementModel.WallHangFactor = 1f;
                    }

                    break;
                }
            case MovementState.HUGGING_WALL:
                {
                    if ( horizontalInput > movementModel.InputThreshold && wallHugDirection > 0f )
                    {
                        //move off the wall
                        movementModel.State = MovementState.FALL_FORGIVENESS;
                    }
                    else if ( horizontalInput < movementModel.InputThreshold && wallHugDirection < 0f )
                    {
                        //move off the wall
                        movementModel.State = MovementState.FALL_FORGIVENESS;
                    }
                    else if ( Mathf.Abs( horizontalInput ) > movementModel.InputThreshold )
                    {
                        movementModel.WallHangFactor = 0.25f;
                    }
                    else
                    {
                        movementModel.WallHangFactor = 1f;
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
        if ( collision.collider.tag == "floor" || editMode )
        {
            foreach ( ContactPoint2D cp in collision.contacts )
            {
                GameObject go = GameObject.Instantiate( temp, cp.point, Quaternion.identity ) as GameObject;

                Destroy( go, 5.0f );
            }
            if ( collision.contacts[ 0 ].normal.y > 0.4f )
            {
                currentFloor = collision.collider;
                movementModel.State = MovementState.STOPPED;
            }
            else if ( collision.contacts[ 0 ].normal.y < -0.4f )
            {
                currentFloor = null;
                movementModel.State = MovementState.JUMPED;
            }
            else if ( Mathf.Abs( collision.contacts[ 0 ].normal.x ) > .6f && currentFloor == null )
            {
                wallHugDirection = collision.contacts[ 0 ].normal.x;
                movementModel.State = MovementState.HITTING_WALL;
            }
        }
        else if ( collision.collider.tag == "Environment" )
        {
            resetCharacter();
        }
    }

    void OnCollisionExit2D( Collision2D collision )
    {
        if ( collision.collider.tag == "floor" || editMode )
        {
            if ( movementModel.State == MovementState.MOVING || movementModel.State == MovementState.STOPPED || 
                movementModel.State == MovementState.ACCELERATING && collision.contacts[ 0 ].normal.y > 0.4f || 
                movementModel.State == MovementState.HITTING_WALL || movementModel.State == MovementState.HUGGING_WALL )
            {
                currentFloor = null;

                movementModel.State = MovementState.FALL_FORGIVENESS;
                startFallTime = Time.time;
                fallDeltaTime = 0f;
            }
            else if ( movementModel.State == MovementState.JUMPING && collision.contacts[ 0 ].normal.y > 0.4f )
            {
                currentFloor = null;

                GameObject jumpGO = GameObject.Instantiate( jumpEffect, new Vector2( transform.position.x, transform.position.y - ( transform.localScale.y / 2 ) ), Quaternion.identity ) as GameObject;

                Destroy( jumpGO, 2.0f );
            }
        }
    }

    #endregion
}