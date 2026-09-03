using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform Player;
    public GameObject BigRoom;

    private bool readytotptobigroom = false;

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
            DialogueUI.instance.prompt.text = "Press E to open";
            readytotptobigroom = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        DialogueUI.instance.prompt.text = "";
        readytotptobigroom = false;
    }
}
