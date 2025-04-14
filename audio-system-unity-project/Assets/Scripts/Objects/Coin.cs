using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private SpriteRenderer visual;
    private ParticleSystem collectParticle;
    private bool collected = false;

    StudioEventEmitter emitter;

    private void Awake()
    {
        visual = this.GetComponentInChildren<SpriteRenderer>();
        collectParticle = this.GetComponentInChildren<ParticleSystem>();
        collectParticle.Stop();

    }

    private void Start()
    {
        emitter = AudioManager.instance.CreateEmitter(FMODEvents.instance.coinIdleEventRef, transform, EmitterGameEvent.ObjectStart, EmitterGameEvent.None, true, 1f, 6f);
    }

    private void OnTriggerEnter2D()
    {
        if (!collected)
        {
            collectParticle.Play();
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        collected = true;
        visual.gameObject.SetActive(false);

        AudioManager.instance.PlayOneShot(FMODEvents.instance.coinCollectedEventRef, transform.position);
        emitter.Stop();

        GameEventsManager.instance.CoinCollected();
    }

}
