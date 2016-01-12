using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FallingState : IMovementState
{
    private MovementState state = MovementState.FALLING;

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
        if ( !model.MidAirJumpingEnabled )
        {
            model.DisableJumping();
        }
    }

    public void OnUpdateState(PlayerInputs input)
    {
        if ( input.horizontalInput > model.InputThreshold )
        {
            model.ApplyHorizontalForce( 1 );
        }
        else if ( input.horizontalInput < -model.InputThreshold )
        {
            model.ApplyHorizontalForce( -1 );
        }

        if ( input.verticalInput > model.InputThreshold && model.JumpingEnabled )
        {
            manager.ChangeState( MovementState.JUMPING );
        }
    }

    public void OnExitState()
    {

    }

    /// <summary>
    /// Assumptions: No collisions
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
            else if ( Mathf.Abs( collision.contacts[ 0 ].normal.x ) > 0.4f )
            {
                manager.ChangeState( MovementState.HUGGING_WALL );
            }
        }
        else if ( collisionState == CollisionState.STAYING )
        {
            if ( manager.CurrentCollisions.Count == 1 && Mathf.Abs( collision.contacts[ 0 ].normal.x ) > 0.4f )
            {
                manager.ChangeState( MovementState.HUGGING_WALL );
            }
        }
    }
}
