using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// 내비게이션 기능을 사용하기 위해 추가해야 하는 네임스페이스
using UnityEngine.AI;


public class MosterCtrl : MonoBehaviour
{
    // 컴포넌트의 캐시를 처리할 변수
    private Transform MonsterTr;
    private Transform PlayerTr;
    private NavMeshAgent agent;



    // Start is called before the first frame update
    void Start()
    {
        // 몬스터의 Transform 할당
        MonsterTr = GetComponent<Transform>();

        // 추적 대상인 Player의 Transform 할당
        PlayerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();

        // NavMeshAgent 컴포넌트 할당
        agent = GetComponent<NavMeshAgent>();

        // 추적 대상의 위치를 설정하면 바로 추적 시작
        agent.destination = PlayerTr.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
