using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalsScript : MonoBehaviour {

    public GameObject enterPortal;
    public GameObject exitPortal;

    GameObject enterClone, exitClone;

    [Space]
    public GameObject spawnPointEnter;
    public GameObject spawnPointExit;
    public float spawnPosIncrease;

    [Space]
    public GameObject cam;
    public Vector3 camTargetPos;
    public float camPosIncrease;
    public float camSpeed;
    public bool moveCam = false;

	void Start ()
    {
        enterClone = Instantiate(enterPortal, spawnPointEnter.transform.position, Quaternion.identity);
        exitClone = Instantiate(exitPortal, spawnPointExit.transform.position, Quaternion.identity);
    }
	
	void Update ()
    {
        if (moveCam)
        {
            UpdateCamPosition(cam, camTargetPos);
        }
        if (cam.transform.position.y <= camTargetPos.y + .0001f)
        {
            moveCam = false;            
        }

        enterClone.transform.position = spawnPointEnter.transform.position;
        exitClone.transform.position = spawnPointExit.transform.position;
	}

    void UpdatePortalPosition(GameObject pos, float posChange)
    {
        pos.transform.position = new Vector3(pos.transform.position.x, pos.transform.position.y + posChange, pos.transform.position.z);
    }

    void UpdateCamPosition(GameObject cam, Vector3 target)
    {       
        cam.transform.position = Vector2.Lerp(cam.transform.position, target, camSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log("Collision detected");

            camTargetPos = new Vector3(camTargetPos.x, camTargetPos.y + camPosIncrease, camTargetPos.z);
            moveCam = true;
            UpdatePortalPosition(spawnPointEnter, spawnPosIncrease);
            UpdatePortalPosition(spawnPointExit, spawnPosIncrease);
            collision.gameObject.transform.position = spawnPointEnter.transform.position;
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y -11, transform.position.z);
        }
    }
}
