using System.Collections.Generic;
using UnityEngine;

public struct Keyframe
{
    public float timeStamp;
    public Vector3 position;
    public Vector3 rotation;

    public Keyframe(float timeStamp, Vector3 position, Vector3 rotation)
    {
        this.timeStamp = timeStamp;
        this.position = position;
        this.rotation = rotation;
    }
}

public class TrackedObject : MonoBehaviour
{
    public string AnimationName;
    public List<Keyframe> Keyframes;

    [HideInInspector]
    public float StopTime;

    private int currentKeyframeIndex;

    public void InitializeRecord()
    {
        Keyframes = new List<Keyframe>();
    }

    public void RecordKeyframe(float timeStamp)
    {
        StopTime = timeStamp;
        Keyframes.Add(
            new Keyframe(timeStamp, transform.localPosition, transform.localEulerAngles));
    }

    public void InitializePlay()
    {
        currentKeyframeIndex = 0;
    }

    public void ScrubToTime(float timeStamp)
    {
        int numKeyframes = Keyframes.Count;

        for (int i = currentKeyframeIndex; i < numKeyframes; ++i)
        {
            if (timeStamp > Keyframes[i].timeStamp)
            {
                currentKeyframeIndex = i;
            }
        }

        Keyframe currentFrame = Keyframes[currentKeyframeIndex];
        transform.localEulerAngles = currentFrame.rotation;
        transform.localPosition = currentFrame.position;
    }
}
