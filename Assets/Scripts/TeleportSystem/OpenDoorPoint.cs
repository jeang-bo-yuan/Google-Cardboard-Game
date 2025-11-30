using UnityEngine;

public class OpenDoorPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TeleportSystem.Instance.TriggerOpenDoor();
        }
    }
}
