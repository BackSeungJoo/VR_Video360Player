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

    bool isHitObj;                      // 인터랙션이 일어나는 오브젝트에 시선이 닿는지 체크
    GameObject prevHitObj;              // 이전 프레임의 시선이 머물렀던 오브젝트 정보를 담기 위한 변수
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
        // 캔버스 오브젝트의 스케일을 거리에 따라 조절한다.
        // 1. 카메라를 기준으로 전방 방향의 좌표를 구한다
        Vector3 dir = transform.TransformPoint(Vector3.forward);

        // 2. 카메라를 기준으로 전방의 레이를 설정한다.
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
        }       // if: 3. 레이에 부딪힌 경우에는 거리 값을 이용해 UICanvas의 크기를 조절한다.

        else 
        {
            uiCanvas.localScale = defaultScale * uiScale;
            uiCanvas.position = transform.position + dir;
        }       // else: 4. 아무것도 부딪히지 않으면 기본 스케일 값

        // 5. uiCanvas가 항상 카메라 오브젝트를 바라보게 한다.
        uiCanvas.forward = transform.forward * -1;

        // GazeObj가 레이에 닿았을 때 실행
        if (isHitObj)
        {
            if(curHitObj == prevHitObj)
            {
                // 인터렉션이 발생해야 하는 오브젝트에 시선이 고정돼 있다면 시간 증가
                curGazeTime += Time.deltaTime;
            }       // if: 현재 프레임의 오브젝트가 이전 프레임의 오브젝트에서 머물러 있는 경우
            else
            {
                // 이전 프레임의 영상 정보를 업데이트한다.
                prevHitObj = curHitObj;
            }       // else: 현재 프레임의 오브젝트가 이전 프레임의 오브젝트에서 벗어난 경우

            // hit된 오브젝트가 VideoPlayer 컴포넌트를 갖고 있는지 확인한다.
            HitObjChecker(curHitObj, true);
        }

        // 시선이 벗어났거나 GazeObj가 아니라면 시간을 초기화
        else
        {
            if (prevHitObj != null && prevHitObj != default)
            {
                HitObjChecker(prevHitObj, false);
                prevHitObj = default;
            }

            curGazeTime = 0f;
        }

        // 시선이 머문 시간을 0과 최댓값 사이로 한다.
        curGazeTime = Mathf.Clamp(curGazeTime, 0, gazeChagreTime);
        // Ui Image 의 fillAmount를 업데이트한다.
        gazeImg.fillAmount = curGazeTime / gazeChagreTime;

        // GazePointer의 게이지를 한 프레임 올린다음에 현재 프레임의 사용된 변수들을 초기화한다.
        isHitObj = false;
        curHitObj = null;
    }       // Update()

    //! 히트된 오브젝트 타입별로 작동 방식을 구분한다.
    private void HitObjChecker(GameObject hitObj, bool isActive)
    {
        // hit가 비디오 플레이어 컴포넌트를 갖고 있는지 확인한다.
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

            // 정해진 시간이 되면 360 스피어에 특정 클립 번호를 전달해 플레이한다.
            if (curHitObj == null || curHitObj == default) { return; }
            if(1.0f <= curGazeTime / gazeChagreTime)
            {
                // 비디오 플레이어가 없는 Mesh_Collider 오브젝트의 이름에 따라 이전/다음 영상으로 재생
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
                    // 360 스피어에 특정 클립 번호를 전달해 플레이한다.
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

