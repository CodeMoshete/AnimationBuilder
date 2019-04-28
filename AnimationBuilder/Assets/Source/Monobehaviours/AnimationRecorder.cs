using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AnimationRecorder : MonoBehaviour
{
    public GameObject LeftHand;
    public GameObject RightHand;
    public GameObject Head;

    private ManipulatorComponent leftHandManipulator;
    private ManipulatorComponent rightHandManipulator;
    private ManipulatorComponent headManipulator;

    public List<TrackedObject> TrackedObjects;
    public float Timestep = 0.1f;

    private bool isRecording;
    private bool isPlaying;
    private float recordingTime;
    private float currentTimestepTime;

	void Start ()
    {
        if (LeftHand == null || RightHand == null || Head == null)
        {
            Debug.LogError("Tracking not assigned! Please ensure the CameraRig prefab is present!");
        }
        else
        {
            leftHandManipulator = LeftHand.AddComponent<ManipulatorComponent>();
            rightHandManipulator = RightHand.AddComponent<ManipulatorComponent>();
            headManipulator = Head.AddComponent<ManipulatorComponent>();
        }

        isRecording = false;
        isPlaying = false;

        Service.EventManager.AddListener(EventId.VRControllerTriggerPress, OnTriggerPressed);
    }

    private bool OnTriggerPressed(object cookie)
    {
        if (!isRecording)
        {
            Service.EventManager.SendEvent(EventId.RecordingStarted, null);
            InitializeRecording();
        }
        else
        {
            Service.EventManager.SendEvent(EventId.RecordingEnded, null);
            EndRecording();
        }
        return true;
    }

    private void InitializeRecording()
    {
        if (!isRecording)
        {
            Debug.Log("Recording...");
            currentTimestepTime = 0f;
            recordingTime = 0f;
            isRecording = true;

            for (int i = 0, count = TrackedObjects.Count; i < count; ++i)
            {
                TrackedObjects[i].InitializeRecord();
            }
        }
        else
        {
            Debug.LogError("Error! Finish the current recording before starting another.");
        }
    }

    public void EndRecording()
    {
        if (isRecording)
        {
            Debug.Log("Saving objects");
            for (int i = 0, count = TrackedObjects.Count; i < count; ++i)
            {
                SaveAnimationFile(TrackedObjects[i]);
            }
        }
        else
        {
            Debug.LogError("Error! No recording currently taking place!");
        }
    }

    public void InitializePlay()
    {
        for (int i = 0, count = TrackedObjects.Count; i < count; ++i)
        {
            TrackedObjects[i].InitializePlay();
        }
    }

    void Update()
    {
        float dt = Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R) && !isPlaying)
        {
            isRecording = !isRecording;

            if (isRecording)
            {
                InitializeRecording();
            }
            else
            {
                EndRecording();
            }
        }

        if (Input.GetKeyDown(KeyCode.P) && !isRecording)
        {
            isPlaying = !isPlaying;

            if (isPlaying)
            {
                Debug.Log("Playing...");
                recordingTime = 0f;
                InitializePlay();
            }
        }

        if (isRecording)
        {
            currentTimestepTime += dt;
            recordingTime += dt;

            if (currentTimestepTime > Timestep)
            {
                currentTimestepTime = 0f;
                for (int i = 0, count = TrackedObjects.Count; i < count; ++i)
                {
                    TrackedObjects[i].RecordKeyframe(recordingTime);
                }
            }
        }
        else if (isPlaying)
        {
            recordingTime += dt;
            for (int i = 0, count = TrackedObjects.Count; i < count; ++i)
            {
                TrackedObjects[i].ScrubToTime(recordingTime);
            }
        }
	}

    private void SaveAnimationFile(TrackedObject obj)
    {
        string output = 
            string.Format(AnimationStringUtils.FILE_CONTENTS_HEADER, obj.AnimationName);

        output += AnimationStringUtils.POST_ANIM_NAME_CONTENTS;
        for (int i = 0, count = obj.Keyframes.Count; i < count; ++i)
        {
            output += GenerateEulerCurveString(obj.Keyframes[i]);
        }
        output += AnimationStringUtils.CURVE_POS_SEP;
        for (int i = 0, count = obj.Keyframes.Count; i < count; ++i)
        {
            output += GeneratePosCurveString(obj.Keyframes[i]);
        }
        output += string.Format(AnimationStringUtils.STOP_TIME_PREFIX, obj.StopTime);
        output += AnimationStringUtils.EDITOR_CURVES_PREFIX;

        // Editor position curves
        for (int i = 0, count = obj.Keyframes.Count; i < count; ++i)
        {
            Keyframe frame = obj.Keyframes[i];
            output += GenerateEditorCurve(frame, frame.position.x);
        }
        output += AnimationStringUtils.EDITOR_POS_SEP_X;
        for (int i = 0, count = obj.Keyframes.Count; i < count; ++i)
        {
            Keyframe frame = obj.Keyframes[i];
            output += GenerateEditorCurve(frame, frame.position.y);
        }
        output += AnimationStringUtils.EDITOR_POS_SEP_Y;
        for (int i = 0, count = obj.Keyframes.Count; i < count; ++i)
        {
            Keyframe frame = obj.Keyframes[i];
            output += GenerateEditorCurve(frame, frame.position.z);
        }
        output += AnimationStringUtils.EDITOR_POS_SEP_Z;

        // Editor rotation curves
        for (int i = 0, count = obj.Keyframes.Count; i < count; ++i)
        {
            Keyframe frame = obj.Keyframes[i];
            output += GenerateEditorCurve(frame, frame.rotation.x);
        }
        output += AnimationStringUtils.EDITOR_EULER_SEP_X;
        for (int i = 0, count = obj.Keyframes.Count; i < count; ++i)
        {
            Keyframe frame = obj.Keyframes[i];
            output += GenerateEditorCurve(frame, frame.rotation.y);
        }
        output += AnimationStringUtils.EDITOR_EULER_SEP_Y;
        for (int i = 0, count = obj.Keyframes.Count; i < count; ++i)
        {
            Keyframe frame = obj.Keyframes[i];
            output += GenerateEditorCurve(frame, frame.rotation.z);
        }
        output += AnimationStringUtils.EDITOR_EULER_SEP_Z_END;

        File.WriteAllText("./Assets/" + obj.AnimationName + "_Animation.anim", output);
    }

    private string GenerateEulerCurveString(Keyframe frame)
    {
        string output = string.Format(
            AnimationStringUtils.CURVE_TEMPLATE, 
            frame.timeStamp,
            frame.rotation.x, 
            frame.rotation.y, 
            frame.rotation.z);

        return output;
    }

    private string GeneratePosCurveString(Keyframe frame)
    {
        string output = string.Format(
            AnimationStringUtils.CURVE_TEMPLATE,
            frame.timeStamp,
            frame.position.x,
            frame.position.y,
            frame.position.z);

        return output;
    }

    private string GenerateEditorCurve(Keyframe frame, float value)
    {
        string output = string.Format(
            AnimationStringUtils.EDITOR_CURVE_TEMPLATE,
            frame.timeStamp,
            value);

        return output;
    }
}
