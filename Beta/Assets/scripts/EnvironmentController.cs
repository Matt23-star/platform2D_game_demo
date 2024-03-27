using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class EnvironmentController : MonoBehaviour
{
    public Tilemap tilemap_white;
    public Tilemap tilemap_black;
    public Tilemap tilemap_gray;

    public LayerMask white_ground;
    public LayerMask black_ground;
    public LayerMask gray_ground;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerColor();
        CheckPlatformColor();
    }

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

    void CheckPlatformColor()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();

        if (GetComponent<SpriteRenderer>().color != Color.white)
        {
            Physics2D.IgnoreCollision(boxCollider, tilemap_black.GetComponent<TilemapCollider2D>(), true);
            if (!CheckOverlap(white_ground))
            {
                Physics2D.IgnoreCollision(boxCollider, tilemap_white.GetComponent<TilemapCollider2D>(), false);
            }

            if (Physics2D.IsTouching(boxCollider, tilemap_white.GetComponent<TilemapCollider2D>()))
            {
                Physics2D.IgnoreCollision(boxCollider, tilemap_white.GetComponent<TilemapCollider2D>(), false);
            }
        }
        else
        {
            Physics2D.IgnoreCollision(boxCollider, tilemap_white.GetComponent<TilemapCollider2D>(), true);
            if (!CheckOverlap(black_ground))
            {
                Physics2D.IgnoreCollision(boxCollider, tilemap_black.GetComponent<TilemapCollider2D>(), false);
            }
            if (Physics2D.IsTouching(boxCollider, tilemap_black.GetComponent<TilemapCollider2D>()))
            {
                Physics2D.IgnoreCollision(boxCollider, tilemap_black.GetComponent<TilemapCollider2D>(), false);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            BoxCollider2D boxCollider2 = collision.gameObject.GetComponent<BoxCollider2D>();
            if (GetComponent<SpriteRenderer>().color == collision.gameObject.GetComponent<SpriteRenderer>().color)
            {
                if (Physics2D.IsTouching(boxCollider, boxCollider2))
                {
                    Physics2D.IgnoreCollision(boxCollider, boxCollider2, true);
                }
            }
            else
            {
                if (Physics2D.IsTouching(boxCollider, boxCollider2))
                {
                    Physics2D.IgnoreCollision(boxCollider, boxCollider2, false);
                }
            }
        }
    }

    bool CheckOverlap(LayerMask ground)
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null) return false;


        float angle = transform.eulerAngles.z;
        // Adjust the position if needed
        Collider2D overlapCollider1 = Physics2D.OverlapBox(boxCollider.bounds.center, boxCollider.size, angle, ground);
        if (overlapCollider1 == null) return false;
        return true;
    }
}
