using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFire : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            if (collision.gameObject.GetComponent<SpriteRenderer>().color == Color.black)
            {
                if (this.gameObject.GetComponent<SpriteRenderer>().color == Color.white)
                {   
                    collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
            else if (collision.gameObject.GetComponent<SpriteRenderer>().color == Color.white)
            {
                if (this.gameObject.GetComponent<SpriteRenderer>().color == Color.black)
                {
                    collision.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                }
            }
        }
        Destroy(this.gameObject);
    }
}
