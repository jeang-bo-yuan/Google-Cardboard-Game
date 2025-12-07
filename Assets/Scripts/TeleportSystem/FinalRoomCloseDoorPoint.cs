using UnityEngine;

[Tooltip("Close Door For One Time")]
public class FinalRoomCloseDoorPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TeleportSystem.Instance.TriggerCloseDoor();
            gameObject.SetActive(false);
        }
    }
}
