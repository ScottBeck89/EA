using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum CollisionState
{
    ENTERING,
    STAYING,
    EXITING
}

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

    private List<Collider2D> currentCollisions = new List<Collider2D>();

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

    public List<Collider2D> CurrentCollisions
    {
        get
        {
            return currentCollisions;
        }
        set
        {
            currentCollisions = value;
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

    public void LaunchPlayer( float force )
    {
        ChangeStateImmediate( MovementState.FALLING );
        MovementModel.VerticalLaunch( force );
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

            if ( !currentCollisions.Contains( collision.collider ) )
            {
                currentCollisions.Add( collision.collider );
            }

            if ( Mathf.Abs( collision.contacts[ 0 ].normal.x ) > 0.4f )
            {
                //for future implementation of wall velocity transfer.
                //impactAngle = SMath.VectorToDegree( collision.relativeVelocity );
                //impactVelocity = collision.relativeVelocity;

                wallHugDirection = collision.contacts[ 0 ].normal.x;
            }

            CurrentState.CollisionChange( collision, CollisionState.ENTERING );
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
            CurrentState.CollisionChange( collision, CollisionState.STAYING );
        }
    }

    void OnCollisionExit2D( Collision2D collision )
    {
         if ( collision.collider.tag == "floor" || editMode )
        {
            if ( currentCollisions.Contains( collision.collider ) )
            {
                currentCollisions.Remove( collision.collider );
            }

            CurrentState.CollisionChange( collision, CollisionState.EXITING );
        }
    }
}

public struct PlayerInputs
{
    public float horizontalInput;
    public float verticalInput;
    public Boolean jumpPressed;
}