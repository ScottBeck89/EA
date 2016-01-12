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

    /// <summary>
    /// Assumptions: No collisions in this state.
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
            else if ( collision.contacts[ 0 ].normal.y < -0.4f )
            {
                manager.ChangeState( MovementState.FALLING );
            }
            else if ( Mathf.Abs( collision.contacts[ 0 ].normal.x ) > 0.4f )
            {
                manager.ChangeState( MovementState.HUGGING_WALL );
            }
        }
    }
}
