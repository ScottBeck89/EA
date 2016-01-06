using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class JumpingState: IMovementState
{
    private MovementState state = MovementState.JUMPING;

    private MovementModel model;

    private MovementStateManager manager;

    private float jumpStartTime = 0f;

    private float jumpDeltaTime = 0f;

    public MovementState State
    {
        get
        {
            return state;
        }
    }

    public JumpingState( MovementModel model, MovementStateManager manager )
    {
        this.model = model;
        this.manager = manager;
    }


    public void OnEnterState()
    {
        jumpStartTime = Time.time;
        jumpDeltaTime = 0f;
        model.Jump();
    }

    public void OnUpdateState(PlayerInputs input)
    {
        jumpDeltaTime += Time.fixedDeltaTime;

        if ( jumpDeltaTime > model.JumpLeniency || input.verticalInput == 0 )
        {
            manager.ChangeState( MovementState.JUMPED );
        }

        if ( input.horizontalInput > 0 )
        {
            model.ApplyHorizontalForce( 1 );
        }
        else if ( input.horizontalInput < 0 )
        {
            model.ApplyHorizontalForce( -1 );
        }
    }

    public void OnExitState()
    {

    }
}
