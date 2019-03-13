using System.Collections.Generic;
using UnityEngine;

public class AnimationRecorder : MonoBehaviour
{
    public List<TrackedObject> TrackedObjects;
    public float Timestep = 0.1f;

    private bool isRecording;
    private bool isPlaying;
    private float recordingTime;
    private float currentTimestepTime;

	void Start ()
    {
        isRecording = false;
        isPlaying = false;
    }

    private void InitializeRecording()
    {
        for (int i = 0, count = TrackedObjects.Count; i < count; ++i)
        {
            TrackedObjects[i].InitializeRecord();
        }
    }

    private void InitializePlay()
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
                Debug.Log("Recording...");
                currentTimestepTime = 0f;
                recordingTime = 0f;
                InitializeRecording();
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
}
