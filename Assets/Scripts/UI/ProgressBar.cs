using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI currLevel;
    [SerializeField] private TextMeshProUGUI nextLevel;
    [SerializeField] private LevelData levelData;
    
    private void Update()
    {
        currLevel.text = $"{PlayerPrefs.GetInt("CurrLevel", 1)}";
        nextLevel.text = $"{PlayerPrefs.GetInt("CurrLevel", 1) + 1}";

        int progress = GameManager.Instance.PassedRingCnt * 100 / (levelData.levels[PlayerPrefs.GetInt("CurrLevel", 1)].rings.Count + 1);
        progressSlider.value = progress;
    }
}
