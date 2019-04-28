using UnityEngine;
using System.Collections;

public class VRTriggerInteraction
{
	public SteamVR_Controller.Device Controller { get; private set; }
	public GameObject ControllerObject { get; private set; }

	public VRTriggerInteraction(SteamVR_Controller.Device device, GameObject controllerObject)
	{
		Controller = device;
		ControllerObject = controllerObject;
	}
}