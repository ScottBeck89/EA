using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MovementStateManager : MonoBehaviour
{
    public MovementState StartingState = MovementState.STOPPED;

    public MovementModel MovementModel;

    private IMovementState CurrentState;

    protected Boolean jumpPressed = false;

    private void Start()
    {
        switch ( StartingState )
        {
            case MovementState.STOPPED:
                {
                    CurrentState = new StoppedState( MovementModel );
                    CurrentState.OnEnterState( Time.time );

                    break;
                }
        }
    }

    public void UpdateState( PlayerInputs input )
    {
        CurrentState.OnUpdateState( input );
    }

    public void ChangeState( MovementState targetState )
    {
        CurrentState.OnExitState();

        switch ( targetState )
        {
            case MovementState.STOPPED:
                {
                    CurrentState = new StoppedState( MovementModel );

                    break;
                }
        }

        CurrentState.OnEnterState( Time.time );
    }
}

public struct PlayerInputs
{
    public float horizontalInput;
    public float verticalInput;
    public Boolean jumpPressed;
}