using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HuggingWallState : IMovementState
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

    public HuggingWallState( MovementModel model, MovementStateManager manager )
    {
        this.model = model;
        this.manager = manager;
    }

    public void OnEnterState()
    {

    }

    public void OnUpdateState(PlayerInputs input)
    {
        if ( input.horizontalInput > model.InputThreshold && manager.WallHugDirection > 0f )
        {
            model.ApplyHorizontalForce( 1 );
        }
        else if ( input.horizontalInput < -model.InputThreshold && manager.WallHugDirection < 0f )
        {
            model.ApplyHorizontalForce( -1 );
        }
        else if ( Mathf.Abs( input.horizontalInput ) > model.InputThreshold )
        {
            model.WallHangFactor = 0.25f;
        }
        else
        {
            model.WallHangFactor = 1f;
        }
    }

    public void OnExitState()
    {

    }
}
