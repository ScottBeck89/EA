using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MovementView : MonoBehaviour
{
    public MovementModel model;

    public GameObject GroupContainer;

    public Text horizontalVelocity;

    public Text verticalVelocity;

    public Text absoluteVelocity;

    public Text movementState;

    public Text previousMovementState;

    public Slider absoluteMaxVelocitySlider;

    public Slider terminalVelocitySlider;

    public Slider horizontalTerminalVelocitySlider;

    public Slider horizontalAccelerationSlider;

    public Slider linearDragSlider;

    public Slider GravityScaleSlider;

    public Slider JumpForceSlider;

    public Slider HorizontalJumpForceSlider;

    public Slider JumpVelocitySlider;

    public Slider HorizontalJumpVelocitySlider;

    public Toggle ParabolicJumpToggle;

    public GameObject saveButton;

    void Update()
    {
        horizontalVelocity.text = model.MyRigidbody.velocity.x.ToString();
        verticalVelocity.text = model.MyRigidbody.velocity.y.ToString();
        absoluteVelocity.text = model.MyRigidbody.velocity.magnitude.ToString();
        movementState.text = model.State.ToString();
        previousMovementState.text = model.PreviousState.ToString();
    }


    void Start()
    {
        setValues();
    }

    private IEnumerator setValues()
    {
        yield return new WaitForEndOfFrame();

        if ( absoluteMaxVelocitySlider != null )
        {
            absoluteMaxVelocitySlider.minValue = 10f;
            absoluteMaxVelocitySlider.maxValue = 100f;
            absoluteMaxVelocitySlider.value = model.AbsoluteMaxVelocity;

            absoluteMaxVelocitySlider.onValueChanged.AddListener( AbsoluteMaxVelocityUpdated );
        }

        if ( horizontalTerminalVelocitySlider != null )
        {
            horizontalTerminalVelocitySlider.minValue = 10f;
            horizontalTerminalVelocitySlider.maxValue = 40f;
            horizontalTerminalVelocitySlider.value = model.HorizontalTerminalVelocity;

            horizontalTerminalVelocitySlider.onValueChanged.AddListener( HorizontalTerminalVelocityUpdated );
        }

        if ( terminalVelocitySlider != null )
        {
            terminalVelocitySlider.minValue = 10f;
            terminalVelocitySlider.maxValue = 40f;
            terminalVelocitySlider.value = model.TerminalVelocity;

            terminalVelocitySlider.onValueChanged.AddListener( TerminalVelocityUpdated );
        }

        if ( horizontalAccelerationSlider != null )
        {
            horizontalAccelerationSlider.minValue = 10f;
            horizontalAccelerationSlider.maxValue = 100f;
            horizontalAccelerationSlider.value = model.HorizontalAcceleration;

            horizontalAccelerationSlider.onValueChanged.AddListener( HorizontalAccelerationUpdated );
        }

        if ( linearDragSlider != null )
        {
            linearDragSlider.minValue = 0f;
            linearDragSlider.maxValue = 10f;
            linearDragSlider.value = model.LinearDrag;

            linearDragSlider.onValueChanged.AddListener( LinearDragUpdate );
        }

        if ( GravityScaleSlider != null )
        {
            GravityScaleSlider.minValue = 0f;
            GravityScaleSlider.maxValue = 50f;
            GravityScaleSlider.value = model.GravityScale;

            GravityScaleSlider.onValueChanged.AddListener( GravityScaleUpdate );
        }

        if ( JumpForceSlider != null )
        {
            JumpForceSlider.minValue = 0f;
            JumpForceSlider.maxValue = 100f;
            JumpForceSlider.value = model.JumpForce;

            JumpForceSlider.onValueChanged.AddListener( JumpForceUpdate );
        }

        if ( HorizontalJumpForceSlider != null )
        {
            HorizontalJumpForceSlider.minValue = 0f;
            HorizontalJumpForceSlider.maxValue = 100f;
            HorizontalJumpForceSlider.value = model.HorizontalJumpForce;

            HorizontalJumpForceSlider.onValueChanged.AddListener( HorizontalJumpForceUpdate );
        }

        if ( JumpVelocitySlider != null )
        {
            JumpVelocitySlider.minValue = 0f;
            JumpVelocitySlider.maxValue = 50f;
            JumpVelocitySlider.value = model.JumpMaxVelocity;

            JumpVelocitySlider.onValueChanged.AddListener( JumpVelocityUpdate );
        }

        if ( HorizontalJumpVelocitySlider != null )
        {
            HorizontalJumpVelocitySlider.minValue = 0f;
            HorizontalJumpVelocitySlider.maxValue = 50f;
            HorizontalJumpVelocitySlider.value = model.HorizontalJumpVelocity;

            HorizontalJumpVelocitySlider.onValueChanged.AddListener( HorizontalJumpVelocityUpdate );
        }

        if ( ParabolicJumpToggle != null )
        {
            ParabolicJumpToggle.isOn = model.ParabolicJump;

            ParabolicJumpToggle.onValueChanged.AddListener( ParabolicJumpUpdated );
        }
    }

    public void ToggleModifiers()
    {
        GroupContainer.SetActive( !GroupContainer.activeSelf );

        StartCoroutine( setValues() );
    }

    #region Updaters

    public void AbsoluteMaxVelocityUpdated( float value )
    {
        value = Round( value, 2 );
        absoluteMaxVelocitySlider.value = value;

        if ( model == null )
        {
            print( "attach the move model" );
            return;
        }

        model.AbsoluteMaxVelocity = value;
    }

    public void TerminalVelocityUpdated( float value )
    {
        value = Round( value, 2 );
        terminalVelocitySlider.value = value;

        if ( model == null )
        {
            print( "attach the move model" );
            return;
        }

        model.TerminalVelocity = value;
    }

    public void HorizontalTerminalVelocityUpdated( float value )
    {
        value = Round( value, 2 );
        horizontalTerminalVelocitySlider.value = value;

        if ( model == null )
        {
            print( "attach the move model" );
            return;
        }

        model.HorizontalTerminalVelocity = value;
    }

    public void HorizontalAccelerationUpdated( float value )
    {
        value = Round( value, 2 );
        horizontalAccelerationSlider.value = value;

        if ( model == null )
        {
            print( "attach the move model" );
            return;
        }

        model.HorizontalAcceleration = value;
    }

    public void LinearDragUpdate(float value)
    {
        value = Round( value, 2 );
        linearDragSlider.value = value;

        if ( model == null )
        {
            print( "attach the move model" );
            return;
        }

        model.LinearDrag = value;
    }

    public void GravityScaleUpdate( float value )
    {
        value = Round( value, 2 );
        GravityScaleSlider.value = value;

        if ( model == null )
        {
            print( "attach the move model" );
            return;
        }

        model.GravityScale = value;
    }

    public void JumpForceUpdate( float value )
    {
        value = Round( value, 2 );
        JumpForceSlider.value = value;

        if ( model == null )
        {
            print( "attach the move model" );
            return;
        }

        model.JumpForce = value;
    }

    public void HorizontalJumpForceUpdate( float value )
    {
        value = Round( value, 2 );
        HorizontalJumpForceSlider.value = value;

        if ( model == null )
        {
            print( "attach the move model" );
            return;
        }

        model.HorizontalJumpForce = value;
    }

    public void JumpVelocityUpdate( float value )
    {
        value = Round( value, 2 );
        JumpVelocitySlider.value = value;

        if ( model == null )
        {
            print( "attach the move model" );
            return;
        }

        model.JumpMaxVelocity = value;
    }

    public void HorizontalJumpVelocityUpdate( float value )
    {
        value = Round( value, 2 );
        HorizontalJumpVelocitySlider.value = value;

        if ( model == null )
        {
            print( "attach the move model" );
            return;
        }

        model.HorizontalJumpVelocity = value;
    }

    public void ParabolicJumpUpdated( Boolean value )
    {
        if ( model == null )
        {
            print( "attach the move model" );
            return;
        }

        model.ParabolicJump = value;
    }

    #endregion

    public void SaveValues()
    {
        PlayerPrefs.SetFloat( "AbsoluteMaxVelocity", model.AbsoluteMaxVelocity );
        PlayerPrefs.SetFloat( "TerminalVelocity", model.TerminalVelocity );
        PlayerPrefs.SetFloat( "HorizontalTerminalVelocity", model.HorizontalTerminalVelocity );
        PlayerPrefs.SetFloat( "HorizontalAcceleration", model.HorizontalAcceleration );
        PlayerPrefs.SetFloat( "LinearDrag", model.LinearDrag );
        PlayerPrefs.SetFloat( "GravityScale", model.GravityScale );
        PlayerPrefs.SetFloat( "JumpForce", model.JumpForce );
        PlayerPrefs.SetFloat( "HorizontalJumpForce", model.HorizontalJumpForce );
        PlayerPrefs.SetFloat( "JumpMaxVelocity", model.JumpMaxVelocity );
        PlayerPrefs.SetFloat( "HorizontalJumpVelocity", model.HorizontalJumpVelocity );

        PlayerPrefs.SetString( "ParabolicJump", model.ParabolicJump.ToString() );

        PlayerPrefs.SetString( "UseCustomSettings", "true" );
    }

    public void ResetValues()
    {
        PlayerPrefs.SetString( "UseCustomSettings", "false" );
    }

    public void Quit()
    {
        Application.Quit();
    }

    public static float Round( float value, int digits )
    {
        float mult = Mathf.Pow( 10.0f, ( float ) digits );
        return Mathf.Round( value * mult ) / mult;
    }
}