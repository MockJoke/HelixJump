using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlinkingSection : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Collider meshCollider;

    private bool blinkState = false; 
    
    private void Awake()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();
        
        if (meshCollider == null)
            meshCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        StartCoroutine(Blink());
    }
    
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    IEnumerator Blink()
    {
        while (true)
        {
            blinkState = !blinkState;
            
            meshRenderer.enabled = blinkState;
            meshCollider.enabled = blinkState;
            
            yield return new WaitForSeconds(Random.Range(0.75f, 2.25f));
        }
    }
}
