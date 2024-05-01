using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using static AppConstants;
public class UIManager : MonoBehaviour
{
    [Serializable]
    private class Guide
    {
        public Sprite guideSprite;
        public string guideLabel;
        public string guideButtonLabel;
        public Image sliderCircle;
    }

    [Header("Guide")]
    [SerializeField] private Guide[] guides;
    [SerializeField] private Image guideImage;
    [SerializeField] private TextMeshProUGUI guideLabel;
    [SerializeField] private TextMeshProUGUI guideButtonLabel;

    [Header("UI GameObjects")]
    [SerializeField] private GameObject imageObject;
    [SerializeField] private GameObject sliderObject;
    [SerializeField] private GameObject skipObject;
    [SerializeField] private GameObject collectObject;
    [SerializeField] private GameObject backObject;
    [SerializeField] private GameObject poemTextObject;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private TextMeshProUGUI poemLabel;

    [Header("AR GameObjects")]
    [SerializeField] private GameObject arPanel;
    [SerializeField] private GameObject bottomPoem;
    [SerializeField] private TextMeshProUGUI arPoemlabel;
    [SerializeField] private TextMeshProUGUI arTimeLabel;
    [SerializeField] private GameObject downArrow;
    [SerializeField] private GameObject upArrow;

    [Header("Win Lose GameObjects")]
    [SerializeField] private GameObject winLosePanel;
    [SerializeField] private Image winLoseImage;
    [SerializeField] private Sprite winSprite;
    [SerializeField] private Sprite loseSprite;
    [SerializeField] private TextMeshProUGUI winLoseLabel;
    [SerializeField] private TextMeshProUGUI statLabel;
    [SerializeField] private TextMeshProUGUI winLosePoemLabel;

    public Color defaultColor;
    public Color selectedColor;
    private int guideIndex = 0;

    public static UIManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // Set the instance to this object if it doesn't exist yet
            Instance = this;
        }
    }

    private void Start()
    {
        HidePanel();
        uiPanel.SetActive(true);
        guideIndex = PlayerPrefs.GetInt(PlayerPrefConstants.GUIDEINDEX);
        UpdateGuide();
    }

    private void UpdateGuide()
    {
        HideUIObject();
        imageObject.SetActive(true);
        sliderObject.SetActive(true);
        skipObject.SetActive(guideIndex > 0 && guideIndex != guides.Length - 1);
        backObject.SetActive(guideIndex > 0);
        foreach (var guide in guides)
        {
            guide.sliderCircle.color = defaultColor;
        }
        if (guideIndex == -1)
        {
            UpdateGuide(0);
            sliderObject.SetActive(false);
            guideButtonLabel.text = guides[^1].guideButtonLabel;
        }
        else
        {
            UpdateGuide(guideIndex);
        }
    }

    private void UpdateGuide(int idex)
    {
        guideImage.sprite = guides[idex].guideSprite;
        guideLabel.text = ConvertToNewLine(guides[idex].guideLabel);
        guideButtonLabel.text = guides[idex].guideButtonLabel;
        guides[idex].sliderCircle.color = selectedColor;
    }

    private void HideUIObject()
    {
        imageObject.SetActive(false);
        sliderObject.SetActive(false);
        skipObject.SetActive(false);
        poemTextObject.SetActive(false);
        collectObject.SetActive(false);
    }
    private void HidePanel()
    {
        uiPanel.SetActive(false);
        arPanel.SetActive(false);
        winLosePanel.SetActive(false);
    }
    public void NextGuide()
    {
        if (guideIndex == guides.Length - 1 || guideIndex == -1)
        {
            PlayerPrefs.SetInt(PlayerPrefConstants.GUIDEINDEX, -1);
            if (!poemTextObject.activeInHierarchy)
            {
                HideUIObject();
                guideLabel.text = UIConstants.POEMBYAI;
                guideButtonLabel.text = UIConstants.FINDINYOURSPACE;
                backObject.SetActive(true);
                collectObject.SetActive(true);
                poemTextObject.SetActive(true);
                StartCoroutine(GeminiAPIManager.Instance.SendPoemGenrateRequest());
            }
            else if (!collectObject.activeInHierarchy)
            {
                collectObject.SetActive(true);
                poemTextObject.SetActive(true);
                StartCoroutine(GeminiAPIManager.Instance.SendPoemGenrateRequest());
            }
            else
            {
                HidePanel();
                arPanel.SetActive(true);
                PoemManager.Instance.StartGame();
            }
        }
        else if (guideIndex < guides.Length - 1)
        {
            guideIndex++;
            UpdateGuide();
        }
    }
    public void OnBack()
    {
        if (uiPanel.activeInHierarchy)
        {
            if (guideIndex > 0)
            {
                guideIndex--;
                UpdateGuide();
            }
            else
            {
                UpdateGuide();
            }
        }
        else if (arPanel.activeInHierarchy)
        {
            HidePanel();
            uiPanel.SetActive(true);
            PoemManager.Instance.OnStopGame();
        }
    }
    public void OnSkip()
    {
        guideIndex = guides.Length - 1;
        UpdateGuide();
    }

    public void UpdateUIPoem(string poem)
    {
        poemLabel.text = poem;
        if (poem == NetworkConstants.FAILTOLOAD)
        {
            guideButtonLabel.text = UIConstants.RETRY;
            collectObject.SetActive(false);
        }
    }
    public void UpdateARPoem(string poem)
    {
        arPoemlabel.text = poem;
    }
    public void UpdateWinLosePoem(string poem, string status)
    {
        statLabel.text = status;
        winLosePoemLabel.text = poem;
    }
    public void UpdateTime(int time)
    {
        arTimeLabel.text = time.ToString();
    }
    public void OnToggleARpoem()
    {
        var value = !bottomPoem.activeInHierarchy;
        bottomPoem.SetActive(value);
        downArrow.SetActive(value);
        upArrow.SetActive(!value);
    }

    public void OnWinLose(bool isWin)
    {
        HidePanel();
        winLosePanel.SetActive(true);
        if (isWin)
        {
            winLoseLabel.text = UIConstants.HURRAY;
            winLoseImage.sprite = winSprite;
        }
        else
        {
            winLoseLabel.text = UIConstants.OOPS;
            winLoseImage.sprite = loseSprite;
        }
    }

    public void OnRestart()
    {
        guideIndex = -1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private static string ConvertToNewLine(string input)
    {
        // Replace "\n" with Environment.NewLine
        string output = input.Replace("\\n", Environment.NewLine);
        return output;
    }
}
