using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    //FMOD VARIABLES
    [FMODUnity.EventRef]
    public string keyPath;

    private FMOD.Studio.EventInstance keyRef;

    public string keyType;
    public Text KeyNameText;

    void OnEnable()
    {
        KeyNameText.text = keyType;
        //FMOD INITIALIZE EVENTS
        keyRef = FMODUnity.RuntimeManager.CreateInstance(keyPath);
        //INITIALIZE WHERE SOUND COMES FROM
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(keyRef, GetComponent<Transform>(), GetComponent<Rigidbody>());
    }

    void OnTriggerEnter(Collider other)
    {
        var keychain = other.GetComponent<Keychain>();

        if (keychain != null)
        {
            //PLAY KEY SOUND
            keyRef.start();

            keychain.GrabbedKey(keyType);
            Destroy(gameObject);
        }
    }
}
