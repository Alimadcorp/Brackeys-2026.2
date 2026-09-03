using Unity.Collections;
using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    public float speed = 2f;
    public float walkDistance = 3f;

    private Vector3 startPosition;
    private bool movingRight = true;

    void Start()
    {
        startPosition = transform.position;    
    }

    void Update()
    {
        float leftLimit = startPosition.x - walkDistance;
        float rightLimit = startPosition.x + walkDistance;

        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            if (transform.position.x >= rightLimit)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            if (transform.position.x <= leftLimit)
            {
                movingRight = true;
            }
        }
    }
}