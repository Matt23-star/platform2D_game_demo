using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class EnvironmentController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckColor()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        CapsuleCollider2D capsuleCollider = GetComponent<CapsuleCollider2D>();
        Color color = GetComponent<SpriteRenderer>().color;

        if (color != Color.white )
        {
            Physics2D.IgnoreCollision(boxCollider, tilemap_black.GetComponent<TilemapCollider2D>(), true);
            Physics2D.IgnoreCollision(capsuleCollider, tilemap_black.GetComponent<TilemapCollider2D>(), true);
            if (!CheckOverlap(white_ground))
            {
                Physics2D.IgnoreCollision(boxCollider, tilemap_white.GetComponent<TilemapCollider2D>(), false);
                Physics2D.IgnoreCollision(capsuleCollider, tilemap_white.GetComponent<TilemapCollider2D>(), false);
            }

            if (Physics2D.IsTouching(boxCollider, tilemap_white.GetComponent<TilemapCollider2D>()))
            {
                Physics2D.IgnoreCollision(boxCollider, tilemap_white.GetComponent<TilemapCollider2D>(), false);
                Physics2D.IgnoreCollision(capsuleCollider, tilemap_white.GetComponent<TilemapCollider2D>(), false);
            }
        }
        else
        {
            Physics2D.IgnoreCollision(boxCollider, tilemap_white.GetComponent<TilemapCollider2D>(), true);
            Physics2D.IgnoreCollision(capsuleCollider, tilemap_white.GetComponent<TilemapCollider2D>(), true);

            if (!CheckOverlap(black_ground))
            {
                Physics2D.IgnoreCollision(boxCollider, tilemap_black.GetComponent<TilemapCollider2D>(), false);
                Physics2D.IgnoreCollision(capsuleCollider, tilemap_black.GetComponent<TilemapCollider2D>(), false);
            }
            if (Physics2D.IsTouching(boxCollider, tilemap_black.GetComponent<TilemapCollider2D>()))
            {
                Physics2D.IgnoreCollision(boxCollider, tilemap_black.GetComponent<TilemapCollider2D>(), false);
                Physics2D.IgnoreCollision(capsuleCollider, tilemap_black.GetComponent<TilemapCollider2D>(), false);
            }
        }
    }


    bool CheckOverlap(LayerMask ground)
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        CapsuleCollider2D capsuleCollider = GetComponent<CapsuleCollider2D>();
        if (boxCollider == null || capsuleCollider == null) return false;

        CapsuleDirection2D direction = capsuleCollider.direction;

        float angle = transform.eulerAngles.z;
        // Adjust the position if needed
        Collider2D overlapCollider1 = Physics2D.OverlapBox(boxCollider.bounds.center, boxCollider.size, angle, ground);
        //find bug. the position lower than observed postion
        Collider2D overlapCollider2 = Physics2D.OverlapCapsule(capsuleCollider.bounds.center, capsuleCollider.size, direction, angle, ground);
        if (overlapCollider1 == null && overlapCollider2 == null) return false;
        return true;
    }
}
