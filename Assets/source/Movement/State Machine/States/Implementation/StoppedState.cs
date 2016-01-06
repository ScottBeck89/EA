﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        model.StopMovement();
    }

    public void OnUpdateState(PlayerInputs input)
    {
        if ( input.horizontalInput > model.RunningThreshold || input.horizontalInput < -model.RunningThreshold )
        {
            manager.ChangeState( MovementState.ACCELERATING );
            return;
        }
        else
        {
            model.StopMovement();
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