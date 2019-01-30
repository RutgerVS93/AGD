using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour {

    private PlatformEffector2D effector;
    public float waitTime;

	void Start ()
    {
        effector = GetComponent<PlatformEffector2D>();
	}
	
	void Update ()
    {
        float v = Input.GetAxis("Vertical");

        //if (Input.GetKeyUp(KeyCode.S) || Input.GetButton("A_Button") && v < 0)
        //{
        //    waitTime = .2f;
        //}

        //if (Input.GetKey(KeyCode.S) || Input.GetButton("A_Button") && v < 0)
        //{
        //    if (waitTime <= 0)
        //    {
        //        effector.rotationalOffset = -180;
        //        waitTime = .2f;
        //    }
        //    else
        //    {
        //        waitTime -= Time.deltaTime;
        //    }
        //}

        if (Input.GetButton("A_Button") && v < 0)
        {
            StartCoroutine(Timer());
        }
        //else
        //{
        //    effector.rotationalOffset = 0;
        //}
        

        //if (Input.GetKey(KeyCode.W))
        //{
        //    effector.rotationalOffset = 0;
        //}
	}

    IEnumerator Timer()
    {
        effector.rotationalOffset = -180;

        yield return new WaitForSeconds(.2f);

        effector.rotationalOffset = 0;
    }
}
