using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carver : MonoBehaviour
{

    Camera mainCamera;
    public float followSpeed = 1f;
    public float maximumSpeed = 1f;
    private Transform dragObj = null;
    private float length;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE
        if (Input.GetMouseButton(0))
            Drag(Input.mousePosition);
        else
            dragObj = null;
#elif UNITY_ANDROID || UNITY_IOS
if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Drag(touch.position);
            }
            else
            {
                dragObj = null;
            }
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

#if UNITY_STANDALONE
    private void OnMouseDrag()
    {
        /*Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4.5f);
        Vector3 pos = mainCamera.ScreenToWorldPoint(mousePosition);
        pos.y = transform.position.y;
        transform.position = pos;*/
    }
#endif

#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                Vector3 touchedPos = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, transform.position.z - mainCamera.transform.position.z));
                transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime * touchFollowSpeed);
            }
        }
#endif

}
