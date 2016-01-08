using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FallForgivenessState : IMovementState
{
    private MovementState state = MovementState.FALL_FORGIVENESS;

    private MovementModel model;

    private MovementStateManager manager;

    private float fallDeltaTime = 0f;

    private float fallLeniency = .2f;

    public MovementState State
    {
        get
        {
            return state;
        }
    }

    public FallForgivenessState( MovementModel model, MovementStateManager manager )
    {
        this.model = model;
        this.manager = manager;
    }


    public void OnEnterState()
    {
        fallLeniency = Time.time;
        fallDeltaTime = 0f;
    }

    public void OnUpdateState(PlayerInputs input)
    {
        fallDeltaTime += Time.fixedDeltaTime;

        if ( fallDeltaTime > model.FallLeniency )
        {
            manager.ChangeState( MovementState.FALLING );
        }
        else if ( input.verticalInput > model.InputThreshold && model.JumpingEnabled )
        {
            manager.ChangeState( MovementState.JUMPING );
        }

        if ( input.horizontalInput > model.InputThreshold )
        {
            model.ApplyHorizontalForce( 1 );
        }
        else if ( input.horizontalInput < -model.InputThreshold )
        {
            model.ApplyHorizontalForce( -1 );
        }
    }

    public void OnExitState()
    {

    }
}
