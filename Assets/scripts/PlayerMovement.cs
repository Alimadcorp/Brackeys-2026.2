using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D playerRB;
    public float movespeed = 5f;
    public float jumpspeed = 8f;
    public bool isgrounded = true;
    public Sprite[] sprites;
    private SpriteRenderer spriteRenderer;

    private float moveInput;
    private bool jumpRequested;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (sprites != null && sprites.Length > 0)
        {
            if (playerRB.linearVelocityX > 0.01f)
            {
                if (sprites.Length > 1) spriteRenderer.sprite = sprites[1];
                spriteRenderer.flipX = false;
            }
            else if (playerRB.linearVelocityX < -0.01f)
            {
                if (sprites.Length > 1) spriteRenderer.sprite = sprites[1];
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.sprite = sprites[0];
            }
        }

        if (DialogueManager.Instance && DialogueManager.Instance.IsDialogueActive)
        {
            moveInput = 0f;
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && isgrounded)
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        if (DialogueManager.Instance && DialogueManager.Instance.IsDialogueActive)
        {
            playerRB.linearVelocity = Vector2.zero;
            return;
        }

        playerRB.linearVelocity = new Vector2(moveInput * movespeed, playerRB.linearVelocity.y);

        if (jumpRequested)
        {
            playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, jumpspeed);
            isgrounded = false;
            jumpRequested = false;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform.CompareTag("ground"))
        {
            for (int i = 0; i < other.contactCount; i++)
            {
                if (other.GetContact(i).normal.y > 0.5f)
                {
                    isgrounded = true;
                    return;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.transform.CompareTag("ground"))
        {
            isgrounded = false;
        }
    }
}