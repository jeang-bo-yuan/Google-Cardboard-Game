using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TeleportSystem.Instance.TriggetTeleport(other.gameObject, this);
        }
    }
}
