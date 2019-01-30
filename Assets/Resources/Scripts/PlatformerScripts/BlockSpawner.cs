using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {

    public GameObject block;
    private GameObject blockClone;
    public int[] numberofPositions;
    public int distance;

    public float delay, cycleDelay;
    public bool canSpawn = true;

	void Start ()
    {
		
	}
	
	void Update ()
    {
        if (canSpawn)
        {
            StartCoroutine(SpawnBlock());
        }        
    }

    IEnumerator SpawnBlock()
    {
        canSpawn = false;
        for (int i = 0; i < numberofPositions.Length; i++)
        {
            Vector3 newPos;
            newPos = transform.position;
            newPos.x += i * distance;

            blockClone = Instantiate(block, newPos, Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(cycleDelay);
        canSpawn = true;
    }
}
