using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spray : MonoBehaviour
{
    private Camera mainCamera;
    public float followSpeed = 5f;
    public Transform orbitTransform;
    private float distanceFromCamera;
    private Vector3 cursorPos;
    private bool isPressed;
    public float orbitXOffset = 0.1f;
    public Renderer canRenderer;
    public ParticleSystem particle;
    public Transform lookDownPoint;

    void Start()
    {
        mainCamera = Camera.main;

        distanceFromCamera = (transform.position - mainCamera.transform.position).magnitude;
        GetToDefaultState();
    }



    void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        ProcessTouch();
#elif UNITY_STANDALONE
        ProcessMouseDrag();
#endif



        if (cursorPos == Vector3.zero) return;
        float offset;
        if (transform.position.x < orbitTransform.position.x)
            offset = -orbitXOffset;
        else
            offset = orbitXOffset;
        transform.position = Vector3.Lerp(transform.position, cursorPos + Vector3.back, Time.deltaTime * followSpeed);
    }

    private void LookDown(Vector3 inputPos)
    {
        RaycastHit hit;
        if (Physics.Raycast(inputPos, Vector3.down,out hit))
        {
            GetToSprayState();
        }
        else
        {
            GetToDefaultState();
        }
    }

    private void GetToSprayState()
    {
        if (!DOTween.IsTweening(transform))
        {
            transform.DORotate(Vector3.right * 75, 0.1f);
            transform.DOScale(2, 0.1f);
        }
        PlayParticle();
    }

    private void GetToDefaultState()
    {
        if (!DOTween.IsTweening(transform))
        {
            transform.DORotate(Vector3.right * 30, 0.1f);
            transform.DOScale(4, 0.1f);
        }
        StopParticle();
    }

    private void PlayParticle()
    {
        if (!particle.isPlaying)
            particle.Play();
    }

    private void StopParticle()
    {
        if (particle.isPlaying)
            particle.Stop();
    }

    private void ProcessMouseDrag()
    {
        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera);
            Vector3 pos = mainCamera.ScreenToWorldPoint(mousePosition);
            LookDown(pos);
            cursorPos = pos;
        }
        else
            StopParticle();
    }
    public void ChangeColor(Color currentColor)
    {
        canRenderer.material.color = currentColor;
    }

    public void SetCursorPos(Vector3 cursorPos)
    {
        this.cursorPos = mainCamera.ScreenToWorldPoint(cursorPos);
    }

#if UNITY_ANDROID || UNITY_IOS
    private void ProcessTouch()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;
            if(touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                Vector3 touchedPos = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, distanceFromCamera));
                LookDown(touchedPos);
                cursorPos = touchedPos;
            }
        }
        else
            StopParticle();

    }
#endif

}
