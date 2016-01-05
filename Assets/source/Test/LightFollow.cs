using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class LightFollow : MonoBehaviour
{
    public GameObject followObject;

    public float speed;

    public Vector3 Offset;

    void Update()
    {
        Vector3 direction = followObject.transform.position - transform.position;
        float degree = SMath.VectorToDegree( direction );
        transform.eulerAngles = new Vector3( 0, 0, degree );//get component losradial light and ste face angle to new angle.

        transform.position = Vector3.Lerp( transform.position, followObject.transform.position + Offset, Time.deltaTime * speed );
    }
}