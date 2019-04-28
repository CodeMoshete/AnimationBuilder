using UnityEngine;
using System.Collections;

public class VRControllerInteraction
{
	public SteamVR_Controller.Device Controller { get; private set; }
	public GameObject CollisionObject { get; private set; }

	public VRControllerInteraction(SteamVR_Controller.Device device, GameObject collisionObject)
	{
		Controller = device;
		CollisionObject = collisionObject;
	}
}