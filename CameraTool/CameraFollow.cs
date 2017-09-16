using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    //调试阶段可以public，正式版一定要private
    //位置偏移
    private Vector3 v3_posOffset = new Vector3(8.5f,14.16f,-12.51f);
    //旋转偏移
    private Vector3 v3_eugleOffset= new Vector3(41.51f,-33.84f,0);
    //跟随目标
    public Transform tran_followTarget;
    //摄像机
    public Transform tran_camera;
    //跟随速度
    public float n_softFollowSpeed = 2;

    public Vector3 V3_PosOffset
    {
        get
        {
            return v3_posOffset;
        }

        set
        {
            v3_posOffset = value;
        }
    }

    public Vector3 V3_RotOffset
    {
        get
        {
            return v3_eugleOffset;
        }

        set
        {
            v3_eugleOffset = value;
        }
    }

    public Transform Tran_FollowTarget
    {
        get
        {
            return tran_followTarget;
        }

        set
        {
            tran_followTarget = value;
        }
    }

    public Transform Tran_camera
    {
        get
        {
            return tran_camera;
        }

        set
        {
            tran_camera = value;
        }
    }

    public float N_softFollowSpeed
    {
        get
        {
            return n_softFollowSpeed;
        }

        set
        {
            n_softFollowSpeed = value;
        }
    }
	
	// Update is called once per frame
	void Update () {


        if (Tran_camera != null)
        {
            //position
            Vector3 v3_fromPos = Tran_camera.position;
            Vector3 v3_toPos = tran_followTarget.position + v3_posOffset;

            Vector3 v3_nowPos = Vector3.Lerp(v3_fromPos, v3_toPos, Time.deltaTime * N_softFollowSpeed);
            Tran_camera.position = v3_nowPos;


            //rotation
            Quaternion qt_fromRote = Tran_camera.rotation;
            Vector3 v3_toEugle =  v3_eugleOffset;
            Quaternion qt_toRote = Quaternion.Euler(v3_toEugle);

            Quaternion qt_nowRote = Quaternion.Lerp(qt_fromRote, qt_toRote, Time.deltaTime * N_softFollowSpeed);
            Tran_camera.rotation = qt_nowRote;

        }

    }




}
