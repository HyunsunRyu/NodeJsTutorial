using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private float maxSpeed = 3f;

    private Vector3 speed;
    private Vector3 rotate;
    private string id;

    private void Awake()
    {
        speed = Vector3.zero;
        rotate = (new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f))).normalized;
    }

    public void InitMainCharacter(string id)
    {
        this.id = id;

        Joystick.instance.SetJoystickMovement(Move);
        Joystick.instance.SetJoystickOn(On);
        Joystick.instance.SetJoystickOff(Off);

        FocusCamera cam = Camera.main.gameObject.GetComponent<FocusCamera>();
        cam.target = transform;
    }

    private void Move(Vector2 dir)
    {
        speed = new Vector3(dir.x, 0f, dir.y) * maxSpeed;
        rotate = (new Vector3(dir.x, 0f, dir.y)).normalized;
    }

    private void On(Vector2 dir)
    {
        Move(dir);
    }

    private void Off()
    {
        speed = Vector3.zero;
    }

    private void Update()
    {
        transform.position = transform.position + speed * Time.deltaTime;
        transform.forward = rotate;

        anim.SetFloat("Speed", speed.magnitude);
    }
}
