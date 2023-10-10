using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video360Play : MonoBehaviour
{
    // ���� �÷��̾� ������Ʈ
    VideoPlayer videoPlayer;

    // ����ؾ��� VR 360 ������ ���� ����
    public VideoClip[] vcList;
    int currentVCidx;

    private void Start()
    {
        // ���� �÷��̾� ������Ʈ�� ������ �޾ƿ´�.
        videoPlayer = GetComponent<VideoPlayer>();

        currentVCidx = 0;
        videoPlayer.clip = vcList[currentVCidx];
        videoPlayer.Stop();
    }

    private void Update()
    {
        // ��ǻ�Ϳ��� ������ �����ϱ� ���� ���
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
     * ���ͷ����� ���� �Լ��� �ۺ����� �����Ѵ�
     * @brief �迭�� �ε��� ��ȣ�� �������� ������ ��ü, ����ϱ� ���� �Լ�
     * @param isNext�� true�̸� ���� ����, false�� ���� ���� ���
     */
    public void SwapVideoClip(bool isNext)
    {
        /**
         * ���� ������� ������ �ѹ��� �������� üũ�Ѵ�.
         * ���� ���� ��ȣ�� ���� ���󺸴� �迭���� �ε��� ��ȣ�� 1�� �۴�.
         * ���� ���� ��ȣ�� ���� ���󺸴� �迭���� �ε��� ��ȣ�� 1�� ũ��.
         */
        int setVCnum = currentVCidx;
        videoPlayer.Stop();
        
        // ����� ������ ���� ���� ����
        if(isNext)
        {
            //// �迭�� ���� ������ ����Ѵ�.
            // ����Ʈ ��ü ���̺��� ũ�� Ŭ���� ����Ʈ�� ù ��° �������� �����Ѵ�.
            setVCnum = (setVCnum + 1) % vcList.Length;

            // LEGACY:
            //setVCnum++;
            //if(vcList.Length <= setVCnum)
            //{
            //    // ����Ʈ ��ü ���̺��� ũ�� ����Ʈ�� Ŭ���� ù ��° �������� �����Ѵ�.
            //    videoPlayer.clip = vcList[0];
            //}
            //else
            //{
            //    // ����Ʈ ���̺��� ������ �ش� ��ȣ�� ������ �����Ѵ�.
            //    videoPlayer.clip = vcList[setVCnum];
            //}
        }
        else
        {
            // �迭�� ���� ������ ����Ѵ�.
            setVCnum = ((setVCnum-1) + vcList.Length) % vcList.Length;

        }

        videoPlayer.clip = vcList[setVCnum];
        videoPlayer.Play();
        currentVCidx = setVCnum;
    }       // SwapVideoClip()

    public void SetVideoPlay(int num)
    {
        // ���� ��� ���� ��ȣ�� ���޹��� ��ȣ�� �ٸ� ���� �����Ѵ�.
        if(currentVCidx != num)
        {
            videoPlayer.Stop();
            videoPlayer.clip = vcList[num];
            currentVCidx = num;
            videoPlayer.Play();
        }
    }
}
