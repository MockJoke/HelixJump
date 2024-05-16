using UnityEngine;

public class Section : MonoBehaviour
{
    [SerializeField] private MeshRenderer sectionRenderer;
    [SerializeField] private SectionType sectionType;
    
    private void Awake()
    {
        if (sectionRenderer == null)
            sectionRenderer = GetComponent<MeshRenderer>();
    }

    public void SetupSection(SectionType type, Color color)
    {
        sectionType = type;
        sectionRenderer.material.color = color;
        
        if (sectionType == SectionType.empty)
        {
            gameObject.SetActive(false);
        }
    }
}

public enum SectionType
{
    empty = -1,
    normal = 0,
    danger = 1,
    goal = 2
}