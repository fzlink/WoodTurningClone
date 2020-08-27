using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarveParticleManager : MonoBehaviour
{
    public ParticleSystem particleObject;
    public Transform particleContainer;
    private Stack<ParticleSystem> deactiveParticles;
    private int pooledParticleSize = 100;
    private float deactivateWaitTime = 1f;

    void Start()
    {
        deactiveParticles = new Stack<ParticleSystem>();
        for (int i = 0; i < pooledParticleSize; i++)
        {
            deactiveParticles.Push(Instantiate(particleObject, particleContainer));
            deactiveParticles.Peek().gameObject.SetActive(false);
        }
    }

    public void PlayParticleAtLocation(Vector3 location)
    {
        ParticleSystem particle = deactiveParticles.Pop();
        particle.gameObject.SetActive(true);
        particle.transform.position = location;
        particle.Play();
        StartCoroutine(DeactivateParticle(particle));
    }

    private IEnumerator DeactivateParticle(ParticleSystem particle)
    {
        yield return new WaitForSeconds(deactivateWaitTime);
        particle.gameObject.SetActive(false);
        deactiveParticles.Push(particle);
    }
}
