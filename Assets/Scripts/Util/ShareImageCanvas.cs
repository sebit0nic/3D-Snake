using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class ShareImageCanvas {

    public string screenshotName = "screenshot.png";

    public void ShareScreenshotWithText() {
        string screenShotPath = Application.persistentDataPath + "/" + screenshotName;
        ScreenCapture.CaptureScreenshot(screenshotName);
        //TODO: write correct text
        Share("My highscore is 69!", screenShotPath, "");
    }

    public void Share(string shareText, string imagePath, string url, string subject = "") {
        if (Application.platform == RuntimePlatform.Android) {
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + imagePath);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("setType", "image/png");

            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareText);

            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, subject);
            currentActivity.Call("startActivity", jChooser);
        } else {
            Debug.Log("No sharing set up for this platform.");
        }
    }
}
