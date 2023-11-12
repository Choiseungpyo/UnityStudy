using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{

    //컴포넌트를 캐시 처리할 변수
    // [SerializeField] // private 접근 지시자의 속성을 유지한 채 인스펙터 뷰에 노출하는 기능.
    private Transform tr;
    // Animation 컴포넌트를 저장할 변수
    private Animation anim;


    //이동 속도 변수 (public으로 선언되어 인스펙터 뷰에 노출됨)
    public float moveSpeed = 10.0f;
    //회전 속도 변수
    public float turnSpeed = 80.0f;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        // Tansform 컴포넌트를 추출해 변수에 대입
        tr = GetComponent<Transform>(); // 함수명<형식(추출할 클래스)>(인자(매개변수));
        anim = GetComponent<Animation>();

        // 애니메이션 실행
        anim.Play("Idle");

        turnSpeed = 0.0f;
        yield return new WaitForSeconds(0.3f);
        turnSpeed = 80.0f;

        // tr = this.gameObject.GetComponent<Transform>() : 이 스크립트가 포함된 게임오브젝트가 가진 여러 컴포넌트 중에서 Transform 컴포넌트를 추출해 tr 변수에 저장하라.
        // tr = GetComponent ("Transform") as Transform;
        // tr = (Transform)GetComponent(trypeof(Transform));

        // this : 해당 클래스(스크립트) ex) PlayerCtrl 스크립트
        // this.gameObject : 이 스크립트가 추가된 게임 오브젝트
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal"); // -1.0f ~ 0.0f ~ +1.0f : A,D  방향키 좌,우
        float v = Input.GetAxis("Vertical");   // -1.0f ~ 0.0f ~ +1.0f : W,S 방향키 상, 하
        float r = Input.GetAxis("Mouse X");

        /*Debug.log("h=" + h);
        Debug.log("v=" + v);*/


        // 전후좌우 이동방향 벡터 계산 
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        // Translate(이동 방향 * 속력 * Time.deltaTime);
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime); //대각선으로 이동할때 더 빨라지기 때문에 정규화시켜 속도를 일정하게 맞춘다.

        
        // Vector3.up 축을 기준으로 turnSpeed만큼의 속도로 회전
        tr.Rotate(Vector3.up * turnSpeed * Time.deltaTime * r);

        // 주인공 캐릭터의 애니메이션 설정
        PlayerAnim(h, v);

        //Transform 컴포넌트의 Position 속성 값을 변경
        //transform.position += new Vector3(0,0,1); 
        //컴포넌트.속성 += 저장할 값.

        //정규화 벡터를 사용한 코드
        // tr.position += Vector3.forward * 1; // 전진 방향 x 속력

        //Translate 함수를 사용한 이동 로직
        //tr.Translate(Vector3.forward * Time.deltaTime * v * moveSpeed);
        //tr.Translate( {이동할 방향} * Time.deltaTime * {전진/후진 변수} * {속도} );

        //Time.deltaTime : 이전 프레임의 시작 시각부터 현재 프레임이 시작되는 시각의 차(델타)
        //Time.deltaTime을 곱하지 않았을 때  ( tr.Translate(Vector3.forward * Time.deltaTime * 10);) : 프레임당 지정한 유닛만큼 이동
        //Time.deltaTime을 곱했을 때          (tr.Translate(Vector3.forward * 10);: 초당 지정한 유닛만큼 이동 
        //Update 함수에 이동 및 회전을 처리하는 로직을 작성하면 반듸 Time.detaTime 속성을 사용해야 한다. 
    }


    void PlayerAnim(float h, float v)
    {
        //키보드 입력값을 기준으로 동작할 애니메이션 수행

        if(v >= 0.1f)
        {
            anim.CrossFade("RunF", 0.25f); //전진 애니메이션 실행
        }
        else if(v <= -0.1f)
        {
            anim.CrossFade("RunB", 0.25f); //후진 애니메이션 실행
        }
        else if (h >= 0.1f)
        {
            anim.CrossFade("RunR", 0.25f); //오른쪽 이동 애니메이션 실행
        }
        else if (h <= -0.1f)
        {
            anim.CrossFade("RunL", 0.25f); //왼쪽 이동 애니메이션 실행
        }
        else
        {
            anim.CrossFade("Idle", 0.25f); //정지 시 Idle 애니메이션 실행
        }

        //anim.CrossFade(변경할 애니메이션 클립의 명칭, 다른 애니메이션 클립으로 페이드아웃 되는 시간)
    }
}
