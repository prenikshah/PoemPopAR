using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

[Serializable]
public class Candidate
{
    public Content content;
    public string finishReason;
    public int index;
    public List<SafetyRating> safetyRatings = new List<SafetyRating>();
}
[Serializable]
public class Content
{
    public List<Part> parts = new List<Part>();
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
    public List<SafetyRating> safetyRatings = new List<SafetyRating>();
}
[Serializable]
public class PromptResponse
{
    public List<Candidate> candidates = new List<Candidate>();
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

    public static GeminiAPIManager instance;

    private void Awake()
    {
        instance = this;
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
                Debug.LogError(www.error);
            }
            else
            {
                var response = JsonUtility.FromJson<PromptResponse>(www.downloadHandler.text);
                Debug.Log($"resposne : {response}");
                var result = response.candidates[0].content.parts[0].text;
                if (OnWordsGenerated != null)
                    OnWordsGenerated.Invoke(result);
            }
        }
    }
}
