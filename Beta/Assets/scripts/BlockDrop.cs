using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockDrop : MonoBehaviour
{
    public GameObject blockPrefab;
    public Tilemap tilemap_black;
    public Tilemap tilemap_white;
    public Tilemap tilemap_gray;
    public Color color;
    public float spawnInterval = 2f;
    public float blockLifetime = 5f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SpawnBlock();
            timer = spawnInterval;
        }
    }

    void SpawnBlock()
    {
        GameObject block = Instantiate(blockPrefab, transform.position, Quaternion.identity);
        block.GetComponent<Transform>().localScale = new Vector3(8f, 0.8f, 1f);
        block.GetComponent<SpriteRenderer>().color = color;
        block.GetComponent<EnvironmentController>().tilemap_black = tilemap_black;
        block.GetComponent<EnvironmentController>().tilemap_white = tilemap_white;
        block.GetComponent<EnvironmentController>().tilemap_gray = tilemap_gray;
        Destroy(block, blockLifetime);
    }
}
