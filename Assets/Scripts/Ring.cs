using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ring : MonoBehaviour
{
    [SerializeField] private List<Section> sections;

    [SerializeField] private float explosionForce = 100f;
    [SerializeField] private float radius = 500f;
    
    private Transform ball;
    
    void Awake()
    {
        if (sections == null)
            sections = transform.GetComponentsInChildren<Section>().ToList();

        if (ball == null)
            ball = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (transform.position.y > ball.position.y + 0.1f)
        {
            foreach (Section section in sections)
            {
                Rigidbody rb = section.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                
                    rb.AddExplosionForce(explosionForce, transform.position, radius, 0f, ForceMode.Impulse);
                }

                section.transform.parent = null;
                // sections.Remove(section);
                
                Destroy(section.gameObject, 2f);
                
                Destroy(this.gameObject, 3.5f);
            }
            
            this.enabled = false;
        }
    }

    public void SetupAsStartRing(Color ringColor)
    {
        foreach (Section section in sections)
        {
            section.SetupSection(SectionType.normal, ringColor);
        }
        
        Section randomPart = sections[Random.Range(0, sections.Count)];
        randomPart.SetupSection(SectionType.empty, ringColor);
    }

    public void SetupAsEndRing(Color ringColor)
    {
        foreach (Section section in sections)
        {
            section.SetupSection(SectionType.goal, ringColor);
            section.gameObject.AddComponent<GoalSection>();
        }
    }
    
    public void SetupRing(RingData ringData, Color normalSectionColor, Color dangerSectionColor)
    {
        int partsToDisable = 12 - ringData.totalSections; 
        List<Section> emptySections = new List<Section>();

        while (emptySections.Count < partsToDisable)            
        {
            Section randEmptySection = sections[Random.Range(0, sections.Count)];
                
            if (!emptySections.Contains(randEmptySection))
            {
                randEmptySection.SetupSection(SectionType.empty, normalSectionColor);
                emptySections.Add(randEmptySection);
            }
        }

        List<Section> normalSections = new List<Section>();

        foreach (Section section in sections)
        {
            if (section.gameObject.activeInHierarchy)
            {
                section.SetupSection(SectionType.normal, normalSectionColor);
                normalSections.Add(section);
            }
        }
        
        List<Section> dangerSections = new List<Section>();

        while (dangerSections.Count < ringData.dangerSections)
        {
            Section randDangerSection = normalSections[Random.Range(0, normalSections.Count)];

            if (!dangerSections.Contains(randDangerSection))
            {
                randDangerSection.gameObject.AddComponent<DangerSection>();
                randDangerSection.SetupSection(SectionType.danger, dangerSectionColor);

                dangerSections.Add(randDangerSection);
                normalSections.Remove(randDangerSection);
            }
        }
    }
}