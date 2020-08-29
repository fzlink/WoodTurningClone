using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Carver : MonoBehaviour
{

    private Rigidbody rb;
    private Camera mainCamera;

    public float followSpeed = 1f;
    private float distToCamera;

    float deltaX;
    float deltaZ;

    public Transform boxTip;
    public Transform triangleTip;
    public Transform archTip;

    Vector3 bottomLeftCorner;
    Vector3 topRightCorner;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        mainCamera = Camera.main;
        StateHandler.instance.OnStateChanged += OnStateChanged;
        distToCamera = (transform.position - mainCamera.transform.position).magnitude;
  
        bottomLeftCorner = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, distToCamera));
        topRightCorner = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, distToCamera));
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
        MouseMovement();
    #elif UNITY_ANDROID || UNITY_IOS
        TouchMovement();
    #endif
    }

#if UNITY_ANDROID || UNITY_IOS
    private void TouchMovement()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = mainCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, distToCamera));
            if (touch.phase == TouchPhase.Began && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    deltaX = touchPos.x - transform.position.x;
                    deltaZ = touchPos.z - transform.position.z;
                    break;
                case TouchPhase.Moved:
                    Vector3 moveTo = new Vector3(touchPos.x - deltaX, transform.position.y, touchPos.z - deltaZ);
                    rb.velocity = (moveTo - transform.position) * followSpeed;
                    break;
                case TouchPhase.Ended:
                    rb.velocity = Vector3.zero;
                    break;
            }
        }
    }
#elif UNITY_STANDALONE
    private void MouseMovement()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distToCamera));
        if (Input.GetMouseButtonDown(0))
        {
            deltaX = mousePos.x - transform.position.x;
            deltaZ = mousePos.z - transform.position.z;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 moveTo = new Vector3(mousePos.x - deltaX, transform.position.y, mousePos.z - deltaZ);
            rb.velocity = (moveTo - transform.position) * followSpeed;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            rb.velocity = Vector3.zero;
        }
    }
#endif
    private void LateUpdate()
    {
        ClampPosition();
    }

    private void ClampPosition()
    {
        Vector3 clampedPos = transform.position;
        clampedPos.x = Mathf.Clamp(clampedPos.x, bottomLeftCorner.x, topRightCorner.x);
        clampedPos.z = Mathf.Clamp(clampedPos.z, bottomLeftCorner.z, -0.5f);
        transform.position = clampedPos;
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
