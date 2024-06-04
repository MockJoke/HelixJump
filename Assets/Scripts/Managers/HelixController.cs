using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

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

    [Header("Debug Fields")] 
    [SerializeField] private int currColorPaletteIndex = -1;
    
    private readonly List<GameObject> spawnedRings = new List<GameObject>();

    private int currLevel = -1;
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
        pillarHeight = startRingTransform.localPosition.y - (endRingTransform.localPosition.y + .1f);

        // LoadLevel(0);
    }

    void Update()
    {
        // Spin helix by using click (or touch) & drag
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

        // Set newTap once the input is released
        if (Input.GetMouseButtonUp(0))
        {
            newTap = true;
            // lastTapPos = Vector3.zero;
        }
    }

    public void LoadLevel(int levelNumber)
    {
        // Remove rings from prev level
        foreach (GameObject GameObj in spawnedRings)
        {
            Destroy(GameObj);
        }

        spawnedRings.Clear();
        
        Level level = levelData.levels[levelNumber];

        if (currLevel == -1)
        {
            currLevel = levelNumber;
        }

        if (currColorPaletteIndex == -1 || currLevel != levelNumber)
        {
            int colorPaletteIdx = GetRandomColorPaletteIndex();
            while (colorPaletteIdx == currColorPaletteIndex)
            {
                colorPaletteIdx = GetRandomColorPaletteIndex();
            }

            currColorPaletteIndex = colorPaletteIdx;
            currLevel = levelNumber;
        }
        
        // In case an out-of-bound index is given from the in-editor debug field
        currColorPaletteIndex = Mathf.Clamp(currColorPaletteIndex, 0, levelData.ColorPalettes.Count - 1);
        
        ColorPalette currColorPalette = levelData.ColorPalettes[currColorPaletteIndex];

        mainCamera.backgroundColor = currColorPalette.BgColor;
        vignette.color.Override(currColorPalette.BgVignetteColor);
        helixRenderer.material.color = currColorPalette.PillarColor;
        ballRenderer.material.color = currColorPalette.BallColor;
        ballTrailRenderer.materials[0].color = currColorPalette.BallColor;
        splashPrefab.GetComponent<Renderer>().sharedMaterial.color = currColorPalette.BallColor;
        transform.localEulerAngles = initRotation;

        //create the new levels
        float distBwRings = pillarHeight / (level.rings.Count + 1); // how far apart each ring should be

        // for vertical-pos of level, initially set it to top platform and then gradually subtract from it
        float initSpawnPosY = startRingTransform.localPosition.y;

        // Start Ring 
        Ring startRing = Instantiate(helixRingPrefab, transform);
        startRing.transform.localPosition = new Vector3(0, initSpawnPosY, 0);
        spawnedRings.Add(startRing.gameObject);
        startRing.SetupAsStartRing(currColorPalette.NormalSectionColor);

        // Middle Rings
        for (int i = 0; i < level.rings.Count; i++)
        {
            initSpawnPosY -= distBwRings; //subtracting distance bw two rings to get the pos for next ring

            Ring ring = Instantiate(helixRingPrefab, transform);
            ring.transform.localPosition = new Vector3(0, initSpawnPosY, 0);
            spawnedRings.Add(ring.gameObject);

            ring.SetupRing(level.rings[i], currColorPalette.NormalSectionColor, currColorPalette.DangerSectionColor);
        }

        // End Ring
        Ring endRing = Instantiate(helixRingPrefab, transform);
        endRing.transform.localPosition = new Vector3(0, endRingTransform.localPosition.y, 0);
        spawnedRings.Add(endRing.gameObject);
        endRing.SetupAsEndRing(currColorPalette.GoalSectionColor);
    }

    private int GetRandomColorPaletteIndex()
    {
        return Random.Range(0, levelData.ColorPalettes.Count);
    }
    
    // Need to reload the level to test different color palettes during runtime by changing the value of currColorPaletteIndex in-editor
    [ContextMenu("Set Color Palette")]
    private void SetColorPalette()
    {
        LoadLevel(currLevel);
    }
}