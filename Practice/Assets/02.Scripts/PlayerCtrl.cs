using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{

    //������Ʈ�� ĳ�� ó���� ����
    // [SerializeField] // private ���� �������� �Ӽ��� ������ ä �ν����� �信 �����ϴ� ���.
    private Transform tr;
    // Animation ������Ʈ�� ������ ����
    private Animation anim;


    //�̵� �ӵ� ���� (public���� ����Ǿ� �ν����� �信 �����)
    public float moveSpeed = 10.0f;
    //ȸ�� �ӵ� ����
    public float turnSpeed = 80.0f;

    // �ʱ� ���� ��
    private readonly float initHp = 100.0f;
    // ���� ���� ��
    private float currHp;
    // Hpbar ������ ����
    private Image hpbar;

    // ��������Ʈ ����
    public delegate void PlayerDieHandler();
    // �̺�Ʈ ����
    public static event PlayerDieHandler OnPlayerDie;


    // Start is called before the first frame update
    IEnumerator Start()
    {
        // Hpbar ����
        hpbar = GameObject.FindGameObjectWithTag("HP_BAR")?.GetComponent<Image>();
        // Hp �ʱ�ȭ
        currHp = initHp;

        // Tansform ������Ʈ�� ������ ������ ����
        tr = GetComponent<Transform>(); // �Լ���<����(������ Ŭ����)>(����(�Ű�����));
        anim = GetComponent<Animation>();

        // �ִϸ��̼� ����
        anim.Play("Idle");

        turnSpeed = 0.0f;
        yield return new WaitForSeconds(0.3f);
        turnSpeed = 80.0f;

        // tr = this.gameObject.GetComponent<Transform>() : �� ��ũ��Ʈ�� ���Ե� ���ӿ�����Ʈ�� ���� ���� ������Ʈ �߿��� Transform ������Ʈ�� ������ tr ������ �����϶�.
        // tr = GetComponent ("Transform") as Transform;
        // tr = (Transform)GetComponent(trypeof(Transform));

        // this : �ش� Ŭ����(��ũ��Ʈ) ex) PlayerCtrl ��ũ��Ʈ
        // this.gameObject : �� ��ũ��Ʈ�� �߰��� ���� ������Ʈ
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal"); // -1.0f ~ 0.0f ~ +1.0f : A,D  ����Ű ��,��
        float v = Input.GetAxis("Vertical");   // -1.0f ~ 0.0f ~ +1.0f : W,S ����Ű ��, ��
        float r = Input.GetAxis("Mouse X");

        /*Debug.log("h=" + h);
        Debug.log("v=" + v);*/


        // �����¿� �̵����� ���� ��� 
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        // Translate(�̵� ���� * �ӷ� * Time.deltaTime);
        tr.Translate(moveDir.normalized * moveSpeed * Time.deltaTime); //�밢������ �̵��Ҷ� �� �������� ������ ����ȭ���� �ӵ��� �����ϰ� �����.

        
        // Vector3.up ���� �������� turnSpeed��ŭ�� �ӵ��� ȸ��
        tr.Rotate(Vector3.up * turnSpeed * Time.deltaTime * r);

        // ���ΰ� ĳ������ �ִϸ��̼� ����
        PlayerAnim(h, v);

        //Transform ������Ʈ�� Position �Ӽ� ���� ����
        //transform.position += new Vector3(0,0,1); 
        //������Ʈ.�Ӽ� += ������ ��.

        //����ȭ ���͸� ����� �ڵ�
        // tr.position += Vector3.forward * 1; // ���� ���� x �ӷ�

        //Translate �Լ��� ����� �̵� ����
        //tr.Translate(Vector3.forward * Time.deltaTime * v * moveSpeed);
        //tr.Translate( {�̵��� ����} * Time.deltaTime * {����/���� ����} * {�ӵ�} );

        //Time.deltaTime : ���� �������� ���� �ð����� ���� �������� ���۵Ǵ� �ð��� ��(��Ÿ)
        //Time.deltaTime�� ������ �ʾ��� ��  ( tr.Translate(Vector3.forward * Time.deltaTime * 10);) : �����Ӵ� ������ ���ָ�ŭ �̵�
        //Time.deltaTime�� ������ ��          (tr.Translate(Vector3.forward * 10);: �ʴ� ������ ���ָ�ŭ �̵� 
        //Update �Լ��� �̵� �� ȸ���� ó���ϴ� ������ �ۼ��ϸ� �ݵ� Time.detaTime �Ӽ��� ����ؾ� �Ѵ�. 
    }


    void PlayerAnim(float h, float v)
    {
        //Ű���� �Է°��� �������� ������ �ִϸ��̼� ����

        if(v >= 0.1f)
        {
            anim.CrossFade("RunF", 0.25f); //���� �ִϸ��̼� ����
        }
        else if(v <= -0.1f)
        {
            anim.CrossFade("RunB", 0.25f); //���� �ִϸ��̼� ����
        }
        else if (h >= 0.1f)
        {
            anim.CrossFade("RunR", 0.25f); //������ �̵� �ִϸ��̼� ����
        }
        else if (h <= -0.1f)
        {
            anim.CrossFade("RunL", 0.25f); //���� �̵� �ִϸ��̼� ����
        }
        else
        {
            anim.CrossFade("Idle", 0.25f); //���� �� Idle �ִϸ��̼� ����
        }

        //anim.CrossFade(������ �ִϸ��̼� Ŭ���� ��Ī, �ٸ� �ִϸ��̼� Ŭ������ ���̵�ƿ� �Ǵ� �ð�)
    }

    // �浹�� Collider�� IsTrigger �ɼ��� üũ���� �� �߻�
    private void OnTriggerEnter(Collider coll)
    {
        // �浹�� Collider�� ������ PUNCH�̸� Player�� HP ����
        if(currHp >= 0.0f &&  coll.CompareTag("PUNCH"))
        {
            currHp -= 10.0f;
            DisplayHealth();

            Debug.Log($"Player hp = {currHp / initHp}");

            // Player�� ������ 0�����̸� ��� ó��
            if(currHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }

    // Player�� ��� ó��
    void PlayerDie()
    {
        Debug.Log("Player Die !");

        // MONSTER �±׸� ���� ��� ���ӿ�����Ʈ�� ã�ƿ�
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");

        // ��� ������ OnPlayerDie �Լ��� ���������� ȣ��
        //foreach(GameObject monster in monsters)
        //{
        //    monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}

        // ���ΰ� ��� �̺�Ʈ ȣ��(�߻�)
        OnPlayerDie();

        // GameManager ��ũ��Ʈ�� IsGameOver ������Ƽ ���� ����
        GameObject.Find("GameMgr").GetComponent<GameManager>().IsGameOver = true;
    }

    void DisplayHealth()
    {
        hpbar.fillAmount = currHp / initHp;
    }
}
