using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public RabbitCtrl target = null;

    private void OnCollisionEnter(Collision collision)
    {
        target = collision.transform.GetComponent<RabbitCtrl>();
    }

    private void OnCollisionExit(Collision collision)
    {
        target = null;
    }
}
