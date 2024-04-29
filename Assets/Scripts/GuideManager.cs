using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GuideManager : MonoBehaviour
{
    [System.Serializable]
    private class Guide
    {
        public Sprite guideSprite;
        public string guideLabel;
        public string guideButtonLabel;
        public Image sliderCircle;
    }

    [SerializeField] private Guide[] guides;
    [SerializeField] private GameObject skipObject;
    [SerializeField] private GameObject backObject;
    [SerializeField] private Image guideImage;
    [SerializeField] private TextMeshProUGUI guideLabel;
    [SerializeField] private TextMeshProUGUI guideButtonLabel;

    public Color defaultColor;
    public Color selectedColor;
    private int guideIndex = 0;

    private void Start()
    {
        UpdateGuide();
    }

    private void UpdateGuide()
    {
        foreach (var guide in guides)
        {
            guide.sliderCircle.color = defaultColor;
        }
        guideImage.sprite = guides[guideIndex].guideSprite;
        guideLabel.text = ConvertToNewLine(guides[guideIndex].guideLabel);
        guideButtonLabel.text = guides[guideIndex].guideButtonLabel;
        guides[guideIndex].sliderCircle.color = selectedColor;
        skipObject.SetActive(guideIndex > 0 && guideIndex != guides.Length - 1);
        backObject.SetActive(guideIndex > 0);
    }

    public void NextGuide()
    {
        if (guideIndex < guides.Length - 1)
        {
            guideIndex++;
            UpdateGuide();
        }
    }
    public void OnBack()
    {
        if (guideIndex > 0)
        {
            guideIndex--;
            UpdateGuide();
        }
    }
    public void OnSkip()
    {
        guideIndex = guides.Length - 1;
        UpdateGuide();
    }

    private static string ConvertToNewLine(string input)
    {
        // Replace "\n" with Environment.NewLine
        string output = input.Replace("\\n", Environment.NewLine);
        return output;
    }

}
