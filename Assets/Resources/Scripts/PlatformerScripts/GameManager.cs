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

    public GameObject[] levels;

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
        ActivateLevel();
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

    public void ActivateLevel()
    {
        switch (currentLevel)
        {
            case 0:
                levels[0].SetActive(true);
                levels[1].SetActive(false);
                levels[2].SetActive(false);
                break;
            case 1:
                levels[0].SetActive(false);
                levels[1].SetActive(true);
                levels[2].SetActive(false);
                break;
            case 2:
                levels[0].SetActive(false);
                levels[1].SetActive(false);
                levels[2].SetActive(true);
                break;
        }
    }
}
