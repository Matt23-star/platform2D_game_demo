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
        transform.position = transform.position + new Vector3(speed * Time.deltaTime, 0, 0);

        if((Mathf.Abs(startX-transform.position.x)) >= range)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Color playerColor = collision.gameObject.GetComponent<Renderer>().material.color;
            //Debug.Log("playerColor " + playerColor);
            
            //Debug.Log("blockColor " +  blockColor);

            //Debug.Log(ColorsAreSimilar(playerColor, blockColor));
            if (ColorsAreSimilar(playerColor, blockColor))
            {
                Physics2D.IgnoreCollision(collision.collider, this.gameObject.GetComponent<Collider2D>());
            }
            else
            {
                Vector2 knockbackDirection = transform.position.x < collision.gameObject.transform.position.x ? Vector2.left : Vector2.right;
                collision.rigidbody.velocity = new Vector2(knockbackDirection.x * knockbackForce, collision.rigidbody.velocity.y);
            }
        }
    }



    private bool ColorsAreSimilar(Color a, Color b, float tolerance = 0.01f)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance &&
               Mathf.Abs(a.a - b.a) < tolerance;
    }
}
