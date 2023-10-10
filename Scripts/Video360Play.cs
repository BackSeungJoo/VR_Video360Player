using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video360Play : MonoBehaviour
{
    // 비디오 플레이어 컴포넌트
    VideoPlayer videoPlayer;

    // 재생해야할 VR 360 영상을 위한 설정
    public VideoClip[] vcList;
    int currentVCidx;

    private void Start()
    {
        // 비디오 플레이어 컴포넌트의 정보를 받아온다.
        videoPlayer = GetComponent<VideoPlayer>();

        currentVCidx = 0;
        videoPlayer.clip = vcList[currentVCidx];
        videoPlayer.Stop();
    }

    private void Update()
    {
        // 컴퓨터에서 영상을 변경하기 위한 기능
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            SwapVideoClip(false);
            // LEGACY :
            //videoPlayer.clip = vcList[0];
        }
        else if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            SwapVideoClip(true);
            // LEGACY :
            //videoPlayer.clip = vcList[1];
        }
    }

    /**
     * 인터랙션을 위해 함수를 퍼블릭으로 선언한다
     * @brief 배열의 인덱스 번호를 기준으로 영상을 교체, 재생하기 위한 함수
     * @param isNext가 true이면 다음 영상, false면 이전 영상 재생
     */
    public void SwapVideoClip(bool isNext)
    {
        /**
         * 현재 재생중인 영상의 넘버를 기준으로 체크한다.
         * 이전 영상 번호는 현재 영상보다 배열에서 인덱스 번호가 1이 작다.
         * 다음 영상 번호는 현재 영상보다 배열에서 인덱스 번호가 1이 크다.
         */
        int setVCnum = currentVCidx;
        videoPlayer.Stop();
        
        // 재생할 영상을 고르기 위한 과정
        if(isNext)
        {
            //// 배열의 다음 영상을 재생한다.
            // 리스트 전체 길이보다 크면 클립을 리스트의 첫 번째 영상으로 지정한다.
            setVCnum = (setVCnum + 1) % vcList.Length;

            // LEGACY:
            //setVCnum++;
            //if(vcList.Length <= setVCnum)
            //{
            //    // 리스트 전체 길이보다 크면 리스트의 클립을 첫 번째 영상으로 지정한다.
            //    videoPlayer.clip = vcList[0];
            //}
            //else
            //{
            //    // 리스트 길이보다 작으면 해당 번호의 영상을 실행한다.
            //    videoPlayer.clip = vcList[setVCnum];
            //}
        }
        else
        {
            // 배열의 이전 영상을 재생한다.
            setVCnum = ((setVCnum-1) + vcList.Length) % vcList.Length;

        }

        videoPlayer.clip = vcList[setVCnum];
        videoPlayer.Play();
        currentVCidx = setVCnum;
    }       // SwapVideoClip()

    public void SetVideoPlay(int num)
    {
        // 현재 재생 중인 번호가 전달받은 번호와 다를 때만 실행한다.
        if(currentVCidx != num)
        {
            videoPlayer.Stop();
            videoPlayer.clip = vcList[num];
            currentVCidx = num;
            videoPlayer.Play();
        }
    }
}
