using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoFrame : MonoBehaviour
{
    VideoPlayer videoPlayer = default;

    private void Start()
    {
        // ���� ������Ʈ�� ���� �÷��̾� ������Ʈ�� ������ �´�.
        videoPlayer = GetComponent<VideoPlayer>();
        // �ڵ� ��� �Ǵ� ���� ���´�.
        videoPlayer.Stop();
    }

    private void Update()
    {
        // S�� ������ �����Ѵ�.
        if (Input.GetKeyDown(KeyCode.S))
        {
            videoPlayer.Stop();
        }

        // �����̽� �ٸ� ������ �� ��� �Ǵ� �Ͻ������Ѵ�.
        if (Input.GetKeyDown("space"))
        {
            // ���� ���� �÷��̾ �÷��� �������� Ȯ���Ѵ�.
            if(videoPlayer.isPlaying)
            {
                // �÷���(���) ���̶�� �Ͻ� �����Ѵ�.
                videoPlayer.Pause();
            }
            else
            {
                // �׷��� �ʴٸ�(�Ͻ����� �� �Ǵ� ����) �÷���(���)�Ѵ�.
                videoPlayer.Play();
            }
        }
    }       //  Update()

    //! GazePointer���� ���� ����� ��Ʈ���ϱ� ���� �Լ�
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

