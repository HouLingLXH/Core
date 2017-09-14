using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollNumRoot : MonoBehaviour {

    const int c_defaultNum = -9999; //默认原先数字 

    public GameObject NumRoot; //位 根节点
    public Image NumPrefab; // 位 预设
    public float MaxSpeed = 7; // 最大滚动速度
    public float MinusSpeed = 1;//每位递减的速度

    private int oldNum = 0;  // 起点数
    private int[] oldNumPer; // 起点数的每一位，从个位开始
    private int AimNum = 0;  // 目标数
    private int[] AimNumPer; // 目标数的每一位，从个位开始

    List<RoolNum> AllNum = new List<RoolNum>(); //所有 位

    //直接记录滚动起点
    public void SetOldNum(int num)
    {
        oldNum = num;
        oldNumPer = ApartNum(oldNum);
    }

    //开始滚动
    public void ChangeNum(int aimNum, int oldNum = c_defaultNum)
    {
        if (oldNum != c_defaultNum)
        {
            SetOldNum(oldNum);
        }

        AimNum = aimNum;
        AimNumPer = ApartNum(AimNum);

        int startNum = 0;
        int endNum = 0;
        int add = 1;
        int needImageNum = 0;


        if (AimNum > oldNum)
        {
            startNum = 0;
            endNum = AimNumPer.Length;
            add = 1;
            needImageNum = endNum;
        }
        else
        {
            startNum = oldNumPer.Length;
            endNum = 0;
            add = -1;
            needImageNum = startNum;
        }

        //展示、创建需要的位数
        for (int i = 0; i < needImageNum; i++)
        {
            if (AllNum.Count <= i)
            {
                GameObject newGo = Instantiate(NumPrefab.gameObject);
                newGo.transform.SetParent(NumRoot.transform);
                newGo.transform.localScale = Vector3.one;
                newGo.SetActive(true);
                newGo.name = i.ToString();
                RoolNum roolNum = newGo.GetComponent<RoolNum>();
                AllNum.Add(roolNum);
                roolNum.SetNum(0);
            }
            else
            {
                int l_oldNum = 0;
                if (oldNumPer.Length > (needImageNum - i - 1))
                {
                    l_oldNum = oldNumPer[needImageNum - i - 1];
                }
                AllNum[i].gameObject.SetActive(true);
                AllNum[i].SetNum(l_oldNum);
            }
        }

        //隐藏不需要的位数
        for (int i = needImageNum ; i < AllNum.Count; i++)
        {
            AllNum[i].gameObject.SetActive(false);
        }

        //根据增减， 滚动的数序和 限制（空位滚到0） 
        if (add > 0)
        {
            for (int i = startNum; i < endNum; i = i + add)
            {
                AllNum[needImageNum - i - 1].Move(AimNumPer[i], MaxSpeed - i * MinusSpeed);
            }
        }
        else
        {
            for (int i = startNum; i > endNum; i = i + add)
            {
                int l_aimNum = 0;
                if (AimNumPer.Length > i-1 )
                {
                    l_aimNum = AimNumPer[i - 1];
                }
                AllNum[needImageNum - i].Move(l_aimNum, MaxSpeed - i* MinusSpeed);
            }
        }
    }

    //将 int 转换为 int 数组， 从个位开始
    int[] ApartNum(int num )
    {
        string stringNum = num.ToString();
        int stringLength = stringNum.Length;
        //Debug.Log("长度：" + stringNum);
        int[] numPer = new int[stringLength];
        for (int i = 0; i < stringLength; i++)
        {
            numPer[i] = int.Parse( stringNum.Substring(stringLength-i-1, 1));
            //Debug.Log("i:" + i + "  " + numPer[i]);
        }

        return numPer;
    }



    //测试
    public void Btn_Change()
    {
        ChangeNum(156, 3);
    }

    public void Btn_Change2()
    {
        ChangeNum(3, 20);
    }
}
