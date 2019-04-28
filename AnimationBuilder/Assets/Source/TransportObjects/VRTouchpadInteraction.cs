using UnityEngine;

public class VRTouchpadInteraction
{
	public GameObject ControllerObject { get; private set; }
	public Vector2 TouchPosition { get; private set; }

	public VRTouchpadInteraction(GameObject controllerObject, Vector2 touchPos)
	{
		ControllerObject = controllerObject;
		TouchPosition = touchPos;
	}
}
