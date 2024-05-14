using System.Collections.Generic;
using UnityEngine;

public class HelixController : MonoBehaviour 
{
    private Vector3 startRotation;
    private Vector2 lastTapPos;
    private float helixDistance;        
    
    [SerializeField] private Transform startPlatform;
    [SerializeField] private Transform endPlatform;
    [SerializeField] private GameObject helixLevelPrefab;

    public List<Stage> allStages = new List<Stage>();       //all stages    
    private List<GameObject> spawnedLevels = new List<GameObject>();        //all levels, every single level is a GameObject itself

    [Header("Components")] 
    [SerializeField] private Renderer helixRenderer;
    [SerializeField] private Camera mainCamera;

    [Space] 
    [SerializeField] private Renderer ballRenderer;

    void Awake() 
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (helixRenderer == null)
            helixRenderer = GetComponent<Renderer>();

        if (ballRenderer == null)
            ballRenderer = FindObjectOfType<BallController>().GetComponent<Renderer>();
            
        startRotation = transform.localEulerAngles;
        helixDistance = startPlatform.localPosition.y - (endPlatform.localPosition.y + .1f);         //distance bw top and goal platform     
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

    public void LoadStage(int stageNumber)
    {
        //get the correct stage
        Stage stage = allStages[Mathf.Clamp(stageNumber, 0, allStages.Count - 1)];      

        if(stage == null)
        {
            Debug.LogError("No stage " + stageNumber + " found in allStages list (HelixController). All stages assigned in list?");
            return;
        }

        //set the new background color in new stage
        mainCamera.backgroundColor = allStages[stageNumber].stageBackgroundColor;

        //set the ball color in new stage
        ballRenderer.material.color = allStages[stageNumber].stageBallColor;

        //set the helix color in new stage
        helixRenderer.material.color = allStages[stageNumber].HelixColor;

        //reset the helix rotation
        transform.localEulerAngles = startRotation;

        //destroy the old levels if there are some
        foreach(GameObject GameObj in spawnedLevels)
        {
            Destroy(GameObj);
        }

        //create the new levels
        float levelDistance = helixDistance / stage.levels.Count;       //how far apart each platform should be
        float spawnPosY = startPlatform.localPosition.y;                //for pos of level, initially set to top platform and then will substract from it

        for(int i = 0; i < stage.levels.Count; i++)
        {
            spawnPosY -= levelDistance;         //subtracting distance bw two levels to get the pos for next level

            GameObject level = Instantiate(helixLevelPrefab, transform);        //instantiating a level 
            Debug.Log("Levels Spawned");
            level.transform.localPosition = new Vector3(0, spawnPosY, 0);
            spawnedLevels.Add(level);           //adding level to the list

            //disable some parts (depending on level setup)
            int partsToDisable = 12 - stage.levels[i].partCount;            //12 -> one stage consists of 12 parts 
            List<GameObject> disabledParts = new List<GameObject>();        //list of disabled parts

            Debug.Log("Should disable " + partsToDisable);

            while(disabledParts.Count < partsToDisable)            
            {
                GameObject randomPart = level.transform.GetChild(Random.Range(0, level.transform.childCount)).gameObject;           //to randomise the disabled parts
                
                if (!disabledParts.Contains(randomPart))
                {
                    randomPart.SetActive(false);
                    disabledParts.Add(randomPart);
                    Debug.Log("Disabled Part");
                }
            }

            //mark parts as death parts
            List<GameObject> leftParts = new List<GameObject>();            //list of death parts

            foreach(Transform t in level.transform)                //go through position of evry single level
            {
                t.GetComponent<Renderer>().material.color = allStages[stageNumber].stageLevelPartColor;             //set color of part

                if(t.gameObject.activeInHierarchy)         
                    leftParts.Add(t.gameObject);            //active parts are added in left parts list
            }

            Debug.Log(leftParts.Count + " parts left");

            //creating death parts
            List<GameObject> deathParts = new List<GameObject>();

            Debug.Log("Should mark " + stage.levels[i].deathPartCount + " death parts");

            while (deathParts.Count < stage.levels[i].deathPartCount)
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
