using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StoppedState : IMovementState
{
    private MovementState state = MovementState.STOPPED;

    private MovementModel model;

    private MovementStateManager manager;

    public MovementState State
    {
        get
        {
            return state;
        }
    }

    public StoppedState( MovementModel model, MovementStateManager manager )
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

        //catch for no collisions, just in case
        if ( manager.CurrentCollisions.Count == 0 )
        {
            manager.ChangeState( MovementState.FALL_FORGIVENESS );
        }

        if ( input.horizontalInput > model.InputThreshold || input.horizontalInput < -model.InputThreshold )
        {
            manager.ChangeStateImmediate( MovementState.ACCELERATING );
        }
        else
        {
            model.StopHorizontalMovement();
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
    /// Assumptions: Colliding with the ground, possibly colliding with a wall.
    /// </summary>
    /// <param name="collision"></param>
    /// <param name="isEntering"></param>
    public void CollisionChange( Collision2D collision, CollisionState collisionState )
    {
        if ( collisionState == CollisionState.EXITING )
        {
            if ( manager.CurrentCollisions.Count == 0 )
            {
                manager.ChangeState( MovementState.FALL_FORGIVENESS );
            }
            else if ( manager.CurrentCollisions.Count == 1 && Mathf.Abs( collision.contacts[ 0 ].normal.x ) > 0.4f )
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