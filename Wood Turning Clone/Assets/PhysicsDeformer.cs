using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsDeformer : MonoBehaviour
{
    public DeformableMesh deformableMesh;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.layer != 8) return;
        foreach(var contact in collision.contacts)
        {
            deformableMesh.AddDepression(contact.point, collision.collider.bounds.extents.x/8);
        }
    }
}
