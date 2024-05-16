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
        GameObject randomPart = transform.GetChild(Random.Range(0, sections.Length)).gameObject;           //to randomise the disabled parts
        randomPart.SetActive(false);

        foreach (Section section in sections)
        {
            section.SetColor(ringColor);
        }
    }

    public void SetupAsEndRing(Color ringColor)
    {
        foreach (Section section in sections)
        {
            section.SetColor(ringColor);
            section.gameObject.AddComponent<GoalSection>();
        }
    }
    
    public void SetupRing(RingData ringData, Color ringColor, Color deathSectionColor)
    {
        //disable some parts (depending on level setup)
        int partsToDisable = 12 - ringData.totalSections;             //one ring consists of 12 sections 
        List<GameObject> disabledParts = new List<GameObject>();            //list of disabled parts

        while (disabledParts.Count < partsToDisable)            
        {
            GameObject randomPart = transform.GetChild(Random.Range(0, sections.Length)).gameObject;           //to randomise the disabled parts
                
            if (!disabledParts.Contains(randomPart))
            {
                randomPart.SetActive(false);
                disabledParts.Add(randomPart);
            }
        }

        //mark parts as death parts
        List<Section> leftParts = new List<Section>();            //list of death parts

        foreach (Section section in sections)
        {
            section.SetColor(ringColor);
            
            if (section.gameObject.activeInHierarchy)
                leftParts.Add(section);
        }
        
        //creating death parts
        List<Section> deathParts = new List<Section>();

        while (deathParts.Count < ringData.deathSections)
        {
            Section randomPart = leftParts[(Random.Range(0, leftParts.Count))];          //to randomise the left parts

            if (!deathParts.Contains(randomPart))
            {
                randomPart.gameObject.AddComponent<DeathSection>();        //adding DeathPart script to random part
                randomPart.SetColor(deathSectionColor);
                
                deathParts.Add(randomPart);
            }
        }
    }
}