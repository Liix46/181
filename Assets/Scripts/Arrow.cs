using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject Pivot;
    private float _force;

    void Start()
    {
        _force = 15 * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && this.transform.position.x >= -2.66f)
        {
            this.transform.RotateAround(
                Pivot.transform.position,
                Vector3.up,
                -_force);
           
        }
        else if (Input.GetKey(KeyCode.RightArrow) && this.transform.position.x <= 2.66f)
        {
            this.transform.RotateAround(
                Pivot.transform.position,
                Vector3.up,
                _force);
        }

        //Debug.Log("Position :\t" + this.transform.position);
    }
}
