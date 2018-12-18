using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

    public static Transform camPos;

    public float shakeDuration;
    public float shakeAmount;
    public float decreaseFactor;

	void Start ()
    {
        camPos = gameObject.GetComponent<Transform>();
	}
	
	void Update ()
    {
		
	}

    public static void Shake(float duration, float intensity, float decrease)
    {
        if (duration > 0)
        {
            camPos.position = Vector2.zero + Random.insideUnitCircle * intensity;
            camPos.position = new Vector3(camPos.position.x, camPos.position.y, -10);
            duration -= Time.deltaTime * decrease;
        }
        else
        {
            duration = 0;
            camPos.position = new Vector3(0, 0, -10);
        }
    }
}
