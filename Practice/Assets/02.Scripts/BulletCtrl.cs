using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // �Ѿ��� �ı���
    public float damage = 20.0f;

    // �Ѿ� �߻� ��
    public float force = 1500.0f;

    private Rigidbody rb;



    // Start is called before the first frame update
    void Start()
    {
        // Rigidbody ������Ʈ�� ����
        rb = GetComponent<Rigidbody>();

        // �Ѿ��� ���� �������� ��(Force)�� ���Ѵ�.
        rb.AddForce(transform.forward * force);
        // Addforce() : ���� ��ǥ��� ���ư�
        // AddRelativeForce() : ������ǥ��� ���ư�
    }

    // Update is called once per frame
    void Update()
    {

    }
}
