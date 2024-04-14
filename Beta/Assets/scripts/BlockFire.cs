using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFire : MonoBehaviour
{
    public float speed;
    public float range;
    
    private float startX;
    public float knockbackForce = 2f;

    private Color blockColor;
    // Start is called before the first frame update
    void Start()
    {
        startX = transform.position.x;
        blockColor = this.gameObject.GetComponent<SpriteRenderer>().color;

        if (blockColor == Color.black) // black bullet set to black block layer
        {
            this.gameObject.layer = 3;

        }
        else if (blockColor == Color.white) // same for white
        {
            this.gameObject.layer = 6;

        }
        else if (blockColor == Color.gray) // same for gray
        {

            this.gameObject.layer = 7;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerColor();
        transform.position = transform.position + new Vector3(speed * Time.deltaTime, 0, 0);

        if((Mathf.Abs(startX-transform.position.x)) >= range)
        {
            Destroy(gameObject);
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Color playerColor = collision.gameObject.GetComponent<Renderer>().material.color;
    //        //Debug.Log("playerColor " + playerColor);
            
    //        //Debug.Log("blockColor " +  blockColor);

    //        //Debug.Log(ColorsAreSimilar(playerColor, blockColor));
    //        if (ColorsAreSimilar(playerColor, blockColor))
    //        {
    //            Physics2D.IgnoreCollision(collision.collider, this.gameObject.GetComponent<Collider2D>());
    //        }
    //        else
    //        {
    //            Vector2 knockbackDirection = transform.position.x < collision.gameObject.transform.position.x ? Vector2.left : Vector2.right;
    //            collision.rigidbody.velocity = new Vector2(knockbackDirection.x * knockbackForce, collision.rigidbody.velocity.y);
    //        }
    //    }
    //}



    void CheckPlayerColor()
    {
        BoxCollider2D boxCollider = GameObject.Find("Player").GetComponent<BoxCollider2D>();
        CapsuleCollider2D capsuleCollider = GameObject.Find("Player").GetComponent<CapsuleCollider2D>();
        Color color = GetComponent<SpriteRenderer>().color;

        if (color == GameObject.Find("Player").GetComponent<SpriteRenderer>().color)
        {
            Physics2D.IgnoreCollision(boxCollider, GetComponent<BoxCollider2D>(), true);
            Physics2D.IgnoreCollision(capsuleCollider, GetComponent<BoxCollider2D>(), true);
        }
        else
        {
            Physics2D.IgnoreCollision(boxCollider, GetComponent<BoxCollider2D>(), false);
            Physics2D.IgnoreCollision(capsuleCollider, GetComponent<BoxCollider2D>(), false);
        }
    }

}
