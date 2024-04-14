using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiggerJumpF : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            
            collision.GetComponent<playerController>().jumpForce = 14;

        }
    }
}
