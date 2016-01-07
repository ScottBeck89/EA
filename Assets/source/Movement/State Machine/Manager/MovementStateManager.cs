using System;
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
                    NextState = new FallingState( MovementModel, this );
                    break;
                }
        }

        changedState = true;
    }

    void OnCollisionEnter2D( Collision2D collision )
    {
        if ( collision.collider.tag == "floor" || editMode )
        {
            if ( collision.contacts[ 0 ].normal.y > 0.4f )
            {
                currentFloor = collision.collider;
                ChangeState( MovementState.STOPPED );
            }
            else if ( collision.contacts[ 0 ].normal.y < -0.4f )
            {
                currentFloor = null;
                ChangeState( MovementState.JUMPED );
            }
            else if ( Mathf.Abs( collision.contacts[ 0 ].normal.x ) > .6f && currentFloor == null )
            {
                ChangeState( MovementState.HUGGING_WALL );
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
            if ( ( ( MovementModel.State == MovementState.MOVING || MovementModel.State == MovementState.STOPPED || MovementModel.State == MovementState.ACCELERATING ) 
                && collision.contacts[ 0 ].normal.y > 0.4f ) ||
                MovementModel.State == MovementState.HITTING_WALL || MovementModel.State == MovementState.HUGGING_WALL )
            {
                currentFloor = null;

                ChangeState( MovementState.FALL_FORGIVENESS );
            }
            else if ( MovementModel.State == MovementState.JUMPING && collision.contacts[ 0 ].normal.y > 0.4f )
            {
                currentFloor = null;

                GameObject jumpGO = GameObject.Instantiate( jumpEffect, new Vector2( transform.position.x, transform.position.y - ( transform.localScale.y / 2 ) ), Quaternion.identity ) as GameObject;

                Destroy( jumpGO, 2.0f );
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