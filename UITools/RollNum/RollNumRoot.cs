using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollNumRoot : MonoBehaviour {

    const int c_defaultNum = -9999; //默认原先数字 

    public GameObject NumRoot;
    public Image NumPrefab;

    private int oldNum = 0;
    private int[] oldNumPer;
    private int AimNum = 0;
    private int[] AimNumPer;

    List<Image> AllNum = new List<Image>();

	// Use this for initialization
	void Start () {

    }

    public void Init()
    {

    }

    public void SetOldNum(int num)
    {
        oldNum = num;
        oldNumPer = ApartNum(oldNum);
    }

    public void ChangeNum(int aimNum,float time, int oldNum = c_defaultNum)
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

        for (int i = 0; i < needImageNum; i++)
        {
            if (AllNum.Count < i)
            {
                GameObject newGo = Instantiate(NumPrefab.gameObject);

                AllNum.Add()
            }
        }


        for (int i = startNum; i < endNum; i = i+ add)
        {




        }

    }



	// Update is called once per frame
	void Update () {
		
	}


    //将 int 转换为 int 数组， 从个位开始
    int[] ApartNum(int num )
    {
        string stringNum = num.ToString();
        int stringLength = stringNum.Length;
        Debug.Log("长度：" + stringNum);
        int[] numPer = new int[stringLength];
        for (int i = 0; i < stringLength; i++)
        {
            numPer[i] = int.Parse( stringNum.Substring(stringLength-i-1, 1));
            Debug.Log("i:" + i + "  " + numPer[i]);
        }

        return numPer;
    }


}
