using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float timeDuration = 1.5f * 60f;
    public float timer;
    bool isFreezingTimer = false;

    [SerializeField] TMP_Text firstMinute;
    [SerializeField] TMP_Text secondMinute;
    [SerializeField] TMP_Text separator;
    [SerializeField] TMP_Text firstSecond;
    [SerializeField] TMP_Text secondSecond;

    [SerializeField] AudioClip bgMusic;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0 && isFreezingTimer == false)
        {
            timer -= Time.deltaTime;
            UpdateTimerDisplay();
            if (audioSource.isPlaying == false)
            {
                audioSource.PlayOneShot(bgMusic);
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    public void ResetTimer()
    {
        timer = timeDuration;
    }

    public void FreezeTimer()
    {
        isFreezingTimer = true;
    }
    public void UnFreezeTimer()
    {
        isFreezingTimer = false;
    }

    void UpdateTimerDisplay()
    {
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);

        string currentTime = string.Format("{00:00}{1:00}", minutes, seconds);
        firstMinute.text = currentTime[0].ToString();
        secondMinute.text = currentTime[1].ToString();
        firstSecond.text = currentTime[2].ToString();
        secondSecond.text = currentTime[3].ToString();
    }
}
