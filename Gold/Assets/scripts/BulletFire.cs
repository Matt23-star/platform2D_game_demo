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
        if (collision.gameObject.CompareTag("platform"))
        {
            Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
        if (collision.gameObject.CompareTag("Block"))
        {
            if (collision.gameObject.GetComponent<SpriteRenderer>().color == Color.black)
            {
                if (this.gameObject.GetComponent<SpriteRenderer>().color == Color.white)
                {
                    collision.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                }
                Destroy(this.gameObject);
            }
            else if (collision.gameObject.GetComponent<SpriteRenderer>().color == Color.white)
            {
                if (this.gameObject.GetComponent<SpriteRenderer>().color == Color.black)
                {
                    collision.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                }
                Destroy(this.gameObject);
            }
            //else if (collision.gameObject.GetComponent<SpriteRenderer>().color == Color.gray)
            //{
            //    Check if the collision is with a slope
            //    if (collision.gameObject.GetComponent<Slope>() != null)
            //    {
            //        Calculate the reflection direction
            //        Vector2 incomingDirection = this.gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
            //        Vector2 normal = collision.contacts[0].normal;
            //        Vector2 slopeDirection = collision.gameObject.GetComponent<Slope>().slopeDirection;
            //        Vector2 reflectionDirection = Vector2.Reflect(incomingDirection, slopeDirection);

            //        Set the new direction for the bullet

            //       this.gameObject.GetComponent<Rigidbody2D>().velocity = reflectionDirection * 10f;
            //    }
            //    else
            //    {
            //        Calculate the reflection direction
            //        Vector2 incomingDirection = this.gameObject.GetComponent<Rigidbody2D>().velocity.normalized;
            //        Vector2 normal = collision.contacts[0].normal;
            //        Vector2 reflectionDirection = Vector2.Reflect(incomingDirection, normal);

            //        Set the new direction for the bullet

            //       this.gameObject.GetComponent<Rigidbody2D>().velocity = reflectionDirection * 10f;
            //    }
            //}
        }
    }
}
