using UnityEngine;

public class BR_to_SR : MonoBehaviour
{
    public Transform player;
    public TMPro.TextMeshProUGUI interaction_statement;
    public GameObject storageroom;
    private bool readytotptostorageroom;
    void Start()
    {
        
    }


    void Update()
    {

        if (readytotptostorageroom)
        {
            teleporttostorageroom();
        }
    }
    void teleporttostorageroom()
    {
            if (Input.GetKeyDown(KeyCode.E))
            {
                player.transform.position = new Vector3(storageroom.transform.position.x, storageroom.transform.position.y, player.transform.position.z);
                readytotptostorageroom = false;
            }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null & other.name == "Player")
        {
            interaction_statement.text = "Press E to open";
            readytotptostorageroom = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interaction_statement.text = "";
        readytotptostorageroom = false;
    }
}
