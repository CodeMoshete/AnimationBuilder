using UnityEngine;

public class ManipulatorComponent : MonoBehaviour
{
    [HideInInspector]
    public bool IsGrabbing;

    public Transform CurrentTarget { get; private set; }
    public TrackedObject CurrentTrackedObject { get; private set; }

    void Start ()
    {
        SphereCollider collider = gameObject.GetComponent<SphereCollider>();
		if (collider == null)
        {
            collider = gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = true;
        }

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
        }

        VRControllerInterface vrInterface = gameObject.GetComponent<VRControllerInterface>();
        if (vrInterface == null)
        {
            vrInterface = gameObject.AddComponent<VRControllerInterface>();
            vrInterface.RegisterGripPressListener(OnGripAction);
        }
	}

    private void OnGripAction(bool isPressed)
    {
        if (isPressed)
        {
            if (!IsGrabbing && CurrentTarget != null)
            {
                CurrentTrackedObject.LockToManipulator(transform);
                IsGrabbing = true;
            }
            else if (IsGrabbing)
            {
                CurrentTrackedObject.ReleaseFromManipulator();
                IsGrabbing = false;
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!IsGrabbing)
        {
            TrackedObject obj = other.gameObject.GetComponent<TrackedObject>();
            if (obj != null)
            {
                CurrentTarget = other.transform;
                CurrentTrackedObject = obj;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (!IsGrabbing && CurrentTarget != null && other.transform == CurrentTarget)
        {
            CurrentTarget = null;
            CurrentTrackedObject = null;
        }
    }
}
