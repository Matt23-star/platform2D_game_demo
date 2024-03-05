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


public class playerController : MonoBehaviour
{
    public Tilemap tilemap_white;
    public Tilemap tilemap_black;
    public Tilemap tilemap_gray;
    private Rigidbody2D rb;
    public float speed = 10f;
    public float jumpForce;
    public float deathY = -10f;
    public TextMeshProUGUI hpText;
    private int hp = 3;
    private float knockbackForce = 5f;
    private bool isHurt = false;
    private float isHurtTime = 0.5f;
    private bool isWhite = true;


    //public GameObject overlapCheck;
    public Transform groundCheck;
    public LayerMask white_ground;
    public LayerMask black_ground;
    public LayerMask gray_ground;


    void Start()
    {
        CapsuleCollider2D children = GetComponentInChildren<CapsuleCollider2D>();
        //print(children.ToString());
        CheckColor();
        rb = GetComponent<Rigidbody2D>();
        Color playerColor = GetComponent<SpriteRenderer>().color;
        isWhite = playerColor == Color.white; // Assuming white is the default color for 'white' state
        
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
    }

    //Realize Move and Jump
    void Move()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        if (horizontalMove != 0)
        {
            //rb.velocity = new Vector2 (horizontalMove * speed * Time.deltaTime, rb.velocity.y);
            rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
        }

        //isLanded = Physics.CheckSphere(player.position, groundCheckRadius, ground);

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
        }
    }

    bool IsGrounded(LayerMask groundLayer)
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
    }

    void ChangeColor()
    {
        //according to color, ignore the collison between Capsule and each tilemap.
        if (Input.GetButtonDown("ChangeColor"))
        {

            if (isWhite)
            {
                GetComponent<Renderer>().material.color = Color.black;
                isWhite = false;
                //print("white" + CheckOverlap(white_ground).ToString());
                if (CheckOverlap(white_ground))
                {
                    Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), tilemap_white.GetComponent<TilemapCollider2D>(), true);
                }
            }
            else
            {
                GetComponent<Renderer>().material.color = Color.white;
                isWhite = true;
                //print("black" + CheckOverlap(black_ground).ToString());
                if (CheckOverlap(black_ground))
                {
                    Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), tilemap_black.GetComponent<TilemapCollider2D>(), true);
                }
            }
        }
    }

    void CheckDeath()
    {
        if (transform.position.y <= deathY)
        {
            //keep track of number of death for analytics
            GameManager.Instance.numberOfDeath++;
            GameManager.Instance.RestartLevel();
        }
    }
    

    void CheckColor()
    {
        if (!isWhite)
        {
            Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), tilemap_black.GetComponent<TilemapCollider2D>(), true);

            if (!CheckOverlap(white_ground))
                Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), tilemap_white.GetComponent<TilemapCollider2D>(), false);
            //else print("there is an overlapping on white");
            if (Physics2D.IsTouching(GetComponent<CapsuleCollider2D>(), tilemap_white.GetComponent<TilemapCollider2D>()))
                Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), tilemap_white.GetComponent<TilemapCollider2D>(), false);

        }
        else
        {
            Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), tilemap_white.GetComponent<TilemapCollider2D>(), true);

            if (!CheckOverlap(black_ground))
                Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), tilemap_black.GetComponent<TilemapCollider2D>(), false);
            //else print("there is an overlapping on black");
            if (Physics2D.IsTouching(GetComponent<CapsuleCollider2D>(), tilemap_black.GetComponent<TilemapCollider2D>()))
                Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), tilemap_black.GetComponent<TilemapCollider2D>(), false);

        }
    }

    bool CheckOverlap(LayerMask ground)
    {


        CapsuleCollider2D capsuleCollider = GetComponent<CapsuleCollider2D>();
        if (capsuleCollider == null) return false;

        CapsuleDirection2D direction = capsuleCollider.direction;

        float angle = transform.eulerAngles.z;
        //find bug. the position lower than observed postion
        Collider2D overlapCollider = Physics2D.OverlapCapsule(capsuleCollider.bounds.center, capsuleCollider.size, direction, angle, ground);
        if (overlapCollider == null) return false;
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
                    GameManager.Instance.numberOfKills++;
                    Destroy(collision.gameObject);  // Example of eliminating the enemy

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

    private void ApplyDamage(Collision2D collision)
    {
        isHurt = true;
        hp -= 1;
        GameManager.Instance.HPLost--;
        UpdateHpText();

        Vector2 knockbackDirection = transform.position.x < collision.gameObject.transform.position.x ? Vector2.left : Vector2.right;
        rb.velocity = new Vector2(knockbackDirection.x * knockbackForce, rb.velocity.y);

        if (hp <= 0)
        {
            //keep track of number of death for analytics
            GameManager.Instance.numberOfDeath++;
            GameManager.Instance.RestartLevel();
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




}
