using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEditor;


public class playerController : MonoBehaviour
{
    public Tilemap tilemap_white;
    public Tilemap tilemap_black;
    public Tilemap tilemap_gray;
    private Rigidbody2D rb;
    public float speed = 10f;
    public float jumpForce;
    public float deathY;
    public TextMeshProUGUI hpText;
    private int hp = 3;
    private float knockbackForce = 5f;
    private bool isHurt = false;
    private float isHurtTime = 0.5f;
    private bool isWhite = true;
    public GameObject bulletPrefab;


    //public GameObject overlapCheck;
    public Transform groundCheck;
    public LayerMask white_ground;
    public LayerMask black_ground;
    public LayerMask gray_ground;
    public LayerMask block;
    private float bulletSpeed = 10f;

    //Analytics variables
    

    void Start()
    {
        //CheckPointManager.Instance.SetCheckpoint(transform, GameObject.FindGameObjectWithTag("Player").GetComponent<Renderer>());
        CheckColor();
        rb = GetComponent<Rigidbody2D>();
        Color playerColor = GetComponent<SpriteRenderer>().color;
        isWhite = playerColor == Color.white; // Assuming white is the default color for 'white' state
        CheckPointManager.Instance.SetStartpoint(transform.position/*, playerColor*/);

    }

    // Update is called once per frame
    void Update()
    {
        if (!isHurt)
        {
            Move();
        }
        ChangeColor();
        CheckDeath();
        CheckColor();

        if (SceneManager.GetActiveScene().buildIndex >= 4)
        {
            Shoot();
        }

       
    }

    //Realize Move and Jump
    void Move()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        // Check for grounded separately, using a downward raycast or an OverlapCircle at the player's feet position.
        //bool isGrounded = IsGrounded(white_ground | black_ground | gray_ground); // Assuming IsGrounded() is defined elsewhere in your code.

        //LayerMask combinedLayer = new LayerMask();
        //if (isWhite)
        //{
        //    combinedLayer |= black_ground;
        //}
        //else
        //{
        //    combinedLayer |= white_ground;
        //}

        //// Only set ray length based on buffer
        //float rayLength = wallCheckDistance + 0.1f; // Slight buffer to account for movement

        //// Offset for starting the raycasts just inside the player's collider edge
        //float colliderEdgeOffset = GetComponent<CapsuleCollider2D>().size.x / 2 * transform.localScale.x;

        //// Cast rays only if there's horizontal input and the player is not grounded
        //if (!Mathf.Approximately(horizontalMove, 0) && !isGrounded)
        //{
        //    Vector2 direction = horizontalMove > 0 ? Vector2.right : Vector2.left;
        //    Vector2 positionOffset = new Vector2(colliderEdgeOffset * (horizontalMove > 0 ? 1 : -1), 0);
        //    Vector2 rayOriginTop = new Vector2(transform.position.x, transform.position.y + GetComponent<CapsuleCollider2D>().size.y / 2) + positionOffset;
        //    Vector2 rayOriginBottom = new Vector2(transform.position.x, transform.position.y - GetComponent<CapsuleCollider2D>().size.y / 2) + positionOffset;

        //    RaycastHit2D hitTop = Physics2D.Raycast(rayOriginTop, direction, rayLength, combinedLayer);
        //    RaycastHit2D hitBottom = Physics2D.Raycast(rayOriginBottom, direction, rayLength, combinedLayer);

        //    // Check if any of the rays hit a wall
        //    if (hitTop.collider != null || hitBottom.collider != null)
        //    {
        //        // There's a wall, so don't move horizontally
        //        horizontalMove = 0;
        //    }
        //}

        //// Apply horizontal movement only if the player is grounded or there is horizontal input
        //if (isGrounded || !Mathf.Approximately(horizontalMove, 0))
        //{
        //    rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        //}

        // Check for a wall on the right
        //if (horizontalMove > 0 && Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, combinedLayer))
        //{
        //    // There's a wall to the right, so don't move right
        //    horizontalMove = 0;
        //}

        //// Check for a wall on the left
        //if (horizontalMove < 0 && Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, combinedLayer))
        //{
        //    // There's a wall to the left, so don't move left
        //    horizontalMove = 0;
        //}

        //// Apply movement
        //rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        //if (horizontalMove != 0)
        //{
        //    //rb.velocity = new Vector2 (horizontalMove * speed * Time.deltaTime, rb.velocity.y);
        //    rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        //}

        if (horizontalMove!=0)
        {
            rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        } 


        //Jump once before landing
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded(white_ground))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if (IsGrounded(black_ground))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if (IsGrounded(gray_ground))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
            else if (IsGrounded(block))
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }
    }

    //bool IsWallAhead(Vector2 direction)
    //{
    //    // Perform a raycast in the direction of movement
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 1f, gray_ground);
    //    if (hit.collider == null)
    //    {
    //        if(CheckOverlap(white_ground))
    //            hit = Physics2D.Raycast(transform.position, direction, 1f, black_ground);
    //    }

    //    if (hit.collider == null)
    //    {
    //        if (CheckOverlap(black_ground))
    //            hit = Physics2D.Raycast(transform.position, direction, 1f, white_ground);
    //    }

    //    // If the raycast hits a wall, return true
    //    return hit.collider != null;
    //}

    bool IsGrounded(LayerMask groundLayer)
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    void ChangeColor()
    {
        // According to color, ignore the collision between BoxCollider and each tilemap.
        if (Input.GetButtonDown("ChangeColor"))
        {
            if (isWhite)
            {
                GetComponent<SpriteRenderer>().color = Color.black;
                isWhite = false;
                // print("white" + CheckOverlap(white_ground).ToString());
                if (CheckOverlap(white_ground))
                {
                    Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), tilemap_white.GetComponent<TilemapCollider2D>(), true);
                    Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), tilemap_white.GetComponent<TilemapCollider2D>(), true);
                }
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                isWhite = true;
                // print("black" + CheckOverlap(black_ground).ToString());
                if (CheckOverlap(black_ground))
                {
                    Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), tilemap_black.GetComponent<TilemapCollider2D>(), true);
                    Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), tilemap_black.GetComponent<TilemapCollider2D>(), true);
                }
            }
        }
    }




    void CheckColor()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        CapsuleCollider2D capsuleCollider = GetComponent<CapsuleCollider2D>();

        if (!isWhite)
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


    //bool CheckOverlap(LayerMask ground)
    //{


    //    CapsuleCollider2D capsuleCollider = GetComponent<CapsuleCollider2D>();
    //    if (capsuleCollider == null) return false;

    //    CapsuleDirection2D direction = capsuleCollider.direction;

    //    float angle = transform.eulerAngles.z;
    //    //find bug. the position lower than observed postion
    //    Collider2D overlapCollider = Physics2D.OverlapCapsule(capsuleCollider.bounds.center, capsuleCollider.size, direction, angle, ground);
    //    if (overlapCollider == null) return false;
    //    return true;
    //}

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
        if (overlapCollider1 == null && overlapCollider2==null) return false;
        return true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Get the enemy's color
            Color enemyColor = collision.gameObject.GetComponent<SpriteRenderer>().color;


            // Check if the collision is on top of the enemy
            if (collision.contacts[0].normal.y > 0.5)
            {
                if ((isWhite && enemyColor.Equals(Color.black)) || (!isWhite && enemyColor.Equals(Color.white)))

                {
                    // Logic to eliminate the enemy
                    string EnemyName = collision.gameObject.name;
                    if (GameObject.Find(EnemyName) != null)
                    {
                        Analytics.Instance.CollectDataEnemyName(EnemyName);
                        Analytics.Instance.Send("EnemykillingRate");
                        Destroy(collision.gameObject);  // Example of eliminating the enemy
                    }
                    
                    

                    // Optionally, add a bounce effect to the player
                    rb.velocity = new Vector2(rb.velocity.x, 10); // Adjust the Y velocity to give a bounce effect
                }
                else
                {
                    ApplyDamage(collision);
                }
            }
            else
            {
                ApplyDamage(collision);
            }

        }


    }


    void CheckDeath()
    {
        
        if (transform.position.y <= deathY)
        {
            //Debug.Log($"Player Y Position: {transform.position.y}, DeathY: {deathY}");
            // Record and send the deathLocation
            if (Analytics.Instance != null)
            {
                //Debug.Log($"Analytics Instance: {Analytics.Instance}");
                Analytics.Instance.CollectDataDeathLoc(transform.position);
            }
            else
            {
                Debug.Log("Analytics instance is null.");
            }
            Analytics.Instance.Send("LocationOfDeath");
            //GameManager.Instance.RestartLevel();
            CheckPointManager.Instance.RespawnPlayer(this.gameObject);
        }
    }

    private void ApplyDamage(Collision2D collision)
    {
        isHurt = true;
        hp -= 1;
        UpdateHpText();

        Vector2 knockbackDirection = transform.position.x < collision.gameObject.transform.position.x ? Vector2.left : Vector2.right;
        rb.velocity = new Vector2(knockbackDirection.x * knockbackForce, rb.velocity.y);

        if (hp <= 0)
        {
            // Record and send the deathLocation
            Analytics.Instance.CollectDataDeathLoc(transform.position);
            Analytics.Instance.Send("LocationOfDeath");
            //GameManager.Instance.RestartLevel();
            CheckPointManager.Instance.RespawnPlayer(gameObject);
            hp = 3;
            UpdateHpText();
        }
        else
        {
            StartCoroutine(ResetIsHurt());
        }
    }

    void UpdateHpText()
    {
        hpText.text = "HP: " + hp;
    }

    IEnumerator ResetIsHurt()
    {
        yield return new WaitForSeconds(isHurtTime);
        isHurt = false;
    }


    //shot bullet for a distance
    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Shot"))
        {
            if (isWhite)
            {
                bulletPrefab.GetComponent<SpriteRenderer>().color = Color.black;
            }
            else
            {
                bulletPrefab.GetComponent<SpriteRenderer>().color = Color.white;
            }

            Vector3 position = transform.position + transform.right; // Generate bullet position relative to transform's position
            GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);

            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = transform.right * bulletSpeed;

            // Destroy the bullet after 5 seconds
            Destroy(bullet, 5f);
        }
    }


}
