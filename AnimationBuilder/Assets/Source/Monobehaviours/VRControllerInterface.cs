using System;
using System.Collections.Generic;
using UnityEngine;

public class VRControllerInterface : MonoBehaviour 
{
	private SteamVR_TrackedObject controller;
	private bool isTouchDown;

    private List<Action<bool>> triggerListeners;
    private List<Action<bool>> gripListeners;

    public VRControllerInterface()
    {
        triggerListeners = new List<Action<bool>>();
        gripListeners = new List<Action<bool>>();
    }

	public void Start()
	{
		controller = gameObject.GetComponent<SteamVR_TrackedObject> ();
	}

	public void Update()
	{
		SteamVR_Controller.Device ctrl = SteamVR_Controller.Input ((int)controller.index);

		// Handle trigger input
		if (ctrl.GetPressDown (SteamVR_Controller.ButtonMask.Trigger))
		{
			VRTriggerInteraction interaction = new VRTriggerInteraction(ctrl, gameObject);
			Service.EventManager.SendEvent (EventId.VRControllerTriggerPress, interaction);
            CallListeners(triggerListeners, true);
		}

		if (ctrl.GetPressUp (SteamVR_Controller.ButtonMask.Trigger))
		{
			VRTriggerInteraction interaction = new VRTriggerInteraction(ctrl, gameObject);
			Service.EventManager.SendEvent (EventId.VRControllerTriggerRelease, interaction);
            CallListeners(triggerListeners, false);
        }

        // Handle trigger input
        if (ctrl.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            VRTriggerInteraction interaction = new VRTriggerInteraction(ctrl, gameObject);
            Service.EventManager.SendEvent(EventId.VRControllerGripPress, interaction);
            CallListeners(gripListeners, true);
        }

        if (ctrl.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            VRTriggerInteraction interaction = new VRTriggerInteraction(ctrl, gameObject);
            Service.EventManager.SendEvent(EventId.VRControllerGripRelease, interaction);
            CallListeners(gripListeners, false);
        }

        // Handle touchpad input
        VRTouchpadInteraction touch;
		if (ctrl.GetTouch (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad))
		{
			if (!isTouchDown)
			{
				touch = new VRTouchpadInteraction (gameObject, ctrl.GetAxis ());
				Service.EventManager.SendEvent(EventId.VRControllerTouchpadPress, touch);
				isTouchDown = true;
			}
			else
			{
				touch = new VRTouchpadInteraction (gameObject, ctrl.GetAxis ());
				Service.EventManager.SendEvent(EventId.VRControllerTouchpadDrag, touch);
			}
		}
		else if(isTouchDown)
		{
			touch = new VRTouchpadInteraction (gameObject, ctrl.GetAxis ());
			Service.EventManager.SendEvent(EventId.VRControllerTouchpadRelease, touch);
			isTouchDown = false;
		}
	}

    private void CallListeners(List<Action<bool>> listeners, bool isPressed)
    {
        for (int i = 0, count = listeners.Count; i < count; ++i)
        {
            listeners[i].Invoke(isPressed);
        }
    }

    public void RegisterTriggerPressListener(Action<bool> onPress)
    {
        triggerListeners.Add(onPress);
    }

    public void UnregisterTriggerPress(Action<bool> onPress)
    {
        triggerListeners.Remove(onPress);
    }

    public void RegisterGripPressListener(Action<bool> onPress)
    {
        gripListeners.Add(onPress);
    }

    public void UnregisterGripPress(Action<bool> onPress)
    {
        gripListeners.Remove(onPress);
    }
}
