using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BeamScript : MonoBehaviour {

    public GameObject beamStart;
    public GameObject beamEnd;
    public GameObject beam;

    private GameObject beamStartClone;
    private GameObject beamEndClone;
    private GameObject beamClone;

    public LineRenderer line;

    public GameObject hand;

    [Header("Adjustable Variables")]
    public float beamEndOffset = 0f; //How far from the raycast hit point the end effect is positioned
    public float textureScrollSpeed = 8f; //How fast the texture scrolls along the beam
    public float textureLengthScale = 3; //Length of the beam texture

    [Space]
    public Transform firePoint;
    public bool spawned = false;

    public float distance;

    private void Start()
    {
        line = beam.GetComponent<LineRenderer>();
        line.SetVertexCount(2);
    }

    void Update()
    {
        line.SetPosition(0, firePoint.position);
        Vector2 hitPoint = Vector2.zero;

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, -transform.up);
        if (hit.collider)
        {
            //hitPoint = hit.point - (Vector2)firePoint.position;
            line.SetPosition(1, hitPoint);
            Debug.Log("Hit");
        }
        else
        {
            hitPoint = (Vector2)transform.position * 100;
            line.SetPosition(1, hitPoint);
            Debug.Log("Miss");
        }

        if (!spawned)
        {
            beamStartClone = Instantiate(beamStart, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            beamEndClone = Instantiate(beamEnd, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            beamClone = Instantiate(beam, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;

            spawned = true;
        }

        line.SetPosition(1, hitPoint);

        beamStartClone.transform.LookAt(beamEndClone.transform.position);
        beamStartClone.transform.position = firePoint.transform.position;

        beamEndClone.transform.LookAt(beamStartClone.transform.position);
        beamEndClone.transform.position = hitPoint;        

        beamClone.transform.rotation = hand.transform.rotation;
    }

    void OnDisable()
    {
        spawned = false;
        Destroy(beamStartClone);
        Destroy(beamEndClone);
        Destroy(beamClone);
    }
}
