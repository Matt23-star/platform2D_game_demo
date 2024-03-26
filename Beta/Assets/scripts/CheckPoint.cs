using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CheckPointManager.Instance.SetCheckpoint(transform.position, GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().color);
        }
    }
}

