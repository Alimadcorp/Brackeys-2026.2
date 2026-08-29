using UnityEngine;

public class ZR_to_BR : MonoBehaviour
{
    public Transform Player;
    public GameObject BigRoom;
    public TMPro.TextMeshPro interaction_statement;

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
            readytotptobigroom = false;
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
        interaction_statement.text = "";
        readytotptobigroom = false;
    }
}
