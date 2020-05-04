using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    //FMOD VARIABLES
    [FMODUnity.EventRef]
    public string pausePath;
    private FMOD.Studio.EventInstance pauseSnapRef;

    void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
        pauseSnapRef = FMODUnity.RuntimeManager.CreateInstance(pausePath);
    }

    public void Display()
    {
        gameObject.SetActive(true);
        GameSystem.Instance.StopTimer();
        Controller.Instance.DisplayCursor(true);
        if (IsPlaying(pauseSnapRef) == false)
        {
            pauseSnapRef.start(); 
        }   
    }

    public void OpenEpisode()
    {
        UIAudioPlayer.PlayPositive();
        gameObject.SetActive(false);
        LevelSelectionUI.Instance.DisplayEpisode();   
    }

    public void ReturnToGame()
    {
        UIAudioPlayer.PlayPositive();
        GameSystem.Instance.StartTimer();
        gameObject.SetActive(false);
        Controller.Instance.DisplayCursor(false);
        pauseSnapRef.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public bool IsPlaying(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
