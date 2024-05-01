using UnityEngine;
using System.Collections;
#if UNITY_ANDROID
using UnityEngine.Android;
#elif UNITY_IOS || UNITY_IPHONE
using UnityEngine.iOS;
#endif
public class MobileCam : MonoBehaviour
{
    WebCamTexture camTexture;
    public Camera cam;

    public GameObject quad;

    void Start()
    {
        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

#if UNITY_IOS || UNITY_IPHONE
        quad.transform.localEulerAngles = (new Vector3(0, 0, 0)); 
#elif UNITY_ANDROID
        quad.transform.localEulerAngles = (new Vector3(0, 0, -90));
#endif
        quad.transform.localScale = new Vector3(worldScreenHeight + 0.1f, worldScreenWidth + 0.1f, 1);
        //StartCoroutine(CallCamera());
    }
    private void Update()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
#elif UNITY_IOS || UNITY_IPHONE
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Application.RequestUserAuthorization(UserAuthorization.WebCam);
        }
#endif
    }

    IEnumerator CallCamera()
    {
#if UNITY_ANDROID
        yield return Permission.HasUserAuthorizedPermission(Permission.Camera);
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            StartCamera();
        }
#elif UNITY_IOS || UNITY_IPHONE
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            StartCamera();
        }
#endif
    }

    void StartCamera()
    {
        if (camTexture != null)
            camTexture.Stop();

        WebCamDevice[] cameraDevices = WebCamTexture.devices;
        string deviceName = cameraDevices[0].name;

        camTexture = new WebCamTexture(deviceName, Screen.height, Screen.width, 60);
        quad.GetComponent<Renderer>().material.mainTexture = camTexture;
        camTexture.Play();
    }
    public void CallCam()
    {
        StartCoroutine(CallCamera());
    }
    private void OnDisable()
    {
        if (camTexture != null)
        {
            camTexture.Stop();
            Debug.Log("Stop");
        }
    }
}