using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] private Section[] sections;

    void Awake()
    {
        if (sections == null)
            sections = transform.GetComponentsInChildren<Section>();
    }

    public void SetupAsStartRing(Color ringColor)
    {
        foreach (Section section in sections)
        {
            section.SetupSection(SectionType.normal, ringColor);
        }
        
        Section randomPart = sections[Random.Range(0, sections.Length)];
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
            Section randEmptySection = sections[Random.Range(0, sections.Length)];
                
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