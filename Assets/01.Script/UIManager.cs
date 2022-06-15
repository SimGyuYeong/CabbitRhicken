using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private PlayerMove _pMove;

    private void OnGUI()
    {
        var labelStyle = new GUIStyle();
        labelStyle.fontSize = 30;
        labelStyle.normal.textColor = Color.black;

        //float _getVelocitySpd = _pMove.GetVelocitySpd();

        //GUILayout.Label("현재속도 : " + _getVelocitySpd.ToString(), labelStyle);

        GUILayout.Label("현재시간 : " + _player.playerTime.ToString() + "초", labelStyle);
    }

}
