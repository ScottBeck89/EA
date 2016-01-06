using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MovingState: IMovementState
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
        if ( input.horizontalInput > model.RunningThreshold )
        {
            model.MoveHorizontally( 1 );
        }
        else if ( input.horizontalInput < -model.RunningThreshold )
        {
            model.MoveHorizontally( -1 );
        }
        else
        {
            manager.ChangeState( MovementState.STOPPED );
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
