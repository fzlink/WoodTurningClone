using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Carver : MonoBehaviour
{

    Camera mainCamera;
    public float followSpeed = 1f;
    public float maximumSpeed = 1f;
    private Transform dragObj = null;
    private float length;
    private int fingerId = -1;
    RaycastHit hit;

    public Transform boxTip;
    public Transform triangleTip;
    public Transform archTip;

    private void Awake()
    {
        fingerId = 0;
    }

    void Start()
    {
        mainCamera = Camera.main;
        StateHandler.instance.OnStateChanged += OnStateChanged;

        dragObj = transform;
        length = (transform.position - mainCamera.transform.position).magnitude;

        /*Ray ray = mainCamera.ScreenPointToRay(transform.position);
        if (!dragObj)
        {
            if (Physics.Raycast(ray, out hit) && hit.rigidbody)
            {
                dragObj = hit.transform;
                length = hit.distance;
            }
        }*/

    }
    private void OnDestroy()
    {
        StateHandler.instance.OnStateChanged -= OnStateChanged;
    }

    private void OnStateChanged(State state)
    {
        if(state == State.Paint ||state == State.Evaluate)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
#if UNITY_STANDALONE
        if (EventSystem.current.IsPointerOverGameObject()) return;
            if (Input.GetMouseButton(0))
            Drag(Input.mousePosition);
        //else
        //dragObj = null;
#elif UNITY_ANDROID || UNITY_IOS

        if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;
            else if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                Drag(touch.position);
            }
            /*else
            {
                dragObj = null;
            }*/
#endif
    }

    private void Drag(Vector3 inputPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(inputPosition);
        if (!dragObj)
        {
            if(Physics.Raycast(ray,out hit) && hit.rigidbody)
            {
                dragObj = hit.transform;
                length = hit.distance;
            }
        }
        else
        {
            Vector3 vel = (ray.GetPoint(length) - dragObj.position) * followSpeed;
            if (vel.magnitude > maximumSpeed)
                vel *= maximumSpeed / vel.magnitude;
            dragObj.gameObject.GetComponent<Rigidbody>().velocity = vel;
        }
    }

    public void ChangeTip(TipTypes tipType)
    {
        DisableTips();
        if(tipType == TipTypes.BoxTip)
        {
            boxTip.gameObject.SetActive(true);
        }
        else if(tipType == TipTypes.TriangleTip)
        {
            triangleTip.gameObject.SetActive(true);
        }
        else if( tipType == TipTypes.ArchTip)
        {
            archTip.gameObject.SetActive(true);
        }
    }
    private void DisableTips()
    {
        boxTip.gameObject.SetActive(false);
        triangleTip.gameObject.SetActive(false);
        archTip.gameObject.SetActive(false);
    }

}
