using UnityEngine;
using UnityEngine.UI;

public class HUDScreen : MonoBehaviour
{
    public Text TimerText;
    public Text StatusText;

    private bool isRecording;
    private float currentTime;

    public void Start()
    {
        TimerText.gameObject.SetActive(false);
        Service.EventManager.AddListener(EventId.RecordingStarted, OnRecordingStarted);
        Service.EventManager.AddListener(EventId.RecordingEnded, OnRecordingEnded);
    }

    private bool OnRecordingStarted(object cookie)
    {
        TimerText.gameObject.SetActive(true);
        StatusText.text = "Recording";
        isRecording = true;
        currentTime = 0f;
        return false;
    }

    private bool OnRecordingEnded(object cookie)
    {
        Service.TimerManager.CreateTimer(3f, HideTimerText, null);
        StatusText.text = "Idle";
        isRecording = false;
        return false;
    }

    public void HideTimerText(object cookie)
    {
        if (!isRecording)
        {
            TimerText.gameObject.SetActive(false);
        }
    }

    void Update ()
    {
        if (isRecording)
        {
            currentTime += Time.deltaTime;
            TimerText.text = currentTime.ToString("00:00");
        }
	}
}
