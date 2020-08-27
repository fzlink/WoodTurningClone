using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Spray : MonoBehaviour
{
    private Camera mainCamera;
    public float touchFollowSpeed = 5f;
    public Transform orbitTransform;
    private float distanceFromCamera;
    private Vector3 cursorPos;
    public float orbitXOffset = 0.1f;
    public Renderer canRenderer;
    public ParticleSystem particle;

    void Start()
    {
        mainCamera = Camera.main;
        distanceFromCamera = (transform.position - mainCamera.transform.position).magnitude;
    }


    void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        ProcessTouch();
#elif UNITY_STANDALONE
        ProcessMouseDrag();
#endif
        transform.LookAt(orbitTransform.position,Vector3.up);
        if (cursorPos == Vector3.zero) return;
        float offset;
        if (transform.position.x < orbitTransform.position.x)
            offset = -orbitXOffset;
        else
            offset = orbitXOffset;
        transform.position = Vector3.Lerp(transform.position, cursorPos + Vector3.right*offset, Time.deltaTime * touchFollowSpeed);
    }

    private void ProcessMouseDrag()
    {
        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera);
            Vector3 pos = mainCamera.ScreenToWorldPoint(mousePosition);
            cursorPos = pos;
            if (!particle.isPlaying)
            {
                particle.Play();
            }
        }
        else
        {
            if (particle.isPlaying)
                particle.Stop();
        }


    }

    internal void ChangeColor(Color currentColor)
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
                cursorPos = touchedPos;
                if (!particle.isPlaying)
                {
                    particle.Play();
                }
            }
        }else{
                    if (particle.isPlaying)
                particle.Stop();
        }

    }
#endif

}
