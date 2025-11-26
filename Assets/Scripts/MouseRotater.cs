using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Tracked Pose Driver 會偵測 XR 裝置的旋轉並修改相機的 Local Rotation。
/// 而這個 class 的功能是讀入滑鼠的移動來加上額外的旋轉，讓我們可同時用滑鼠和 XR 裝置旋轉視角。
/// </summary>
[RequireComponent(typeof(Camera))]
public class MouseRotater : MonoBehaviour
{
    // 設成 static，讓切換場景後保留視角
    private static float _yaw = 0f; // left / right
    private static float _pitch = 0f; // up / down
    // smooth damp
    private static Vector3 _targetEuler = Vector3.zero;
    private static float _currSmoothVelocity_pitch = 0f;
    private static float _currSmoothVelocity_yaw = 0f;

    private InputAction _look;
    public float Sensitivity = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _look = InputSystem.actions.FindAction("Look");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 delta = _look.ReadValue<Vector2>();

        // 更新 yaw, pitch
        _yaw += delta.x * Sensitivity; // left / right
        _pitch -= delta.y * Sensitivity; // up / down

        // 目前 Camera 的 yaw, pitch
        var camEuler = transform.localEulerAngles;
        if (camEuler.x > 90)
            camEuler.x = camEuler.x - 360f;
        // 在相機的 yaw, pitch 上加上滑鼠輸入的 yaw, pitch
        float targetYaw = camEuler.y + _yaw;
        float targetPitch = Mathf.Clamp(camEuler.x + _pitch, -90f, 90f);

        _pitch = targetPitch - camEuler.x;

        _targetEuler.x = Mathf.SmoothDampAngle(_targetEuler.x, targetPitch, ref _currSmoothVelocity_pitch, 0.1f);
        _targetEuler.y = Mathf.SmoothDampAngle(_targetEuler.y, targetYaw, ref _currSmoothVelocity_yaw, 0.1f);
        _targetEuler.z = camEuler.z;

        transform.parent.localRotation = Quaternion.Euler(_targetEuler) * Quaternion.Inverse(transform.localRotation);
    }
}
