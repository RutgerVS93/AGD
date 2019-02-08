using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    static GameManager gameManager;

    private PlayerController playerController;

    [SerializeField]
    private GameObject player, activePlayer;

    [SerializeField]
    public Transform[] startPositions;

    [SerializeField]
    public int currentLevel;

    public static Vector3 activeCheckpoint ;

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
        }

        SpawnPlayerAtLevelStart();
    }

    public void SpawnPlayerAtCheckPoint()
    {
        activePlayer = Instantiate(player);
        activePlayer.transform.position = activeCheckpoint;
        playerController = activePlayer.GetComponent<PlayerController>();
    }

    public void SpawnPlayerAtLevelStart()
    {
        activePlayer = Instantiate(player);
        activePlayer.transform.position = startPositions[currentLevel].transform.position;
    }
}
