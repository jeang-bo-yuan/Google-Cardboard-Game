using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 開門 -> 進入傳送點 -> 關門 -> 進入開門點
///  ^ ------------------------------
/// </summary>
public class TeleportSystem : MonoBehaviour
{
    public static TeleportSystem Instance { get; private set; }

    public TeleportPoint EntryPoint;
    public TeleportPoint ExitPoint;
    public OpenDoorPoint DoorPoint;
    public List<Animator> DoorsToControl;
    private bool IsDoorOpen = false;

    private AudioClip OpenDoorClip;
    private AudioClip LockDoorClip;

    void OnEnable()
    {
        if (Instance != null)
            Debug.LogError("超過一個 TeleportSystem 的實例");

        Instance = this;
        OpenDoorClip = Resources.Load("Sound/open door") as AudioClip;
        LockDoorClip = Resources.Load("Sound/lock door") as AudioClip;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        // 等場景載入完成時才載入開門動畫
        TriggerOpenDoor();
    }

    void OnDisable()
    {
        Instance = null;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void TriggetTeleport(GameObject Player, TeleportPoint Point)
    {
        Debug.Assert(IsDoorOpen);

        // 關閉 controller 才能直接調整位置、旋轉
        CharacterController controller = Player.GetComponent<CharacterController>();
        controller.enabled = false;

        Vector3 delta = Player.transform.position - Point.transform.position;
        Vector3 dest;

        if (Point == ExitPoint)
        {
            dest = EntryPoint.transform.position + delta;
            //execute level
            GameManager.Instance.computeLevel(TpPointType.EXIT);
        }
        else
        {
            Player.transform.rotation *= Quaternion.Euler(0, 180, 0);
            dest = EntryPoint.transform.position - delta;
            //execute level
            GameManager.Instance.computeLevel(TpPointType.ENTRY);
        }

        dest.y = Player.transform.position.y;
        Player.transform.position = dest;

        controller.enabled = true;

        // 切換成關門的狀態
        IsDoorOpen = false;
        EntryPoint.GetComponent<BoxCollider>().enabled = false;
        ExitPoint.GetComponent<BoxCollider>().enabled = false;
        DoorPoint.GetComponent<BoxCollider>().enabled = true;
        foreach (Animator anim in DoorsToControl)
        {
            anim.SetTrigger("Toggle");
            var source = anim.GetComponent<AudioSource>();
            source.clip = LockDoorClip;
            source.Play();
        }

        //change room here
        GameManager.Instance.changeRoom();
    }

    public void TriggerOpenDoor()
    {
        Debug.Assert(!IsDoorOpen);

        IsDoorOpen = true;
        EntryPoint.GetComponent<BoxCollider>().enabled = true;
        ExitPoint.GetComponent<BoxCollider>().enabled = true;
        DoorPoint.GetComponent<BoxCollider>().enabled = false;

        foreach (Animator anim in DoorsToControl)
        {
            anim.SetTrigger("Toggle");
            var source = anim.GetComponent<AudioSource>();
            source.clip = OpenDoorClip;
            source.Play();
        }
    }
}
