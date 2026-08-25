using UnityEngine;

public class Doors : MonoBehaviour
{
    public Transform Player;
    public GameObject BigRoom;
    public TMPro.TextMeshProUGUI interaction_statement;

    private bool readytotptobigroom = false;
    void Start()
    {
        
    }

    void Update()
    {
        if (readytotptobigroom)
        {
            teleporttobigroom();
        }
    }
    void teleporttobigroom()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Player.transform.position = new Vector3(BigRoom.transform.position.x, BigRoom.transform.position.y, Player.transform.position.z);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.name == "Player")
        {
            interaction_statement.text = "Press E to open";
            readytotptobigroom = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        interaction_statement.text = "";    }
}
