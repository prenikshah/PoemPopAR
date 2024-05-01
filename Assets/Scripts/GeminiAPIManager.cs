using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using TMPro;
using static AppConstants;
[Serializable]
public class Candidate
{
    public Content content;
    public string finishReason;
    public int index;
    public List<SafetyRating> safetyRatings = new();
}
[Serializable]
public class Content
{
    public List<Part> parts = new();
    public string role;
}
[Serializable]
public class Part
{
    public string text;
}
[Serializable]
public class PromptFeedback
{
    public List<SafetyRating> safetyRatings = new();
}
[Serializable]
public class PromptResponse
{
    public List<Candidate> candidates = new();
    public PromptFeedback promptFeedback;
}
[Serializable]
public class SafetyRating
{
    public string category;
    public string probability;
}


public class GeminiAPIManager : MonoBehaviour
{
    // Gemini API key
    const string API_KEY = "";

    public Action<string> OnWordsGenerated;
    public Action<string> OnPoemGenerated;

    public GameObject loadingPanel;
    public TextMeshProUGUI loadingLabel;
    public static GeminiAPIManager Instance;

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

    IEnumerator SendWordGenerationRequest(string poem)
    {
        string prompt = $"Generate random 8 words from this poem without any numbering or bullet formatting, not adjectives\n{poem}";
        string data = $"{{\"contents\": [{{\"parts\":[{{\"text\": \"{prompt}\"}}]}}]}}";
        string text_only_api = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={API_KEY}";
        using (UnityWebRequest www = UnityWebRequest.Post(text_only_api, data, "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                OnPoemGenerated?.Invoke(NetworkConstants.FAILTOLOAD);
                //Debug.LogError(www.error);
            }
            else
            {
                var response = JsonUtility.FromJson<PromptResponse>(www.downloadHandler.text);
                //Debug.Log($"resposne : {response}");
                var result = response.candidates[0].content.parts[0].text;
                OnWordsGenerated?.Invoke(result);
            }
        }
    }

    public IEnumerator SendPoemGenrateRequest()
    {
        string poemWord1 = NetworkConstants.POEMWORDS[UnityEngine.Random.Range(0, NetworkConstants.POEMWORDS.Count)];
        NetworkConstants.POEMWORDS.Remove(poemWord1);
        string poemWord2 = NetworkConstants.POEMWORDS[UnityEngine.Random.Range(0, NetworkConstants.POEMWORDS.Count)];
        NetworkConstants.POEMWORDS.Add(poemWord1);
        string prompt = $"Generate 6 line Poem about {poemWord1} and {poemWord2}";
        //Debug.Log($"Prompt is {prompt}");
        string data = $"{{\"contents\": [{{\"parts\":[{{\"text\": \"{prompt}\"}}]}}]}}";
        string text_only_api = $"{NetworkConstants.TEXTAPI}{API_KEY}";
        string loadingText = NetworkConstants.LOADINGWORDS[UnityEngine.Random.Range(0, NetworkConstants.LOADINGWORDS.Length)];
        loadingLabel.text = loadingText;
        loadingPanel.SetActive(true);
        using (UnityWebRequest www = UnityWebRequest.Post(text_only_api, data, "application/json"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                OnPoemGenerated?.Invoke(NetworkConstants.FAILTOLOAD);
                //Debug.LogError(www.error);
                loadingPanel.SetActive(false);
            }
            else
            {
                var response = JsonUtility.FromJson<PromptResponse>(www.downloadHandler.text);
                //Debug.Log($"resposne result: {response.candidates[0].content.parts[0].text}");
                var result = response.candidates[0].content.parts[0].text;
                string modifiedString = result.Replace("\n\n", "\n");
                OnPoemGenerated?.Invoke(modifiedString);
                yield return StartCoroutine(SendWordGenerationRequest(modifiedString));
                loadingPanel.SetActive(false);
            }
        }
    }
}
