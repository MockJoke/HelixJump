using UnityEngine;

public class Section : MonoBehaviour
{
    [SerializeField] private MeshRenderer sectionRenderer;

    private bool isDeadly = false;
    private bool isGoal = false;
    
    private void Awake()
    {
        if (sectionRenderer == null)
            sectionRenderer = GetComponent<MeshRenderer>();
    }

    public void SetColor(Color color)
    {
        sectionRenderer.material.color = color;
    }

    public void SetDeadlySection()
    {
        isDeadly = true;
    }
    
    public void SetGoalSection()
    {
        isGoal = true;
    }
}
