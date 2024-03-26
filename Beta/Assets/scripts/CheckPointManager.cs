using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager Instance { get; private set; }
    private Vector3 currentCheckpoint;
    private Renderer currentRenderer;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists.
        Debug.Log(Instance == null ? "Instance is null" : "Instance is not null");

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

    public void SetCheckpoint(Vector3 checkpoint, Renderer renderer)
    {
        currentCheckpoint = checkpoint;
        currentRenderer = renderer;
    }

    public void RespawnPlayer(GameObject player)
    {
        if (currentCheckpoint != null)
        {
            player.transform.position = currentCheckpoint;
            player.GetComponent<SpriteRenderer>().material.color = currentRenderer.material.color;
        }
    }
}
