using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StoppedState : IMovementState
{
    private MovementState state = MovementState.STOPPED;

    private MovementModel model;

    private MovementStateManager manager;

    public MovementState State
    {
        get
        {
            return state;
        }
    }

    public StoppedState( MovementModel model, MovementStateManager manager )
    {
        this.model = model;
        this.manager = manager;
    }


    public void OnEnterState()
    {
    }

    public void OnUpdateState(PlayerInputs input)
    {
        if ( input.verticalInput < model.InputThreshold && !model.JumpingEnabled )
        {
            model.EnableJumping();
        }

        if ( input.horizontalInput > model.InputThreshold || input.horizontalInput < -model.InputThreshold )
        {
            manager.ChangeState( MovementState.ACCELERATING );
            return;
        }
        else
        {
            model.StopHorizontalMovement();
        }

        if ( input.verticalInput > model.InputThreshold && model.JumpingEnabled )
        {
            manager.ChangeState( MovementState.JUMPING );
        }
    }

    public void OnExitState()
    {

    }
}