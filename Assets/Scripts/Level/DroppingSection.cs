using UnityEngine;

public class DroppingSection : MonoBehaviour
{
    public void Drop()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        GetComponent<MeshCollider>().enabled = false;
        
        GetComponentInParent<Ring>().RemoveSection(gameObject.GetComponent<Section>());
        
        transform.parent = null;
        Destroy(gameObject, 1.15f);
    }
}
