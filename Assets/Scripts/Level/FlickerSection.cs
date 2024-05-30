using System.Collections;
using UnityEngine;

public class FlickerSection : MonoBehaviour
{
    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private Collider meshCollider;

    private bool toggleState = true; 
    
    private void Awake()
    {
        if (renderer == null)
            renderer = GetComponent<MeshRenderer>();
        
        if (meshCollider == null)
            meshCollider = GetComponent<Collider>();
    }

    void Start()
    {
        StartCoroutine(flicker());
    }

    IEnumerator flicker()
    {
        while (true)
        {
            Toggle();
            yield return new WaitForSeconds(5f);
        }
    }

    private void Toggle()
    {
        toggleState = !toggleState;
        renderer.enabled = toggleState;
        meshCollider.enabled = toggleState;
    }
}
