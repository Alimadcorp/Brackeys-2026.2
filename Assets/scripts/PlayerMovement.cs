using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
 
    Rigidbody2D playerRB;
    public float movespeed = 5f;
    public float jumpspeed = 8f;
    public bool isgrounded = true;
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
               
        if (Input.GetKey(KeyCode.A))
            playerRB.linearVelocity = new Vector2(-movespeed, playerRB.linearVelocity.y);
        if (Input.GetKey(KeyCode.D))
            playerRB.linearVelocity = new Vector2(movespeed, playerRB.linearVelocity.y);
        if (playerRB.linearVelocity.y != 0)
        {
            isgrounded = false;
        }
        else { isgrounded = true; }
        if (Input.GetKeyDown(KeyCode.Space) && isgrounded)
        {
            playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, jumpspeed);
            

        }

    }
    
}
