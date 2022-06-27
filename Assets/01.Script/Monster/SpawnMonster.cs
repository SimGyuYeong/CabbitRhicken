using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
    //몬스터 오브젝트
    public GameObject monsterObj = null;

    //몬스터들을 담아놓기
    public List<GameObject> monsters = new List<GameObject>();

    //몬스터 최대 생성 수
    public int maxSpawnCount = 20;

    //몬스터 생성 카운트
    public int spawnCount = 0;

    //몬스터 생성 랜덤좌표 최소, 최대값
    public Vector3 minPos, maxPos;

    public void Init(int count)
    {
        if(monsters.Count != 0)
        {
            foreach(var monster in monsters)
            {
                Destroy(monster);
            }
        }

        maxSpawnCount = count;
        spawnCount = 0;
        monsters.Clear();
    }

    public IEnumerator MonsterSpawnCoroutine()
    {
        while(true)
        {
            spawnCount = monsters.Count;
            if (spawnCount == maxSpawnCount) break;

            //몬스터 생성 위치 랜덤설정
            Vector3 spawnPos = new Vector3(Random.Range(minPos.x, maxPos.x), 1000f, Random.Range(minPos.z, maxPos.z));

            Ray ray = new Ray(spawnPos, Vector3.down);

            RaycastHit raycastHit = new RaycastHit();
            if(Physics.Raycast(ray, out raycastHit, Mathf.Infinity) == true)
            {
                spawnPos.y = raycastHit.point.y;
            }

            GameObject monster = Instantiate(monsterObj, spawnPos, Quaternion.identity);

            monsters.Add(monster);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
