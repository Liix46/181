using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject Pivot;
    private float _force;

    private AudioSource squeak;

 

    void Start()
    {
        _force = 10;
        squeak = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && this.transform.position.x >= -2.66f)
        {
            this.transform.RotateAround(
                Pivot.transform.position,
                Vector3.up,
                -_force * Time.deltaTime);
            // Play sound ( if not playing )
            if (!squeak.isPlaying)
            {
                squeak.Play();
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow) && this.transform.position.x <= 2.66f)
        {
            this.transform.RotateAround(
                Pivot.transform.position,
                Vector3.up,
                _force * Time.deltaTime);

            if (!squeak.isPlaying)
            {
                squeak.Play();
            }
        }

        // if sound is playing, but key released
        if (Input.GetKeyUp(KeyCode.RightArrow)
            || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (squeak.isPlaying)
            {
                squeak.Stop();
            }
        }

    }

}
