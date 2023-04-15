using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public enum Modo { Increasing, Decreasing }

    [SerializeField] Modo modo;
    [SerializeField] float startTime;
    [SerializeField] float stopTime;
    [SerializeField] TextMeshPro timerText;
    float timer;
    bool start = false;
    public void StartTimer()
    {
        timer = startTime;
        start = true;
    }

    public float StopTimer()
    {
        start = false;
        return timer;
    }

    public float GetTime()
    {
        return timer;
    }

    private void Update()
    {
        if (start)
        {
            if(modo == Modo.Increasing)
            {
                timer += Time.deltaTime;
                if (timer >= stopTime)
                    start = false;
            }
            else
            {
                timer -= Time.deltaTime;
                if (timer <= stopTime)
                    start = false;
            }

        }
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = string.Format("{0:00}.{1:00}",minutes,seconds);
    }
}
