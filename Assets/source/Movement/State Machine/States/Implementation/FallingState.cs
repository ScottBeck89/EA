using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class FallingState : IMovementState
{
    private MovementState state = MovementState.JUMPING;

    private MovementModel model;

    private MovementStateManager manager;

    public MovementState State
    {
        get
        {
            return state;
        }
    }

    public FallingState( MovementModel model, MovementStateManager manager )
    {
        this.model = model;
        this.manager = manager;
    }


    public void OnEnterState()
    {
    }

    public void OnUpdateState(PlayerInputs input)
    {
        if ( input.horizontalInput > model.InputThreshold )
        {
            model.ApplyHorizontalForce( 1 );
        }
        else if ( input.horizontalInput < model.InputThreshold )
        {
            model.ApplyHorizontalForce( -1 );
        }

        if ( input.verticalInput > model.InputThreshold && !model.Jumped )
        {
            manager.ChangeState( MovementState.JUMPING );
        }
    }

    public void OnExitState()
    {

    }
}
