using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoFrame : MonoBehaviour
{
    VideoPlayer videoPlayer = default;

    private void Start()
    {
        // 현재 오브젝트의 비디오 플레이어 컴포넌트를 가지고 온다.
        videoPlayer = GetComponent<VideoPlayer>();
        // 자동 재생 되는 것을 막는다.
        videoPlayer.Stop();
    }

    private void Update()
    {
        // S를 누르면 정지한다.
        if (Input.GetKeyDown(KeyCode.S))
        {
            videoPlayer.Stop();
        }

        // 스페이스 바를 눌렀을 때 재생 또는 일시정지한다.
        if (Input.GetKeyDown("space"))
        {
            // 현재 비디오 플레이어가 플레이 상태인지 확인한다.
            if(videoPlayer.isPlaying)
            {
                // 플레이(재생) 중이라면 일시 정지한다.
                videoPlayer.Pause();
            }
            else
            {
                // 그렇지 않다면(일시정지 중 또는 멈춤) 플레이(재생)한다.
                videoPlayer.Play();
            }
        }
    }       //  Update()

    //! GazePointer에서 영상 재생을 컨트롤하기 위한 함수
    public void CheckVideoFrame(bool checker)
    {
        if(checker)
        {
            if (videoPlayer.isPlaying == false)
            {
                videoPlayer.Play();
            }
        }
        else
        {
            videoPlayer.Stop();
        }
    }       // CheckVideoFrame()
}

