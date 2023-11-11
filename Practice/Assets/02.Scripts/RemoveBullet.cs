using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    // ����ũ ��ƼŬ �������� ������ ����
    public GameObject sparkEffect;

    // �浹�� ������ �� �߻��ϴ� �̺�Ʈ
    private void OnCollisionEnter(Collision coll)
    {
        // �浹�� ���ӿ�����Ʈ�� �±װ� ��
        if (coll.collider.tag == "BULLET")
        {
            // ù ��° �浹 ������ ���� ����
            ContactPoint cp = coll.GetContact(0);
            // �浹�� �Ѿ��� ���� ���͸� ���ʹϾ� Ÿ������ ��ȯ
            Quaternion rot = Quaternion.LookRotation(-cp.normal);

            // ����ũ ��ƼŬ�� �������� ����
            GameObject spark = Instantiate(sparkEffect, cp.point, rot);
            // ���� �ð��� ���� �� ����Ŭ ��ƼŬ�� ����
            Destroy(spark, 0.5f);

            // �浹�� ���ӿ�����Ʈ ����
            Destroy(coll.gameObject);
        }
    }
}
