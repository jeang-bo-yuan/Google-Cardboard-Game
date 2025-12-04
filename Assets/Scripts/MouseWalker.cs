using UnityEngine;
using UnityEngine.InputSystem;

public class MouseWalker : MonoBehaviour
{
    public float Speed = 5f;
    [Tooltip("Use Camera's Transform to indicate move direction")]
    public Transform CameraTransform;
    private AudioClip footStep;
    public float stepRate = 0.5f;
	public float stepCoolDown;

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
            stepCoolDown -= Time.deltaTime;
		if (stepCoolDown < 0f){
            footStep = Resources.Load("Sound/Footsteps_Tile_Walk_05") as AudioClip;
            var source = GetComponent<AudioSource>();
			source.pitch = 1f + Random.Range (-0.2f, 0.2f);
			source.PlayOneShot (footStep, 0.9f);
			stepCoolDown = stepRate;
		}
            
            _controller.Move(dir);
        }
    }
}
