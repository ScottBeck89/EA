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
            manager.ChangeState( MovementState.FALL_FORGIVENESS );
            model.ApplyHorizontalForce( 1 );
        }
        else if ( input.horizontalInput < -model.InputThreshold && manager.WallHugDirection < 0f )
        {
            manager.ChangeState( MovementState.FALL_FORGIVENESS );
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

    /// <summary>
    /// Assumptions: Colliding with only the walls.
    /// </summary>
    /// <param name="collision"></param>
    /// <param name="isEntering"></param>
    public void CollisionChange( Collision2D collision, CollisionState collisionState )
    {
        if ( collisionState == CollisionState.ENTERING )
        {
            if ( collision.contacts[ 0 ].normal.y > 0.4f )
            {
                manager.ChangeState( MovementState.STOPPED );
            }
        }
        else if ( collisionState == CollisionState.EXITING )
        {
            if ( manager.CurrentCollisions.Count == 0 )
            {
                manager.ChangeState( MovementState.FALL_FORGIVENESS );
            }
        }
    }
}
