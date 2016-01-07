using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class TempInputController : MonoBehaviour
{
    public MovementModel movementModel;

    public MovementStateManager movementManager;

    #region Unity Methods

    void Update()
    {
        if ( Input.GetKey( "space" ) )
        {
            movementManager.resetCharacter();
            movementManager.ChangeState( MovementState.STOPPED );
        }
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxisRaw( "Horizontal" ) + Input.GetAxisRaw( "XBOXLeftJoystickX" );
        float verticalInput = Input.GetAxisRaw( "Vertical" );

        if ( Input.GetButton( "XBOXAButton" ) )
        {
            verticalInput = 1f;
        }
        PlayerInputs inputs = new PlayerInputs();

        inputs.horizontalInput = horizontalInput;
        inputs.verticalInput = verticalInput;

        if ( movementModel == null )
        {
            print( "movement model is null!" );
            return;
        }

        if ( movementManager == null )
        {
            print( "movement model is null!" );
            return;
        }

        movementManager.UpdateState( inputs );
    }

    #endregion
}