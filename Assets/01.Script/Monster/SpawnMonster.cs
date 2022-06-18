using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
    //���� ������Ʈ
    public GameObject monsterObj = null;

    //���͵��� ��Ƴ���
    public List<GameObject> monsters = new List<GameObject>();

    //���� �ִ� ���� ��
    public int maxSpawnCount = 20;

    //���� ���� ī��Ʈ
    public int spawnCount = 0;

    //������ ���� ���� �ּ�, �ִ밪
    public Vector3 minPos, maxPos;

    public IEnumerator MonsterSpawnCoroutine()
    {
        while(true)
        {
            spawnCount = monsters.Count;
            if (spawnCount == maxSpawnCount) break;

            //���� ���� ��ġ ��������
            Vector3 spawnPos = new Vector3(Random.Range(minPos.x, maxPos.x), 1000f, Random.Range(minPos.z, maxPos.z));

            Ray ray = new Ray(spawnPos, Vector3.down);

            RaycastHit raycastHit = new RaycastHit();
            if(Physics.Raycast(ray, out raycastHit, Mathf.Infinity) == true)
            {
                spawnPos.y = raycastHit.point.y;
            }

            GameObject monster = Instantiate(monsterObj, spawnPos, Quaternion.identity);

            monsters.Add(monster);

            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }
    }
}
