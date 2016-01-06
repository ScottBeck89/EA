using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AccelState : IMovementState
{
    private MovementState state = MovementState.ACCELERATING;

    private MovementModel model;

    private MovementStateManager manager;

    public MovementState State
    {
        get
        {
            return state;
        }
    }

    public AccelState( MovementModel model, MovementStateManager manager )
    {
        this.model = model;
        this.manager = manager;
    }


    public void OnEnterState()
    {

    }

    public void OnUpdateState(PlayerInputs input)
    {
        if ( input.horizontalInput > model.RunningThreshold )
        {
            model.ApplyHorizontalForce( 1 );
        }
        else if ( input.horizontalInput < -model.RunningThreshold )
        {
            model.ApplyHorizontalForce( -1 );
        }
        else
        {
            manager.ChangeState( MovementState.STOPPED );
        }

        if ( Mathf.Abs( model.MyRigidbody.velocity.x ) >= model.HorizontalTerminalVelocity )
        {
            manager.ChangeState( MovementState.MOVING );
            return;
        }

        if ( input.verticalInput > 0 && !model.Jumped )
        {
            manager.ChangeState( MovementState.JUMPING );
        }
    }

    public void OnExitState()
    {

    }
}