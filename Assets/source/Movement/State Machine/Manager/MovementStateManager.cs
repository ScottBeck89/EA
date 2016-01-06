using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    public MovementState StartingState = MovementState.STOPPED;

    public MovementModel MovementModel;

    private IMovementState PreviousState;

    private IMovementState CurrentState;

    private IMovementState NextState;

    private Boolean changedState = false;

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

    public void UpdateState( PlayerInputs input )
    {
        if ( changedState )
        {
            CurrentState.OnExitState();

            PreviousState = CurrentState;
            CurrentState = NextState;

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
        }
    }
}

public struct PlayerInputs
{
    public float horizontalInput;
    public float verticalInput;
    public Boolean jumpPressed;
}