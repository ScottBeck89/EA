using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AccelState : MonoBehaviour, IMovementState
{
    private MovementState state = MovementState.ACCELERATING;

    private MovementModel model;

    private MovementStateManager manager;

    public MovementState State
    {
        get
        {
            return state;
        }
    }

    public AccelState( MovementModel model, MovementStateManager manager )
    {
        this.model = model;
        this.manager = manager;
    }


    public void OnEnterState()
    {

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
        else
        {
            manager.ChangeState( MovementState.STOPPED );
        }

        if ( input.verticalInput > model.InputThreshold && !model.Jumped )
        {
            manager.ChangeState( MovementState.JUMPING );
            return;
        }

        if ( Mathf.Abs( model.MyRigidbody.velocity.x ) >= model.HorizontalTerminalVelocity )
        {
            manager.ChangeState( MovementState.MOVING );
            return;
        }
    }

    public void OnExitState()
    {

    }
    /*
    void OnCollisionEnter2D( Collision2D collision )
    {
        if ( collision.collider.tag == "floor" || manager.EditMode )
        {
            if ( collision.contacts[ 0 ].normal.y > 0.4f )
            {
                currentFloor = collision.collider;
                MovementModel.State = MovementState.STOPPED;
            }
            else if ( collision.contacts[ 0 ].normal.y < -0.4f )
            {
                currentFloor = null;
                MovementModel.State = MovementState.JUMPED;
            }
            else if ( Mathf.Abs( collision.contacts[ 0 ].normal.x ) > .6f && currentFloor == null )
            {
                wallHugDirection = collision.contacts[ 0 ].normal.x;
                MovementModel.State = MovementState.HITTING_WALL;
            }
        }
        else if ( collision.collider.tag == "Environment" )
        {
            manager.resetCharacter();
        }
    }

    void OnCollisionExit2D( Collision2D collision )
    {
        if ( collision.collider.tag == "floor" || editMode )
        {
            if ( ( ( MovementModel.State == MovementState.MOVING || MovementModel.State == MovementState.STOPPED || MovementModel.State == MovementState.ACCELERATING )
                && collision.contacts[ 0 ].normal.y > 0.4f ) ||
                MovementModel.State == MovementState.HITTING_WALL || MovementModel.State == MovementState.HUGGING_WALL )
            {
                currentFloor = null;

                ChangeState( MovementState.FALL_FORGIVENESS );
            }
            else if ( MovementModel.State == MovementState.JUMPING && collision.contacts[ 0 ].normal.y > 0.4f )
            {
                currentFloor = null;

                GameObject jumpGO = GameObject.Instantiate( jumpEffect, new Vector2( transform.position.x, transform.position.y - ( transform.localScale.y / 2 ) ), Quaternion.identity ) as GameObject;

                Destroy( jumpGO, 2.0f );
            }
        }
    }*/
}