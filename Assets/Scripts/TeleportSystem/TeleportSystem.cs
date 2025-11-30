using System.Collections.Generic;
using UnityEngine;

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
    private bool IsDoorOpen = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Instance != null)
            Debug.LogError("超過一個 TeleportSystem 的實例");

        Instance = this;

        IsDoorOpen = true;
        EntryPoint.GetComponent<BoxCollider>().enabled = true;
        ExitPoint.GetComponent<BoxCollider>().enabled = true;
        DoorPoint.GetComponent<BoxCollider>().enabled = false;
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
        }
        else
        {
            Player.transform.rotation *= Quaternion.Euler(0, 180, 0);
            dest = EntryPoint.transform.position - delta;
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
        }
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
        }
    }
}
