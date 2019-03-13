using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMovementControls : MonoBehaviour
{
    private readonly float SPEED = 3f;
	void Update ()
    {
		if (Input.GetKey(KeyCode.W))
        {
            Vector3 pos = transform.localPosition;
            pos.y += SPEED * Time.deltaTime;
            transform.localPosition = pos;
        }

        if (Input.GetKey(KeyCode.A))
        {
            Vector3 pos = transform.localPosition;
            pos.x -= SPEED * Time.deltaTime;
            transform.localPosition = pos;
        }

        if (Input.GetKey(KeyCode.S))
        {
            Vector3 pos = transform.localPosition;
            pos.y -= SPEED * Time.deltaTime;
            transform.localPosition = pos;
        }

        if (Input.GetKey(KeyCode.D))
        {
            Vector3 pos = transform.localPosition;
            pos.x += SPEED * Time.deltaTime;
            transform.localPosition = pos;
        }
    }
}
