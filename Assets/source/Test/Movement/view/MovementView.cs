using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MovementView : MonoBehaviour
{
    public MovementController moveController;

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

    void Update()
    {
        horizontalVelocity.text = moveController.MovementModel.MyRigidbody.velocity.x.ToString();
        verticalVelocity.text = moveController.MovementModel.MyRigidbody.velocity.y.ToString();
        absoluteVelocity.text = moveController.MovementModel.MyRigidbody.velocity.magnitude.ToString();
        movementState.text = moveController.movementModel.State.ToString();
        previousMovementState.text = moveController.movementModel.PreviousState.ToString();
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
            absoluteMaxVelocitySlider.value = moveController.movementModel.AbsoluteMaxVelocity;

            absoluteMaxVelocitySlider.onValueChanged.AddListener( AbsoluteMaxVelocityUpdated );
        }

        if ( horizontalTerminalVelocitySlider != null )
        {
            horizontalTerminalVelocitySlider.minValue = 10f;
            horizontalTerminalVelocitySlider.maxValue = 40f;
            horizontalTerminalVelocitySlider.value = moveController.movementModel.HorizontalTerminalVelocity;

            horizontalTerminalVelocitySlider.onValueChanged.AddListener( HorizontalTerminalVelocityUpdated );
        }

        if ( terminalVelocitySlider != null )
        {
            terminalVelocitySlider.minValue = 10f;
            terminalVelocitySlider.maxValue = 40f;
            terminalVelocitySlider.value = moveController.movementModel.TerminalVelocity;

            terminalVelocitySlider.onValueChanged.AddListener( TerminalVelocityUpdated );
        }

        if ( horizontalAccelerationSlider != null )
        {
            horizontalAccelerationSlider.minValue = 10f;
            horizontalAccelerationSlider.maxValue = 100f;
            horizontalAccelerationSlider.value = moveController.movementModel.HorizontalAcceleration;

            horizontalAccelerationSlider.onValueChanged.AddListener( HorizontalAccelerationUpdated );
        }

        if ( linearDragSlider != null )
        {
            linearDragSlider.minValue = 0f;
            linearDragSlider.maxValue = 10f;
            linearDragSlider.value = moveController.movementModel.LinearDrag;

            linearDragSlider.onValueChanged.AddListener( LinearDragUpdate );
        }

        if ( GravityScaleSlider != null )
        {
            GravityScaleSlider.minValue = 0f;
            GravityScaleSlider.maxValue = 50f;
            GravityScaleSlider.value = moveController.movementModel.GravityScale;

            GravityScaleSlider.onValueChanged.AddListener( GravityScaleUpdate );
        }
    }

    public void ToggleModifiers()
    {
        absoluteMaxVelocitySlider.gameObject.SetActive( !absoluteMaxVelocitySlider.gameObject.activeSelf );
        terminalVelocitySlider.gameObject.SetActive( !terminalVelocitySlider.gameObject.activeSelf );
        horizontalTerminalVelocitySlider.gameObject.SetActive( !horizontalTerminalVelocitySlider.gameObject.activeSelf );
        horizontalAccelerationSlider.gameObject.SetActive( !horizontalAccelerationSlider.gameObject.activeSelf );
        linearDragSlider.gameObject.SetActive( !linearDragSlider.gameObject.activeSelf );
        GravityScaleSlider.gameObject.SetActive( !GravityScaleSlider.gameObject.activeSelf );
        StartCoroutine( setValues() );
    }

    public void AbsoluteMaxVelocityUpdated( float value )
    {
        value = Round( value, 2 );
        absoluteMaxVelocitySlider.value = value;

        if ( moveController == null )
        {
            print( "attach the move controller" );
            return;
        }

        moveController.MovementModel.AbsoluteMaxVelocity = value;
    }

    public void TerminalVelocityUpdated( float value )
    {
        value = Round( value, 2 );
        terminalVelocitySlider.value = value;

        if ( moveController == null )
        {
            print( "attach the move controller" );
            return;
        }

        moveController.MovementModel.TerminalVelocity = value;
    }

    public void HorizontalTerminalVelocityUpdated( float value )
    {
        value = Round( value, 2 );
        horizontalTerminalVelocitySlider.value = value;

        if ( moveController == null )
        {
            print( "attach the move controller" );
            return;
        }

        moveController.MovementModel.HorizontalTerminalVelocity = value;
    }

    public void HorizontalAccelerationUpdated( float value )
    {
        value = Round( value, 2 );
        horizontalAccelerationSlider.value = value;

        if ( moveController == null )
        {
            print( "attach the move controller" );
            return;
        }

        moveController.MovementModel.HorizontalAcceleration = value;
    }

    public void LinearDragUpdate(float value)
    {
        value = Round( value, 2 );
        linearDragSlider.value = value;

        if ( moveController == null )
        {
            print( "attach the move controller" );
            return;
        }

        moveController.MovementModel.LinearDrag = value;
    }

    public void GravityScaleUpdate( float value )
    {
        value = Round( value, 2 );
        GravityScaleSlider.value = value;

        if ( moveController == null )
        {
            print( "attach the move controller" );
            return;
        }

        moveController.MovementModel.GravityScale = value;
    }

    public void SaveValues()
    {
        PlayerPrefs.SetFloat( "AbsoluteMaxVelocity", moveController.MovementModel.AbsoluteMaxVelocity );
        PlayerPrefs.SetFloat( "TerminalVelocity", moveController.MovementModel.TerminalVelocity );
        PlayerPrefs.SetFloat( "HorizontalTerminalVelocity", moveController.MovementModel.HorizontalTerminalVelocity );
        PlayerPrefs.SetFloat( "HorizontalAcceleration", moveController.MovementModel.HorizontalAcceleration );
        PlayerPrefs.SetFloat( "LinearDrag", moveController.MovementModel.LinearDrag );
        PlayerPrefs.SetFloat( "GravityScale", moveController.MovementModel.GravityScale );

        PlayerPrefs.SetString( "UseCustomSettings", "true" );
    }

    public static float Round( float value, int digits )
    {
        float mult = Mathf.Pow( 10.0f, ( float ) digits );
        return Mathf.Round( value * mult ) / mult;
    }
}