using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    public Transform rightpoint, leftpoint;
    private float leftx, rightx;
    public float speed = 2f;
    private bool movingRight = true;
    //private bool isColliding = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        if (movingRight)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }

        if (transform.position.x >= rightx)
        {
            movingRight = false;
        }
        else if (transform.position.x <= leftx)
        {
            movingRight = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Collider2D otherCollider = collision.collider;
            if (otherCollider is BoxCollider2D)
            {
                if (collision.contacts[0].normal.y < 0)
                {
                    //print(collision.contacts[0].normal.y);
                    if (collision.gameObject.GetComponent<SpriteRenderer>().color != this.gameObject.GetComponent<SpriteRenderer>().color)
                    {
                        if (this.gameObject != null)
                        {
                            Analytics.Instance.CollectDataEnemyName(this.gameObject.name);
                            Analytics.Instance.Send("EnemykillingRate");
                            Destroy(this.gameObject);
                        }
                    }

                }
            }

        }
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    isColliding = false;
    //}
}
