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
    public GameObject reachText;
    private bool isDie = false;


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
        if(GameObject.Find("CheckPointManager"))CheckPointManager.Instance.SetStartpoint(transform.position/*, playerColor*/);
        reachText.SetActive(true);
        if (SceneManager.GetActiveScene().buildIndex < 4)
        {
            StartCoroutine(ShowAndFadeMessage("Reach to the Green Endpoint and Use the Yellow Checkpoint", 3f));
        }
        else { StartCoroutine(ShowAndFadeMessage("Reach to the Green Endpoint and Use the Yellow Checkpoint\r\nTry to use <J> to color the blocks", 3f));}
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
            if (collision.contacts[0].normal.y > 0.5 && collision.contacts[0].normal.y <1)
            {
                print("From player : "+ collision.contacts[0].normal.y);
                if ((isWhite && enemyColor.Equals(Color.black)) || (!isWhite && enemyColor.Equals(Color.white)))
                {
                    // Logic to eliminate the enemy
                    //string EnemyName = collision.gameObject.name;
                    //if (GameObject.Find(EnemyName) != null)
                    //{
                    //    Analytics.Instance.CollectDataEnemyName(EnemyName);
                    //    Analytics.Instance.Send("EnemykillingRate");
                    //    Destroy(collision.gameObject);  // Example of eliminating the enemy
                    //}

                    // Optionally, add a bounce effect to the player
                    rb.velocity = new Vector2(rb.velocity.x, 10); // Adjust the Y velocity to give a bounce effect
                }
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x, 10);
                    //print("Player is hurt on the top");
                    ApplyDamage(collision);
                }
            }
            else if(collision.contacts[0].normal.y == 1)
            {
                
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
            if (!isDie)
            {
                isDie = true;
                Analytics.Instance.CollectDataDeathLoc(transform.position);
                Analytics.Instance.Send("LocationOfDeath");
            }
            //GameManager.Instance.RestartLevel();
            hp = 3;
            UpdateHpText();
            StartCoroutine(ResetIsHurt());
            RespawnPlayer();
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
            if (!isDie)
            {
                isDie = true;
                Analytics.Instance.CollectDataDeathLoc(transform.position);
                Analytics.Instance.Send("LocationOfDeath");
            }
            //GameManager.Instance.RestartLevel();
            hp = 3;
            UpdateHpText();
            StartCoroutine(ResetIsHurt());
            RespawnPlayer();
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

    // Delay the respawn time to avoid bug
    void  RespawnPlayer()
    {
        isDie = false;
        CheckPointManager.Instance.RespawnPlayer(gameObject);
        
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

    IEnumerator ShowAndFadeMessage(string text, float duration)
    {
        TextMeshProUGUI reachedText = reachText.GetComponent<TextMeshProUGUI>();
        reachedText.text = text; // Set the text
        reachedText.alpha = 1; // Make sure the text is fully visible
        reachedText.fontSize = 15; // Set the font size
        // Wait for a brief moment before starting the fade
        yield return new WaitForSeconds(0.5f);
         // Duration over which to fade out
        float startAlpha = reachedText.alpha;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            reachedText.alpha = Mathf.Lerp(startAlpha, 0, normalizedTime); // Linearly interpolate alpha value over time
            yield return null;
        }

        reachedText.alpha = 0;// Ensure the text is fully transparent at the end
        reachText.SetActive(false);
    }

}
