using UnityEngine;
using System.IO; // 파일 저장을 위해 필요

public class TransparentCapture : MonoBehaviour
{
    public int width = 256;  // 이미지 가로 크기
    public int height = 256; // 이미지 세로 크기

    // 컴포넌트 우클릭 메뉴에 버튼을 만드는 기능입니다.
    [ContextMenu("Capture Image")]
    public void Capture()
    {
        // 1. 카메라 가져오기
        Camera cam = GetComponent<Camera>();

        // 2. 임시 렌더 텍스처(필름) 만들기
        RenderTexture rt = new RenderTexture(width, height, 24);
        cam.targetTexture = rt; // 카메라가 이 필름에 그림을 그리게 설정

        // 3. 텍스처(사진) 생성
        Texture2D screenShot = new Texture2D(width, height, TextureFormat.ARGB32, false);

        // 4. 촬영
        cam.Render();

        // 5. 픽셀 정보 읽어오기
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenShot.Apply();

        // 6. 카메라 원상복구
        cam.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt); // 임시 필름 삭제

        // 7. PNG로 변환 후 저장
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = "Screenshot_" + System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png";
        string path = Application.dataPath + "/Resources/Image/ES/" + filename; // Assets 폴더에 저장

        File.WriteAllBytes(path, bytes);
        Debug.Log("저장 완료: " + path);
    }
}