using UnityEngine;

public class box : MonoBehaviour
{
    public GameObject hand;
    private float counter;
    private bool canpick;
    private bool inhand;
    public Transform dropPoint;

    void Start()
    {
        
    }

   
    void Update()
    {
        if(canpick == true)
        {
            pickup();
        }
        if (inhand)
        {
           drop();
        }
    }
    void pickup()
    {
        if( Input.GetKeyDown(KeyCode.E))
        {
            counter = 1;
            transform.position = hand.transform.position;
            transform.SetParent(hand.transform);
            inhand = true;
            
        }
    }
     void drop()
      {
          if (Input.GetKeyDown(KeyCode.E))
          {
            counter++;
            if (counter == 3)
            {
                transform.position = dropPoint.transform.position;
                transform.SetParent(null);
                inhand = false;
            }
          }
      }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null &&  other.CompareTag("Player"))
        {
            canpick = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            canpick = false;
        }
    }
}
