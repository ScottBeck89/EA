using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StoppedState : IMovementState
{
    private MovementState state = MovementState.STOPPED;

    private MovementModel model;

    public MovementState State
    {
        get
        {
            return state;
        }
    }

    public StoppedState( MovementModel model )
    {
        this.model = model;
    }


    public void OnEnterState( float transitionTime )
    {

    }

    public void OnUpdateState(PlayerInputs input)
    {
        if ( input.horizontalInput > model.RunningThreshold || input.horizontalInput < -model.RunningThreshold )
        {
            model.State = MovementState.ACCELERATING;
        }
        else
        {
            model.StopMovement();
        }

        if ( input.verticalInput > 0 && !model.Jumped )
        {
            model.Jump();
            model.State = MovementState.JUMPING;
        }
        else if ( input.verticalInput == 0 && model.Jumped )
        {
            model.StopJump();
        }
    }

    public void OnExitState()
    {

    }
}