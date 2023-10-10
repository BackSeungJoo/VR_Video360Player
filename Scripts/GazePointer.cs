using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GazePointer : MonoBehaviour
{
    public Video360Play video360 = default;
    public Transform uiCanvas;
    public Image gazeImg;

    Vector3 defaultScale = Vector3.one;
    public float uiScale = 1f;

    bool isHitObj;                      // ���ͷ����� �Ͼ�� ������Ʈ�� �ü��� ����� üũ
    GameObject prevHitObj;              // ���� �������� �ü��� �ӹ����� ������Ʈ ������ ��� ���� ����
    GameObject curHitObj;
    float curGazeTime = 0;
    public float gazeChagreTime = 3f;

    private void Start()
    {
        defaultScale = uiCanvas.localScale;
        curGazeTime = 0f;
    }

    private void Update()
    {
        // ĵ���� ������Ʈ�� �������� �Ÿ��� ���� �����Ѵ�.
        // 1. ī�޶� �������� ���� ������ ��ǥ�� ���Ѵ�
        Vector3 dir = transform.TransformPoint(Vector3.forward);

        // 2. ī�޶� �������� ������ ���̸� �����Ѵ�.
        Ray ray = new Ray(transform.position, dir);
        RaycastHit hitInfo;

        if(Physics.Raycast(ray, out hitInfo))
        {
            uiCanvas.localScale = defaultScale * uiScale * hitInfo.distance;
            uiCanvas.position = transform.forward * hitInfo.distance;
            if (hitInfo.transform.CompareTag("GazeObj"))
            {
                isHitObj = true;
            }
            curHitObj = hitInfo.transform.gameObject;
        }       // if: 3. ���̿� �ε��� ��쿡�� �Ÿ� ���� �̿��� UICanvas�� ũ�⸦ �����Ѵ�.

        else 
        {
            uiCanvas.localScale = defaultScale * uiScale;
            uiCanvas.position = transform.position + dir;
        }       // else: 4. �ƹ��͵� �ε����� ������ �⺻ ������ ��

        // 5. uiCanvas�� �׻� ī�޶� ������Ʈ�� �ٶ󺸰� �Ѵ�.
        uiCanvas.forward = transform.forward * -1;

        // GazeObj�� ���̿� ����� �� ����
        if (isHitObj)
        {
            if(curHitObj == prevHitObj)
            {
                // ���ͷ����� �߻��ؾ� �ϴ� ������Ʈ�� �ü��� ������ �ִٸ� �ð� ����
                curGazeTime += Time.deltaTime;
            }       // if: ���� �������� ������Ʈ�� ���� �������� ������Ʈ���� �ӹ��� �ִ� ���
            else
            {
                // ���� �������� ���� ������ ������Ʈ�Ѵ�.
                prevHitObj = curHitObj;
            }       // else: ���� �������� ������Ʈ�� ���� �������� ������Ʈ���� ��� ���

            // hit�� ������Ʈ�� VideoPlayer ������Ʈ�� ���� �ִ��� Ȯ���Ѵ�.
            HitObjChecker(curHitObj, true);
        }

        // �ü��� ����ų� GazeObj�� �ƴ϶�� �ð��� �ʱ�ȭ
        else
        {
            if (prevHitObj != null && prevHitObj != default)
            {
                HitObjChecker(prevHitObj, false);
                prevHitObj = default;
            }

            curGazeTime = 0f;
        }

        // �ü��� �ӹ� �ð��� 0�� �ִ� ���̷� �Ѵ�.
        curGazeTime = Mathf.Clamp(curGazeTime, 0, gazeChagreTime);
        // Ui Image �� fillAmount�� ������Ʈ�Ѵ�.
        gazeImg.fillAmount = curGazeTime / gazeChagreTime;

        // GazePointer�� �������� �� ������ �ø������� ���� �������� ���� �������� �ʱ�ȭ�Ѵ�.
        isHitObj = false;
        curHitObj = null;
    }       // Update()

    //! ��Ʈ�� ������Ʈ Ÿ�Ժ��� �۵� ����� �����Ѵ�.
    private void HitObjChecker(GameObject hitObj, bool isActive)
    {
        // hit�� ���� �÷��̾� ������Ʈ�� ���� �ִ��� Ȯ���Ѵ�.
        if(hitObj.GetComponent<VideoPlayer>())
        {
            if(isActive)
            {
                hitObj.GetComponent<VideoFrame>().CheckVideoFrame(true);
            }
            else
            {
                hitObj.GetComponent<VideoFrame>().CheckVideoFrame(false);
            }

            // ������ �ð��� �Ǹ� 360 ���Ǿ Ư�� Ŭ�� ��ȣ�� ������ �÷����Ѵ�.
            if (curHitObj == null || curHitObj == default) { return; }
            if(1.0f <= curGazeTime / gazeChagreTime)
            {
                // ���� �÷��̾ ���� Mesh_Collider ������Ʈ�� �̸��� ���� ����/���� �������� ���
                if(hitObj.name.Contains("Right"))
                {
                    video360.SwapVideoClip(true);
                }
                else if (hitObj.name.Contains("Left"))
                {
                    video360.SwapVideoClip(false);
                }
                else
                {
                    // 360 ���Ǿ Ư�� Ŭ�� ��ȣ�� ������ �÷����Ѵ�.
                    video360.SetVideoPlay(hitObj.transform.GetSiblingIndex());
                }
                curGazeTime = 0;
            }

            // LEGACY:
            //if(1.0f <= gazeImg.fillAmount)
            //{
            //    video360.SetVideoPlay(curHitObj.transform.GetSiblingIndex());
            //}
        }
    }       // HitObjChecker()
}

