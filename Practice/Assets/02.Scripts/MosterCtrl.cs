using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// ������̼� ����� ����ϱ� ���� �߰��ؾ� �ϴ� ���ӽ����̽�
using UnityEngine.AI;


public class MosterCtrl : MonoBehaviour
{
    // ������Ʈ�� ĳ�ø� ó���� ����
    private Transform MonsterTr;
    private Transform PlayerTr;
    private NavMeshAgent agent;



    // Start is called before the first frame update
    void Start()
    {
        // ������ Transform �Ҵ�
        MonsterTr = GetComponent<Transform>();

        // ���� ����� Player�� Transform �Ҵ�
        PlayerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();

        // NavMeshAgent ������Ʈ �Ҵ�
        agent = GetComponent<NavMeshAgent>();

        // ���� ����� ��ġ�� �����ϸ� �ٷ� ���� ����
        agent.destination = PlayerTr.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
