using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HelixController : MonoBehaviour 
{
    [SerializeField] private Transform startRingTransform;
    [SerializeField] private Transform endRingTransform;
    [SerializeField] private Ring helixRingPrefab;
    
    [Header("Components")] 
    [SerializeField] private Renderer helixRenderer;

    [Space] 
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Volume ppVolume;
    [SerializeField] private Renderer ballRenderer;
    [SerializeField] private TrailRenderer ballTrailRenderer;
    
    [Space]
    [SerializeField] private GameObject splashPrefab;
    
    [Space]
    public LevelData levelData; 
    
    private readonly List<GameObject> spawnedRings = new List<GameObject>();

    private Vignette vignette;
    
    private float pillarHeight;
    private Vector3 initRotation = Vector3.zero;
    private Vector3 lastTapPos = Vector3.zero;
    private bool newTap = true;
    
    void Awake() 
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (helixRenderer == null)
            helixRenderer = GetComponent<Renderer>();

        if (ballRenderer == null)
            ballRenderer = FindObjectOfType<BallController>().GetComponent<Renderer>();

        if (ballTrailRenderer == null)
            ballTrailRenderer = ballRenderer.GetComponentInChildren<TrailRenderer>();

        ppVolume.profile.TryGet(out vignette);
        
        initRotation = transform.localEulerAngles;
        pillarHeight = startRingTransform.localPosition.y - (endRingTransform.localPosition.y + .1f);         //distance bw top and goal platform
        
        // LoadLevel(0);
    }
	
	void Update() 
    {
        // spin helix by using click (or touch) & drag
        if (Input.GetMouseButton(0))
        {
            Vector3 curTapPos = Input.mousePosition;

            // if (lastTapPos == Vector3.zero)
            if (newTap)    
                lastTapPos = curTapPos;

            float delta = lastTapPos.x - curTapPos.x;
            transform.Rotate(Vector3.up * delta);

            lastTapPos = curTapPos;
            newTap = false;
        }
        
        // set newTap once the input is released
        if (Input.GetMouseButtonUp(0))
        {
            newTap = true;
            // lastTapPos = Vector3.zero;
        }
    }

    public void LoadLevel(int levelNumber)
    {
        // remove rings from prev level
        foreach (GameObject GameObj in spawnedRings)
        {
            Destroy(GameObj);
        }
        spawnedRings.Clear();
        
        // levelNumber = Mathf.Clamp(levelNumber, 0, levelData.levels.Count - 1);
        Level level = levelData.levels[levelNumber];
        
        mainCamera.backgroundColor = levelData.levels[levelNumber].BgColor;
        vignette.color.Override(levelData.levels[levelNumber].BgVignetteColor);
        helixRenderer.material.color = levelData.levels[levelNumber].PillarColor;
        ballRenderer.material.color = levelData.levels[levelNumber].BallColor;
        ballTrailRenderer.materials[0].color = levelData.levels[levelNumber].BallColor;
        splashPrefab.GetComponent<Renderer>().sharedMaterial.color = levelData.levels[levelNumber].BallColor;
        transform.localEulerAngles = initRotation;
        
        //create the new levels
        float distBwRings = pillarHeight / (level.rings.Count + 1);           // how far apart each ring should be
        
        // for vertical-pos of level, initially set it to top platform and then gradually subtract from it
        float initSpawnPosY = startRingTransform.localPosition.y;
        
        // Start Ring 
        Ring startRing = Instantiate(helixRingPrefab, transform); 
        startRing.transform.localPosition = new Vector3(0, initSpawnPosY, 0);
        spawnedRings.Add(startRing.gameObject);
        startRing.SetupAsStartRing(levelData.levels[levelNumber].NormalSectionColor);
        
        // Middle Rings
        for (int i = 0; i < level.rings.Count; i++)
        {
            initSpawnPosY -= distBwRings;         //subtracting distance bw two rings to get the pos for next ring

            Ring ring = Instantiate(helixRingPrefab, transform); 
            ring.transform.localPosition = new Vector3(0, initSpawnPosY, 0);
            spawnedRings.Add(ring.gameObject);
            
            ring.SetupRing(level.rings[i], levelData.levels[levelNumber].NormalSectionColor, levelData.levels[levelNumber].DangerSectionColor);
        }
        
        // End Ring
        Ring endRing = Instantiate(helixRingPrefab, transform); 
        endRing.transform.localPosition = new Vector3(0, endRingTransform.localPosition.y, 0);
        spawnedRings.Add(endRing.gameObject);
        endRing.SetupAsEndRing(levelData.levels[levelNumber].GoalSectionColor);
    }
}