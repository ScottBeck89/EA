using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IMovementState
{
    /// <summary>
    /// The current state the controller is in.
    /// </summary>
    MovementState State
    {
        get;
    }

    /// <summary>
    /// Enters a state.
    /// </summary>
    /// <param name="transitionTime">The time at which the state was entered</param>
    void OnEnterState( float transitionTime );

    /// <summary>
    /// Handles input based on the state's requirements.
    /// </summary>
    /// <param name="input"></param>
    void OnUpdateState( PlayerInputs input );

    /// <summary>
    /// Leaves a state.
    /// </summary>
    void OnExitState();
}