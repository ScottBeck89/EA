using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MovingState : IMovementState
{
    private MovementState state = MovementState.MOVING;

    private MovementModel model;

    private MovementStateManager manager;

    public MovementState State
    {
        get
        {
            return state;
        }
    }

    public MovingState( MovementModel model, MovementStateManager manager )
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

        if ( input.horizontalInput > model.InputThreshold )
        {
            model.MoveHorizontally( 1 );
        }
        else if ( input.horizontalInput < -model.InputThreshold )
        {
            model.MoveHorizontally( -1 );
        }
        else
        {
            model.StopHorizontalMovement();
            manager.ChangeState( MovementState.STOPPED );
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
