using Constant;
using DG.Tweening;
using Info;
using Manager;
using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using UnityEngine.UI;

public enum JumpNumDir
{
    Right = 1,
    Left  = -1,
}

public class JumpNumCell : MonoBehaviour
{
    public Text text;

    private bool isDestroy;
    private JumpNumDir dir;
    private float iniScale = 1.0f;
    private float maxScale = 1.5f;

    public void Init(string content, Color color, JumpNumDir dir = JumpNumDir.Right)
    {
        isDestroy = false;
        text.text = content;
        text.color = color;
        isDestroy = false;
        this.dir = dir;
        InitMovePos();
    }

    void InitMovePos()
    {
        transform.localEulerAngles = Vector3.zero;
        transform.localPosition    = Vector3.zero;
        transform.localScale       = new Vector3(iniScale, iniScale, iniScale);
        Move2();
    }

    private float move1_Y      = 70f;
    private float move1_Y_Time = 0.5f;
    // 从上面中间跳
    void Move1()
    {
        transform.DOScale(maxScale, 0.2f).OnComplete(() =>
        {
            transform.DOScale(0.8f, 0.2f);
        });

        transform.DOLocalMoveY(move1_Y, move1_Y_Time).OnComplete(Destroy);
    }

    private float move2_X      = 90f;
    private float move2_Y1     = 50f;
    private float move2_Y2     = 30f;
    private float move2_X_Time = 0.8f;
    private float move2_Y_Time1 = 0.4f;
    private float move2_Y_Time2 = 0.4f;
    // 往左右两边抛物线
    void Move2()
    {
        transform.DOLocalMoveX(move2_X * (int)dir, move2_X_Time);
        transform.DOLocalMoveY(move2_Y1, move2_Y_Time1).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            transform.DOLocalMoveY(move2_Y2, move2_Y_Time2).SetEase(Ease.InQuad).OnComplete(Destroy);
        });
    }

    public void Destroy()
    {
        isDestroy = true;
        transform.DOPause();
        PoolManager.GetUIPool().UnSpawn(this.gameObject, name);
    }
}
