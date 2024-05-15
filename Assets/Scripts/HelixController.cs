using System.Collections.Generic;
using UnityEngine;

public class HelixController : MonoBehaviour 
{
    [SerializeField] private Transform startPlatform;
    [SerializeField] private Transform endPlatform;
    [SerializeField] private GameObject helixRingPrefab;

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
    
    void Awake() 
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (helixRenderer == null)
            helixRenderer = GetComponent<Renderer>();

        if (ballRenderer == null)
            ballRenderer = FindObjectOfType<BallController>().GetComponent<Renderer>();
            
        initRotation = transform.localEulerAngles;
        pillarHeight = startPlatform.localPosition.y - (endPlatform.localPosition.y + .1f);         //distance bw top and goal platform
        
        LoadStage(0);
    }
	
	void Update() 
    {
        //spin helix by using click (or touch) and drag
        if(Input.GetMouseButton(0))
        {
            Vector2 curTapPos = Input.mousePosition;

            if (lastTapPos == Vector2.zero)
                lastTapPos = curTapPos;

            float delta = lastTapPos.x - curTapPos.x;
            lastTapPos = curTapPos;

            transform.Rotate(Vector3.up * delta);       //how much to rotate 
        }

        if(Input.GetMouseButtonUp(0))
        {
            lastTapPos = Vector2.zero;
        }
    }

    public void LoadStage(int levelNumber)
    {
        levelNumber = Mathf.Clamp(levelNumber, 0, levelData.levels.Count - 1);
        
        Level level = levelData.levels[levelNumber];      

        mainCamera.backgroundColor = levelData.levels[levelNumber].BgColor;
        ballRenderer.material.color = levelData.levels[levelNumber].BallColor;
        helixRenderer.material.color = levelData.levels[levelNumber].PillarColor;
        transform.localEulerAngles = initRotation;

        //destroy the old levels if there are some
        foreach(GameObject GameObj in spawnedRings)
        {
            Destroy(GameObj);
        }

        //create the new levels
        float levelDistance = pillarHeight / level.rings.Count;       //how far apart each platform should be
        float spawnPosY = startPlatform.localPosition.y;                //for pos of level, initially set to top platform and then will subtract from it

        for(int i = 0; i < level.rings.Count; i++)
        {
            spawnPosY -= levelDistance;         //subtracting distance bw two levels to get the pos for next level

            GameObject ring = Instantiate(helixRingPrefab, transform);        //instantiating a level 
            Debug.Log("Levels Spawned");
            ring.transform.localPosition = new Vector3(0, spawnPosY, 0);
            spawnedRings.Add(ring);           //adding level to the list

            //disable some parts (depending on level setup)
            int partsToDisable = 12 - level.rings[i].totalSections;            //12 -> one stage consists of 12 parts 
            List<GameObject> disabledParts = new List<GameObject>();        //list of disabled parts

            Debug.Log("Should disable " + partsToDisable);

            while(disabledParts.Count < partsToDisable)            
            {
                GameObject randomPart = ring.transform.GetChild(Random.Range(0, ring.transform.childCount)).gameObject;           //to randomise the disabled parts
                
                if (!disabledParts.Contains(randomPart))
                {
                    randomPart.SetActive(false);
                    disabledParts.Add(randomPart);
                    Debug.Log("Disabled Part");
                }
            }

            //mark parts as death parts
            List<GameObject> leftParts = new List<GameObject>();            //list of death parts

            foreach(Transform t in ring.transform)                //go through position of every single level
            {
                t.GetComponent<Renderer>().material.color = levelData.levels[levelNumber].RingColor;             //set color of part

                if(t.gameObject.activeInHierarchy)         
                    leftParts.Add(t.gameObject);            //active parts are added in left parts list
            }

            Debug.Log(leftParts.Count + " parts left");

            //creating death parts
            List<GameObject> deathParts = new List<GameObject>();

            Debug.Log("Should mark " + level.rings[i].deathSections + " death parts");

            while (deathParts.Count < level.rings[i].deathSections)
            {
                GameObject randomPart = leftParts[(Random.Range(0, leftParts.Count))];          //to randomise the left parts

                if (!deathParts.Contains(randomPart))
                {
                    randomPart.gameObject.AddComponent<DeathPart>();        //adding DeathPart script to random part
                    deathParts.Add(randomPart);
                    Debug.Log("Set death part");
                }
            }
        }
    }
}
