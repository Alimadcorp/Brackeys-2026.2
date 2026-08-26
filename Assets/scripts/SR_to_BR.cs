using UnityEngine;

public class SR_to_BR : MonoBehaviour
{
    public GameObject BigRoom;
    public Transform player;
    public TMPro.TextMeshProUGUI interaction_statement;
    private bool readytotptobigroom;

    

    
    void Update()
    {
        if (readytotptobigroom)
        {
            teleporttobigroom();
        }
    }
    void teleporttobigroom()
    {
        if (Input.GetKeyUp(KeyCode.E))
        {
            player.transform.position = new Vector3(BigRoom.transform.position.x, BigRoom.transform.position.y, player.transform.position.z);
            readytotptobigroom = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other != null && other.name == "Player")
        {
            interaction_statement.text = "Press E to open";
            readytotptobigroom = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        interaction_statement.text = "";
        readytotptobigroom = false;
    }
}
