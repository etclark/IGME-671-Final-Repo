using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class UIAudioPlayer : MonoBehaviour
{

    public static UIAudioPlayer Instance { get; private set; }

    public AudioClip PositiveSound;
    public AudioClip NegativeSound;
    //FMOD VARIABLES
    //[FMODUnity.EventRef]
    //public string confirmPath;
    //[FMODUnity.EventRef]
    //public string negativePath;

    AudioSource m_Source;

    //private FMOD.Studio.EventInstance confirmRef;
    //private FMOD.Studio.EventInstance negativeRef;

    void Awake()
    {
        //FMOD INITIALIZE EVENTS
        //confirmRef = FMODUnity.RuntimeManager.CreateInstance(confirmPath);
        //negativeRef = FMODUnity.RuntimeManager.CreateInstance(negativePath);
        //INITIALIZE WHERE SOUND COMES FROM
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(confirmRef, GetComponent<Transform>(), GetComponent<Rigidbody>());
        //FMODUnity.RuntimeManager.AttachInstanceToGameObject(negativeRef, GetComponent<Transform>(), GetComponent<Rigidbody>());

        m_Source = GetComponent<AudioSource>();
        Instance = this;
    }

    public static void PlayPositive()
    {
        Instance.m_Source.PlayOneShot(Instance.PositiveSound);
        //PLAY CONFIRM SOUND
        //confirmRef.start();
    }

    public static void PlayNegative()
    {
        Instance.m_Source.PlayOneShot(Instance.NegativeSound);
        //PLAY NEGATIVE SOUND
        //negativeRef.start();
    }
}
