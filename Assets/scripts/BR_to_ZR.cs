using UnityEngine;

public class BR_to_ZR : MonoBehaviour
{
    public GameObject zachroom;
    public Transform player;
    public TMPro.TextMeshProUGUI interaction_statement;
    private bool readytotptozachroom = false;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (readytotptozachroom){
            teleporttozachroom();
        }
    }
    private void teleporttozachroom()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            player.transform.position = new Vector3(zachroom.transform.position.x, zachroom.transform.position.y, zachroom.transform.position.z);
            readytotptozachroom = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other != null && other.name == "Player")
        {
            interaction_statement.text = "Press E to open";
            readytotptozachroom = true;
        }
       
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other != null && other.name == "Player")
        {
            interaction_statement.text = "";
            readytotptozachroom = false;
        }
    }
}
