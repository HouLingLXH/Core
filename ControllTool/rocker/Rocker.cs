using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Rocker : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public float n_speedMin = 1; //最小速度
    public float n_speedMax = 5; //最大速度
    public Image i_rockerBg; //摇杆背景（活动区域）
    public Image i_rockerCenter;//摇杆中心（杆）
    public PlayerMove moveComp; //移动组件
    public CameraFollow cameraFollow; // 摄像机跟随组件

    private float n_rockerbgSizeHalf; // 摇杆半径半边长度
    private float n_rockerCenterSizeHalf;// 杆半边长度
    private float n_rockerActiveSizeHalf;// 活动区域半边长度
    private float n_rockerActiveSizeHalfSqr;// 活动区域半边长度平方
    private Vector2 n_rockerBgCenter;
    Vector3 v3_moveDir;

    private void Start()
    {
        //获取必要数据
        n_rockerbgSizeHalf = i_rockerBg.rectTransform.sizeDelta.x * 0.5f;
        n_rockerCenterSizeHalf = i_rockerCenter.rectTransform.sizeDelta.x * 0.5f;
        n_rockerActiveSizeHalf = n_rockerbgSizeHalf - n_rockerCenterSizeHalf;
        n_rockerActiveSizeHalfSqr = n_rockerActiveSizeHalf * n_rockerActiveSizeHalf;

        n_rockerBgCenter = i_rockerBg.rectTransform.position;

        //摇杆归位
        Rehome();

    }

    //摇杆归位
    public void Rehome()
    {
        i_rockerCenter.transform.localPosition = Vector3.zero;

    }

    //在拖拽时
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 v3_rockerDir = (eventData.position - n_rockerBgCenter);
        v3_moveDir = new Vector3(v3_rockerDir.x, 0, v3_rockerDir.y);
    }

    //结束拖拽
    public void OnEndDrag(PointerEventData eventData)
    {
        Rehome();
        v3_moveDir = Vector3.zero;
    }

    //开始拖拽
    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    //摇杆中心 移动
    private void MoveRockerCenter(Vector3 moveDir)
    {
        //不允许摇杆中心超出背景边界(背景宽度 - 杆宽度)
        if (moveDir.sqrMagnitude > n_rockerActiveSizeHalfSqr)
        {
            moveDir = moveDir.normalized * n_rockerActiveSizeHalf;
        }

        i_rockerCenter.transform.localPosition = new Vector3(moveDir.x, moveDir.z, 0);
        //通知角色移动
        float speed = moveDir.sqrMagnitude / n_rockerActiveSizeHalfSqr * (n_speedMax - n_speedMin) + n_speedMin;
        moveComp.Move(moveDir.normalized.Vector3RotateInXZ2(cameraFollow.V3_RotOffset.y), speed);
    }


    private void Update()
    {
        //摇杆中心 移动
        MoveRockerCenter(v3_moveDir);

    }
}
