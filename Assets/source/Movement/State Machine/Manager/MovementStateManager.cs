using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    public MovementState StartingState = MovementState.STOPPED;

    public MovementModel MovementModel;

    public GameObject jumpEffect;

    private IMovementState PreviousState;

    private IMovementState CurrentState;

    private IMovementState NextState;

    private Boolean changedState = false;

    private Boolean editMode = false;

    private Collider2D currentFloor = null;

    private List<Collider2D> currentFloors = new List<Collider2D>();

    private Vector3 StartingPosition;

    private float wallHugDirection = 0f;

    private Vector2 impactVelocity = Vector2.zero;

    private float impactAngle = 0f;

    #region Properties

    public float WallHugDirection
    {
        get
        {
            return wallHugDirection;
        }
    }

    public Boolean EditMode
    {
        get
        {
            return editMode;
        }
    }

    public List<Collider2D> CurrentFloors
    {
        get
        {
            return currentFloors;
        }
        set
        {
            currentFloors = value;
        }
    }

    public Vector2 ImpactVelocity
    {
        get
        {
            return impactVelocity;
        }
    }

    public float ImpactAngle
    {
        get
        {
            return impactAngle;
        }
    }

    #endregion

    private void Start()
    {
        switch ( StartingState )
        {
            case MovementState.STOPPED:
                {
                    CurrentState = new StoppedState( MovementModel, this );

                    break;
                }
        }

        PreviousState = CurrentState;
        CurrentState.OnEnterState();

        StartingPosition = transform.position;
    }

    public void editModeToggle()
    {
        editMode = !editMode;
    }

    public void resetCharacter()
    {
        transform.position = StartingPosition;
        MovementModel.StopMovement();
    }

    public void UpdateState( PlayerInputs input )
    {
        if ( changedState )
        {
            CurrentState.OnExitState();

            PreviousState = CurrentState;
            CurrentState = NextState;
            MovementModel.State = CurrentState.State;

            CurrentState.OnEnterState();

            changedState = false;
        }

        CurrentState.OnUpdateState( input );
    }

    public void ChangeState( MovementState targetState )
    {
        switch ( targetState )
        {
            case MovementState.STOPPED:
                {
                    NextState = new StoppedState( MovementModel, this );
                    break;
                }
            case MovementState.ACCELERATING:
                {
                    NextState = new AccelState( MovementModel, this );
                    break;
                }
            case MovementState.MOVING:
                {
                    NextState = new MovingState( MovementModel, this );
                    break;
                }
            case MovementState.JUMPING:
                {
                    NextState = new JumpingState( MovementModel, this );
                    break;
                }
            case MovementState.FALL_FORGIVENESS:
                {
                    NextState = new FallForgivenessState( MovementModel, this );
                    break;
                }
            case MovementState.FALLING:
                {
                    NextState = new FallingState( MovementModel, this );
                    break;
                }
            case MovementState.HUGGING_WALL:
                {
                    NextState = new HuggingWallState( MovementModel, this );
                    break;
                }
            case MovementState.WALL_LAUNCH:
                {
                    NextState = new WallLaunchState( MovementModel, this );
                    break;
                }
        }

        changedState = true;
    }

    public void ChangeStateImmediate( MovementState targetState )
    {
        switch ( targetState )
        {
            case MovementState.STOPPED:
                {
                    NextState = new StoppedState( MovementModel, this );
                    break;
                }
            case MovementState.ACCELERATING:
                {
                    NextState = new AccelState( MovementModel, this );
                    break;
                }
            case MovementState.MOVING:
                {
                    NextState = new MovingState( MovementModel, this );
                    break;
                }
            case MovementState.JUMPING:
                {
                    NextState = new JumpingState( MovementModel, this );
                    break;
                }
            case MovementState.FALL_FORGIVENESS:
                {
                    NextState = new FallForgivenessState( MovementModel, this );
                    break;
                }
            case MovementState.FALLING:
                {
                    NextState = new FallingState( MovementModel, this );
                    break;
                }
            case MovementState.HUGGING_WALL:
                {
                    NextState = new HuggingWallState( MovementModel, this );
                    break;
                }
            case MovementState.WALL_LAUNCH:
                {
                    NextState = new WallLaunchState( MovementModel, this );
                    break;
                }
        }

        CurrentState.OnExitState();

        PreviousState = CurrentState;
        CurrentState = NextState;
        MovementModel.State = CurrentState.State;

        CurrentState.OnEnterState();
    }

    void OnCollisionEnter2D( Collision2D collision )
    {
        if ( collision.collider.tag == "floor" || editMode )
        {
            MovementModel.Landed();

            if ( collision.contacts[ 0 ].normal.y > 0.4f )
            {
                currentFloor = collision.collider;
                ChangeStateImmediate( MovementState.STOPPED );
            }
            else if ( collision.contacts[ 0 ].normal.y < -0.4f )
            {
                ChangeStateImmediate( MovementState.FALLING );
            }
            else if ( Mathf.Abs( collision.contacts[ 0 ].normal.x ) > 0.4f && ( currentFloor == null || MovementModel.State == MovementState.FALLING ) )
            {
                impactAngle = SMath.VectorToDegree( collision.relativeVelocity );
                impactVelocity = collision.relativeVelocity;

                wallHugDirection = collision.contacts[ 0 ].normal.x;
                ChangeStateImmediate( MovementState.HUGGING_WALL );
            }
        }
        else if ( collision.collider.tag == "Environment" )
        {
            resetCharacter();
        }
    }

    void OnCollisionStay2D( Collision2D collision )
    {
        if ( collision.collider.tag == "floor" || editMode )
        {
            if ( MovementModel.State == MovementState.STOPPED && Mathf.Abs( collision.contacts[ 0 ].normal.x ) > 0.4f && currentFloor == null )
            {
                ChangeState( MovementState.FALL_FORGIVENESS );
            }
            else if ( currentFloor == null && Mathf.Abs( collision.contacts[ 0 ].normal.x ) > 0.4f && MovementModel.State == MovementState.FALLING ) 
            {
                wallHugDirection = collision.contacts[ 0 ].normal.x;
                ChangeState( MovementState.HUGGING_WALL );
            }
        }
    }

    void OnCollisionExit2D( Collision2D collision )
    {
        if ( collision.collider.tag == "floor" || editMode )
        {
            if ( ( MovementModel.State != MovementState.JUMPING && collision.contacts[ 0 ].normal.y > 0.4f ) ||
                ( MovementModel.State == MovementState.HITTING_WALL || MovementModel.State == MovementState.HUGGING_WALL && currentFloor == null ) )
            {
                currentFloor = null;
                wallHugDirection = 0f;

                ChangeState( MovementState.FALL_FORGIVENESS );
            }
            else if ( MovementModel.State == MovementState.JUMPING && collision.contacts[ 0 ].normal.y > 0.4f )
            {
                currentFloor = null;
                wallHugDirection = 0f;

                if ( MovementModel.ParabolicJump )
                {
                    ChangeState( MovementState.FALLING );
                }
            }
        }
    }
}

public struct PlayerInputs
{
    public float horizontalInput;
    public float verticalInput;
    public Boolean jumpPressed;
}