using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AudioMaster : MonoBehaviour
{

    public static AudioMaster AM;

    public float volume = 5;

    private float musictimer = 0;

    [SerializeField] private AudioSource song1;

    [SerializeField] private AudioSource sound1;
    [SerializeField] private AudioSource sound2;
    [SerializeField] private AudioSource sound3;
    [SerializeField] private AudioSource sound4;
    [SerializeField] private AudioSource sound5;
    [SerializeField] private AudioSource sound6;
    [SerializeField] private AudioSource sound7;
    [SerializeField] private AudioSource sound8;
    [SerializeField] private AudioSource sound9;
    [SerializeField] private AudioSource sound10;
    [SerializeField] private AudioSource sound11;
    [SerializeField] private AudioSource sound12;
    [SerializeField] private AudioSource sound13;
    [SerializeField] private AudioSource sound14;
    [SerializeField] private AudioSource sound15;

    void Awake()
    {
        if (AM == null)
        {
            DontDestroyOnLoad(gameObject);
            AM = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        song1.Play();
        musictimer = 30;
    }

    void FixedUpdate()
    {
        AudioListener.volume = volume / 10.0f;
        musictimer -= Time.fixedDeltaTime;
        if (musictimer < 0)
        {
            song1.Play();
            musictimer = 30;
        }
    }

    public void StopMusic()
    {
        song1.Stop();
    }

    public void Death()
    {
        song1.Stop();
        musictimer = 3;
        Invoke("ReloadLevel", 3);
    }

    public void Reloadlevel()
    {
        SceneManager.LoadScene(0);
    }

    public void Sound(int sound)
    {
        if (sound == 1)
        {
            sound1.Play();
        }
        if (sound == 2)
        {
            sound2.Play();
        }
        if (sound == 3)
        {
            sound3.Play();
        }
        if (sound == 4)
        {
            sound4.Play();
        }
        if (sound == 5)
        {
            sound5.Play();
        }
        if (sound == 6)
        {
            sound6.Play();
        }
        if (sound == 7)
        {
            sound7.Play();
        }
        if (sound == 8)
        {
            sound8.Play();
        }
        if (sound == 9)
        {
            sound9.Play();
        }
        if (sound == 10)
        {
            sound10.Play();
        }
        if (sound == 11)
        {
            sound11.Play();
        }
        if (sound == 12)
        {
            sound12.Play();
        }
        if (sound == 13)
        {
            sound13.Play();
        }
        if (sound == 14)
        {
            sound14.Play();
        }
        if (sound == 15)
        {
            sound15.Play();
        }
    }
}