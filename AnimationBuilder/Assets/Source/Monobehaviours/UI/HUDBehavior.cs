using UnityEngine;
using Utils;

public class HUDBehavior : MonoBehaviour
{
    private const float CAM_DIST = 0.6f;
    private readonly Vector3 HUD_TARGET_OFFSET = new Vector3(0f, 0f, CAM_DIST);
    public Transform PlayerCamera;
    public float TrackingTime = 0.3f;
    private Transform hudReference;
    private Vector3 hudVelocity;

	public void Start ()
    {
        if (PlayerCamera == null)
        {
            GameObject playerContainer = GameObject.Find("OVRPlayerController");
            PlayerCamera = UnityUtils.FindGameObject(playerContainer, "CenterEyeAnchor").transform;
        }
        hudReference = (new GameObject()).transform;
        hudReference.parent = PlayerCamera;
        hudReference.transform.localPosition = HUD_TARGET_OFFSET;
	}
	
	public void Update ()
    {
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            hudReference.position, 
            ref hudVelocity, 
            TrackingTime);

        Vector3 vecToHud = transform.position - PlayerCamera.position;
        Vector3 upVector = Vector3.Cross(vecToHud, transform.right);
        transform.rotation = 
            Quaternion.LookRotation(transform.position - PlayerCamera.position, upVector);
        Vector3 euler = transform.eulerAngles;
        euler.z = 0f;
        transform.eulerAngles = euler;
	}

    public void LateUpdate()
    {
        Vector3 currentPos = (transform.position - PlayerCamera.position).normalized * CAM_DIST;
        transform.position = currentPos + PlayerCamera.position;
    }
}
