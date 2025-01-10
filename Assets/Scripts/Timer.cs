using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour 
{
    public event System.Action OnTimerEnd; // Event triggered when the timer ends
    public float duration {  get; private set; }
    private float currentTime;// The current time remaining
    public bool isPlaying { get; private set; }
    public void StartTimer(float duration)
    {
        this.duration = duration;
        isPlaying = true;
        StartCoroutine(TimerCoroutine());
    }
    private IEnumerator TimerCoroutine()
    {
        while (isPlaying)
        {
            currentTime -= Time.deltaTime; // Decrease the time remaining

            if (currentTime <= 0f)
            {
                currentTime = 0f;
                OnTimerEnd?.Invoke(); // Trigger the OnTimerEnd event
                yield return new WaitForSeconds(duration); // Wait before restarting the timer
                currentTime = duration; // Reset timer
            }

            yield return null; // Wait for the next frame
        }
    }
    public void StopTimer()
    {
        isPlaying = false;
    }
}
