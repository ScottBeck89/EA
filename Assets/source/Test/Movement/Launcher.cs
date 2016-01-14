using System;
using System.Collections;
using UnityEngine;

[RequireComponent( typeof(SpriteRenderer), typeof(BoxCollider2D) ) ]
public class Launcher : MonoBehaviour
{
    [SerializeField]
    private float LaunchForce = 100f;

    [SerializeField]
    private float ResetTime = 1f;

    [SerializeField]
    private Sprite[] SpriteStates = new Sprite[2];

    private SpriteRenderer myRenderer;

    private MovementStateManager manager;

    private Boolean launched = false;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<MovementStateManager>();
        myRenderer = GetComponent<SpriteRenderer>();

        myRenderer.sprite = SpriteStates[ 0 ];
    }

    void OnTriggerEnter2D( Collider2D collision )
    {
        if ( collision.gameObject.tag == "Player" && !launched)
        {
            manager.LaunchPlayer( LaunchForce );
            myRenderer.sprite = SpriteStates[ 1 ];
            StartCoroutine( ResetLauncher() );
            launched = true;
        }
    }

    private IEnumerator ResetLauncher()
    {
        yield return new WaitForSeconds( ResetTime );

        myRenderer.sprite = SpriteStates[ 0 ];
        launched = false;
    }
}