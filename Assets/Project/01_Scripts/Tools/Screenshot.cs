using UnityEngine;

public class Screenshot : MonoBehaviour
{
    private static int count = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ScreenCapture.CaptureScreenshot("Assets/Screenshots/screenshot" + count + ".png");
            count++;
        }
    }
}
