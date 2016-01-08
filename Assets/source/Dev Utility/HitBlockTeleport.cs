using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HitBlockTeleport : MonoBehaviour
{
    public GameObject teleportPosition;

    void OnCollisionEnter2D( Collision2D collision )
    {
        collision.gameObject.transform.position = teleportPosition.transform.position;
    }
}