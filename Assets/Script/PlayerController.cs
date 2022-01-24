using Funzilla;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float speed = 8.0f;
    [SerializeField] private DynamicJoystick Joystick;
    void Start()
    {

    }
    private Vector3 mousePos;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            var v = Input.mousePosition - mousePos;
            if (v.magnitude <= 0) return;
            v = v.normalized;
            v *= speed * Time.smoothDeltaTime;
            transform.position += new Vector3(v.x, 0, v.y);
        }
    }
}
