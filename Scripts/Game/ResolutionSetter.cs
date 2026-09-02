using UnityEngine;

public class ResolutionSetter : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject); // 씬 이동해도 계속 유지
        SetResolution();
    }

    private void SetResolution()
    {
        int targetWidth = 1920;
        int targetHeight = 1080;

        // 현재 모니터가 목표 해상도보다 크거나 같으면 그대로 설정
        if (Screen.currentResolution.width >= targetWidth && Screen.currentResolution.height >= targetHeight)
        {
            Screen.SetResolution(targetWidth, targetHeight, FullScreenMode.FullScreenWindow);
        }
        else
        {
            // 해상도가 부족하면 현재 해상도 그대로 전체화면 설정
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
        }
    }
}