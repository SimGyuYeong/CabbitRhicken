using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerTime = 100;

    private void Start()
    {
        StartCoroutine(ReductionTime());
    }

    IEnumerator ReductionTime()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            playerTime -= 1;
        }
    }

    public void DamagedMonster(int damage)
    {
        playerTime -= damage;
    }
}
