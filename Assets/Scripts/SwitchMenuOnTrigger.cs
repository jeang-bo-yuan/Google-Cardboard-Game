using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchMenuOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            SceneManager.LoadScene("Menu");
    }
}
