using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WallLaunchState : IMovementState
{
    private MovementState state = MovementState.WALL_LAUNCH;

    private MovementModel model;

    private MovementStateManager manager;

    public MovementState State
    {
        get
        {
            return state;
        }
    }

    public WallLaunchState( MovementModel model, MovementStateManager manager )
    {
        this.model = model;
        this.manager = manager;
    }


    public void OnEnterState()
    {
        model.DisableJumping();

        GameObject jumpGO = GameObject.Instantiate( manager.jumpEffect,
            new Vector2( manager.transform.position.x, manager.transform.position.y - ( manager.transform.localScale.y / 2 ) ), Quaternion.identity ) as GameObject;

        jumpGO.transform.eulerAngles = new Vector3( 0, 0, -1 * Mathf.Round( manager.WallHugDirection ) * 45 );

        GameObject.Destroy( jumpGO, 2.0f );
    }

    public void OnUpdateState( PlayerInputs input )
    {
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
    /// Assumptions: First only wall collisions, then no collisions.
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
