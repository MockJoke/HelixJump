using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ring : MonoBehaviour
{
    [SerializeField] private List<Section> sections;
    
    [Header("Explosion Related Fields")]
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
            GameManager.Instance.IncreasePassedRingCnt();
            GameManager.Instance.AddScore(2);
            
            foreach (Section section in sections)
            {
                Rigidbody rb = section.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                
                    rb.AddExplosionForce(explosionForce, transform.position, radius, 0f, ForceMode.Impulse);
                }

                section.GetComponent<MeshCollider>().enabled = false;
                
                section.transform.parent = null;
                Destroy(section.gameObject, 1.15f);
                
                Destroy(this.gameObject, 2.25f);
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
        List<Section> normalSections = new List<Section>();

        foreach (Section section in sections)
        {
            section.SetupSection(SectionType.normal, normalSectionColor);
            normalSections.Add(section);
        }
        
        List<Section> emptySections = new List<Section>();

        while (emptySections.Count < (12 - ringData.totalSections))            
        {
            Section randEmptySection = normalSections[Random.Range(0, normalSections.Count)];
                
            if (!emptySections.Contains(randEmptySection))
            {
                randEmptySection.SetupSection(SectionType.empty, normalSectionColor);
                emptySections.Add(randEmptySection);
                
                normalSections.Remove(randEmptySection);
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
        
        List<Section> droppingSections = new List<Section>();

        while (droppingSections.Count < ringData.droppingSections)
        {
            Section randDroppingSection = normalSections[Random.Range(0, normalSections.Count)];

            if (!droppingSections.Contains(randDroppingSection))
            {
                randDroppingSection.gameObject.AddComponent<DroppingSection>();
                randDroppingSection.SetupSection(SectionType.drop, normalSectionColor);

                droppingSections.Add(randDroppingSection);
                normalSections.Remove(randDroppingSection);
            }
        }
        
        List<Section> blinkingSections = new List<Section>();

        while (blinkingSections.Count < ringData.blinkingSections)
        {
            Section randBlinkingSection = normalSections[Random.Range(0, normalSections.Count)];

            if (!blinkingSections.Contains(randBlinkingSection))
            {
                randBlinkingSection.gameObject.AddComponent<BlinkingSection>();
                randBlinkingSection.SetupSection(SectionType.blink, normalSectionColor);

                blinkingSections.Add(randBlinkingSection);
                normalSections.Remove(randBlinkingSection);
            }
        }
    }

    public void RemoveSection(Section section)
    {
        sections.Remove(section);
    }
}