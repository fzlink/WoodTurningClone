using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class WoodSubject : MonoBehaviour
{
    ProBuilderMesh mesh;
    
    Camera mainCamera;
    RaycastHit hit;
    Collider collider;


    public int heightSegmentCount = 48;
    public int height = 3;
    public int faceLoopCount = 8;

    private void Awake()
    {
        mesh = GetComponent<ProBuilderMesh>();
        mainCamera = Camera.main;
        collider = GetComponent<Collider>();
    }
    private void Start()
    {
        /*Vertex[] vertices = mesh.GetVertices();
        List<Face> tmpfaces = new List<Face>();
        for (int i = 0; i < mesh.faces.Count/4; i++)
        {
            tmpfaces.Add(mesh.faces[i]);
        }
        //mesh.SetVertices(vertices);
        mesh.Extrude(mesh.faces.Take(24),ExtrudeMethod.FaceNormal, -0.1f);
        ConnectElements.Connect(mesh, mesh.faces);
        //mesh.Connect(faces);
        mesh.ToMesh();
        mesh.Refresh();*/
    }

    private void Update()
    {
#if UNITY_STANDALONE
        if (Input.GetMouseButton(0))
        {

            Vector3 mousePos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z + 10));
            if(Physics.Raycast(mainCamera.transform.position,mousePos- mainCamera.transform.position,out hit))
            {
                Carve(hit.point);
            }

        }
#elif UNITY_ANDROID || UNITY_IOS
if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
            {
                Ray ray = mainCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hit))
                {
                    Carve(hit.point);
                }
            }
 
        }
#endif
    }

    private void Carve(Vector3 hitPoint)
    {
        int segmentIndexToCarve = 0;
        Vector3 minPoint = collider.bounds.min;
        float segmentSize = (float)height / heightSegmentCount;
        for (int i = 0; i <= heightSegmentCount; i++)
        {
            if (hitPoint.x <= minPoint.x + segmentSize * i)
            {
                segmentIndexToCarve = i;
                break;
            }
        }
        List<Face> facesToCarve = new List<Face>();
        List<Edge> edgesToCarve = new List<Edge>();
        for (int i = 0; i < faceLoopCount; i++)
        {
            int index = faceLoopCount * segmentIndexToCarve + i;
            facesToCarve.Add(mesh.faces[index]);
            edgesToCarve.AddRange(mesh.faces[index].edges);
        }

        mesh.Extrude(facesToCarve, ExtrudeMethod.FaceNormal, -Time.deltaTime/2);
        //mesh.Extrude(edgesToCarve, -Time.deltaTime / 2, true, false);

        mesh.ToMesh();
        mesh.Refresh();
    }
}
