using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager Instance { get; private set; }
    private Vector3 currentCheckpoint;
    private Vector3 currentStartpoint;
    private bool isStartpoint = true;
    //private Color currentColor;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists.
        //Debug.Log(Instance == null ? "Instance is null" : "Instance is not null");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint(Vector3 checkpoint/*, Color color*/)
    {
        currentCheckpoint = checkpoint;
        isStartpoint = false;
        //currentColor = color;
    }


    public void SetStartpoint(Vector3 startpoint)
    {
        currentStartpoint = startpoint;
        isStartpoint = true;
    }

    public void RespawnPlayer(GameObject player)
    {
        if (currentCheckpoint != null)
        {
            if (!isStartpoint)
            {
                player.transform.position = currentCheckpoint;
            }
            else
            {
                player.transform.position = currentStartpoint;
            }
            //player.GetComponent<SpriteRenderer>().color = currentColor;
            //player.GetComponent<SpriteRenderer>().color = currentColor;
        }
    }
}
