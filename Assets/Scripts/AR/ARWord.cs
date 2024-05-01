using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ARWord : MonoBehaviour
{
    public GameObject offsetObject;
    public Button wordButton;
    public Canvas arCanvas;
    public TextMeshProUGUI arLabel;
    public int id;

    public void InitARWord(int _id, string label, Camera mainCam, Action<int> invokeEvent)
    {
        id = _id;
        offsetObject.transform.localPosition = new Vector3(0, UnityEngine.Random.Range(-0.25f, 1.05f), UnityEngine.Random.Range(0.75f, 1.5f));
        arLabel.text = label;
        arCanvas.worldCamera = mainCam;
        wordButton.onClick.RemoveAllListeners();
        wordButton.onClick.AddListener(delegate
        {
            invokeEvent.Invoke(id);
        });
        this.gameObject.transform.localEulerAngles = new Vector3(0, UnityEngine.Random.Range(0.0f, 360.0f), 0);
    }
}
