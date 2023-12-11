using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// ������̼� ����� ����ϱ� ���� �߰��ؾ� �ϴ� ���ӽ����̽�
using UnityEngine.AI;


public class MonsterCtrl : MonoBehaviour
{
    // ������ ���� ����
    public enum State
    {
        IDLE,
        TRACE,
        ATTACK,
        DIE
    }

    // ������ ���� ����
    public State state = State.IDLE;
    // ���� �����Ÿ�
    public float traceDist = 10.0f;
    // ���� �����Ÿ�
    public float attackDist = 2.0f;
    // ������ ��� ����
    public bool isDie = false;

    // ������Ʈ�� ĳ�ø� ó���� ����
    private Transform MonsterTr;
    private Transform PlayerTr;
    private NavMeshAgent agent;
    private Animator anim;

    // Animator �Ķ������ �ؽð� ����
    private readonly int hashTrace = Animator.StringToHash("IsTrace");
    private readonly int hashAttack = Animator.StringToHash("IsAttack");
    private readonly int hashHit = Animator.StringToHash("Hit");
    private readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");

    // ���� ȿ�� ������
    private GameObject bloodEffect;


    // ���� ���� ����
    private int hp = 100;

    // ��ũ��Ʈ�� Ȱ��ȭ�� ������ ȣ��Ǵ� �Լ�
    private void OnEnable()
    {
        // �̺�Ʈ �߻� �� ������ �Լ� ����
        PlayerCtrl.OnPlayerDie += this.OnPlayerDie;

        // ������ ���¸� üũ�ϴ� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(CheckMonsterState());
        // ���¿� ���� ������ �ൿ�� �����ϴ� �ڷ�ƾ �Լ� ȣ��
        StartCoroutine(MonsterAction());
    }

    private void OnDisable()
    {
        // ������ ����� �Լ� ����
        PlayerCtrl.OnPlayerDie -= this.OnPlayerDie;
    }


    void Awake()
    {
        // ������ Transform �Ҵ�
        MonsterTr = GetComponent<Transform>();

        // ���� ����� Player�� Transform �Ҵ�
        PlayerTr = GameObject.FindWithTag("PLAYER").GetComponent<Transform>();

        // NavMeshAgent ������Ʈ �Ҵ�
        agent = GetComponent<NavMeshAgent>();

        // Animator ������Ʈ �Ҵ�
        anim = GetComponent<Animator>();

        // BloodSprayEffect ������ �ε�
        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");
    }

    // ������ �������� ������ �ൿ ���¸� üũ
    IEnumerator CheckMonsterState()
    {
        while (!isDie)
        {
            // 0.3�� ���� ����(���)�ϴ� ���� ������� �޽��� ������ �纸
            yield return new WaitForSeconds(0.3f);

            // ������ ���°� DIE�� �� �ڷ�ƾ�� ����
            if (state == State.DIE) yield break;


            // ���Ϳ� ���ΰ� ĳ���� ������ �Ÿ� ����
            float distance = Vector3.Distance(PlayerTr.position, MonsterTr.position);

            //���� �����Ÿ� ������ ���Դ��� Ȯ��
            if (distance <= attackDist)
            {
                state = State.ATTACK;
            }
            // ���� �����Ÿ� ������ ���Դ��� Ȯ��
            else if (distance <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
        }
    }

    // ������ ���¿� ���� ������ ������ ����
    IEnumerator MonsterAction()
    {
        while (!isDie)
        {
            switch (state)
            {
                // IDLE ����  
                case State.IDLE:
                    // ���� ����
                    agent.isStopped = true;
                    // Animator�� IsTrace ������ false ����
                    anim.SetBool(hashTrace, false);
                    break;

                // ���� ����  
                case State.TRACE:
                    // ���� ����� ��ǥ�� �̵� ����
                    agent.SetDestination(PlayerTr.position);
                    agent.isStopped = false;

                    // Animator�� IsTrace ������ true ����
                    anim.SetBool(hashTrace, true);

                    // Animator�� IsAttack ������ false ����
                    anim.SetBool(hashAttack, false);
                    break;

                // ���� ����  
                case State.ATTACK:
                    // Animator�� IsAttack ������ true ����
                    anim.SetBool(hashAttack, true);
                    break;

                // ��� ����  
                case State.DIE:
                    isDie = true;
                    // ���� ����
                    agent.isStopped = true;
                    // ��� �ִϸ��̼� ����
                    anim.SetTrigger(hashDie);
                    // ������ Collider ������Ʈ ��Ȱ��ȭ
                    GetComponent<CapsuleCollider>().enabled = false;

                    // ���� �ð� ��� �� ������Ʈ Ǯ������ ȯ��
                    yield return new WaitForSeconds(3.0f);

                    // ��� �� �ٽ� ����� ���� ���� hp �� �ʱ�ȭ
                    hp = 100;
                    isDie = false;

                    // ������ Collider ������Ʈ Ȱ��ȭ
                    GetComponent<CapsuleCollider>().enabled = true;
                    // ���͸� ��Ȱ��ȭ
                    this.gameObject.SetActive(false);

                    state = State.IDLE; //���� �� ���� �ذ� �ڵ�
                    break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("BULLET"))
        {
            // �浹�� �Ѿ��� ����
            Destroy(coll.gameObject);
        }
    }

    void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        // ���� ȿ�� ����
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot, MonsterTr);
        Destroy(blood, 1.0f);
    }

    private void OnDrawGizmos()
    {
        // ���� �����Ÿ� ǥ��
        if (state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDist);
        }
        // ���� �����Ÿ� ǥ��
        if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDist);
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        Debug.Log(coll.gameObject.name);
    }

    void OnPlayerDie()
    {
        // ������ ���¸� üũ�ϴ� �ڷ�ƾ �Լ��� ��� ������Ŵ
        StopAllCoroutines();

        // ������ �����ϰ� �ִϸ��̼��� ����
        agent.isStopped = true;
        anim.SetFloat(hashSpeed, Random.Range(0.8f, 1.2f));
        anim.SetTrigger(hashPlayerDie);
    }

    public void OnDamage(Vector3 pos, Vector3 normal)
    {
        // �ǰ� ���׼� �ִϸ��̼� ����
        anim.SetTrigger(hashHit);
        Quaternion rot = Quaternion.LookRotation(normal);

        // ���� ȿ���� �����ϴ� �Լ� ȣ��
        ShowBloodEffect(pos, rot);

        // ������ hp ����
        hp -= 30;
        if (hp <= 0)
        {
            state = State.DIE;
            // ���Ͱ� ������� �� 50���� �߰�
            GameManager.instance.DisplayScore(50);
        }
    }
}