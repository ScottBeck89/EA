using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HuggingWallState : IMovementState
{
    private MovementState state = MovementState.HUGGING_WALL;

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
        if ( model.PreviousState == MovementState.JUMPING )
        {
            //model.ApplyVelocityTransfer( manager.ImpactVelocity, manager.ImpactAngle );
        }
    }

    public void OnUpdateState(PlayerInputs input)
    {
        if ( input.verticalInput < model.InputThreshold && !model.JumpingEnabled )
        {
            model.EnableJumping();
        }

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

        if ( input.verticalInput > model.InputThreshold && model.JumpingEnabled )
        {
            model.WallJump();
            manager.ChangeState( MovementState.WALL_LAUNCH );
        }
    }

    public void OnExitState()
    {

    }
}
