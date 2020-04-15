using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Target : MonoBehaviour
{
    public float health = 5.0f;
    public int pointValue;

    public ParticleSystem DestroyedEffect;

    //FMOD VARIABLES
    [FMODUnity.EventRef]
    public string deathEvent;
    [FMODUnity.EventRef]
    public string hitEvent;
    [FMODUnity.EventRef]
    public string idleEvent;

    private FMOD.Studio.EventInstance deathRef;
    private FMOD.Studio.EventInstance hitRef;
    private FMOD.Studio.EventInstance idleRef;

    [Header("Audio")]
    public RandomPlayer HitPlayer;
    public AudioSource IdleSource;
    
    public bool Destroyed => m_Destroyed;

    bool m_Destroyed = false;
    float m_CurrentHealth;

    void Awake()
    {
        Helpers.RecursiveLayerChange(transform, LayerMask.NameToLayer("Target"));
    }

    void Start()
    {
        if(DestroyedEffect)
            PoolSystem.Instance.InitPool(DestroyedEffect, 16);
        
        m_CurrentHealth = health;
        if(IdleSource != null)
            IdleSource.time = Random.Range(0.0f, IdleSource.clip.length);

        //FMOD INITIALIZE EVENTS
        deathRef = FMODUnity.RuntimeManager.CreateInstance(deathEvent);
        hitRef = FMODUnity.RuntimeManager.CreateInstance(hitEvent);
        idleRef = FMODUnity.RuntimeManager.CreateInstance(idleEvent);

        //INITIALIZE WHERE SOUND COMES FROM
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(deathRef, GetComponent<Transform>(), GetComponent<Rigidbody>());
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(hitRef, GetComponent<Transform>(), GetComponent<Rigidbody>());
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(idleRef, GetComponent<Transform>(), GetComponent<Rigidbody>());

        //PLAY TARGET IDLE
        idleRef.start();
    }

    public void Got(float damage)
    {
        m_CurrentHealth -= damage;
        
        if(HitPlayer != null)
            HitPlayer.PlayRandom();
        //PLAY TARGET IS HIT
            hitRef.start();
        
        if(m_CurrentHealth > 0)
            return;

        Vector3 position = transform.position;
        
        //the audiosource of the target will get destroyed, so we need to grab a world one and play the clip through it
        if (HitPlayer != null)
        {
            var source = WorldAudioPool.GetWorldSFXSource();
            source.transform.position = position;
            source.pitch = HitPlayer.source.pitch;
            source.PlayOneShot(HitPlayer.GetRandomClip());
            //PLAY TARGET DEATH
            deathRef.start();
            //STOP TARGET IDLE
            idleRef.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }

        if (DestroyedEffect != null)
        {
            var effect = PoolSystem.Instance.GetInstance<ParticleSystem>(DestroyedEffect);
            effect.time = 0.0f;
            effect.Play();
            effect.transform.position = position;
        }

        m_Destroyed = true;
        
        gameObject.SetActive(false);
       
        GameSystem.Instance.TargetDestroyed(pointValue);
    }
}
