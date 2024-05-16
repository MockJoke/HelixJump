using System.Collections.Generic;
using UnityEngine;

public class HelixController : MonoBehaviour 
{
    [SerializeField] private Transform startRingTransform;
    [SerializeField] private Transform endRingTransform;
    [SerializeField] private Ring helixRingPrefab;

    [SerializeField] private LevelData levelData; 
    private readonly List<GameObject> spawnedRings = new List<GameObject>();

    [Header("Components")] 
    [SerializeField] private Renderer helixRenderer;
    [SerializeField] private Camera mainCamera;

    [Space] 
    [SerializeField] private Renderer ballRenderer;
    
    private Vector3 initRotation;
    private float pillarHeight;
    private Vector2 lastTapPos;
    private bool newTap = true;
    
    void Awake() 
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (helixRenderer == null)
            helixRenderer = GetComponent<Renderer>();

        if (ballRenderer == null)
            ballRenderer = FindObjectOfType<BallController>().GetComponent<Renderer>();
            
        initRotation = transform.localEulerAngles;
        pillarHeight = startRingTransform.localPosition.y - (endRingTransform.localPosition.y + .1f);         //distance bw top and goal platform
        
        LoadStage(0);
    }
	
	void Update() 
    {
        //spin helix by using click (or touch) and drag
        if (Input.GetMouseButton(0))
        {
            Vector2 curTapPos = Input.mousePosition;

            // if (lastTapPos == Vector2.zero)
            if (newTap)    
                lastTapPos = curTapPos;

            float delta = lastTapPos.x - curTapPos.x;
            lastTapPos = curTapPos;

            transform.Rotate(Vector3.up * delta);       //how much to rotate

            newTap = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            newTap = true;
            lastTapPos = Vector2.zero;
        }
    }

    public void LoadStage(int levelNumber)
    {
        //remove rings from prev level
        foreach (GameObject GameObj in spawnedRings)
        {
            Destroy(GameObj);
        }
        spawnedRings.Clear();
        
        levelNumber = Mathf.Clamp(levelNumber, 0, levelData.levels.Count - 1);
        
        Level level = levelData.levels[levelNumber];      

        mainCamera.backgroundColor = levelData.levels[levelNumber].BgColor;
        ballRenderer.material.color = levelData.levels[levelNumber].BallColor;
        helixRenderer.material.color = levelData.levels[levelNumber].PillarColor;
        transform.localEulerAngles = initRotation;
        
        //create the new levels
        float distBwRings = pillarHeight / (level.rings.Count + 1);           // how far apart each ring should be
        float initSpawnPosY = startRingTransform.localPosition.y;            // for pos of level, initially set to top platform and then will subtract from it
        
        Ring startRing = Instantiate(helixRingPrefab, transform); 
        startRing.transform.localPosition = new Vector3(0, initSpawnPosY, 0);
        spawnedRings.Add(startRing.gameObject);
        startRing.SetupAsStartRing(levelData.levels[levelNumber].RingColor);

        for (int i = 0; i < level.rings.Count; i++)
        {
            initSpawnPosY -= distBwRings;         //subtracting distance bw two rings to get the pos for next ring

            Ring ring = Instantiate(helixRingPrefab, transform); 
            ring.transform.localPosition = new Vector3(0, initSpawnPosY, 0);
            spawnedRings.Add(ring.gameObject);
            
            ring.SetupRing(level.rings[i], levelData.levels[levelNumber].RingColor, levelData.levels[levelNumber].DeathSectionColor);
        }
        
        Ring endRing = Instantiate(helixRingPrefab, transform); 
        endRing.transform.localPosition = new Vector3(0, endRingTransform.localPosition.y, 0);
        spawnedRings.Add(endRing.gameObject);
        endRing.SetupAsEndRing(levelData.levels[levelNumber].RingColor);
    }
}