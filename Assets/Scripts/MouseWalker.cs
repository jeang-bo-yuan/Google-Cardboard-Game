using UnityEngine;
using UnityEngine.InputSystem;

public class MouseWalker : MonoBehaviour
{
    public float Speed = 5f;
    [Tooltip("Use Camera's Transform to indicate move direction")]
    public Transform CameraTransform;

    InputAction _toWalk;
    CharacterController _controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _toWalk = InputSystem.actions.FindAction("toMove");
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_toWalk.IsPressed())
        {
            Vector3 dir = CameraTransform.forward;
            dir.y = 0;
            dir = Speed * Time.deltaTime * dir.normalized;

            _controller.Move(dir);
        }
    }
}
