using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FlootExpand {

    //四舍五入
    public static int Round45(this float value)
    {
        //Debug.Log("==Round45== value ==" + value);
        if (value > 0)
        {
            value += 0.49f;
        }
        else
        {
            value -= 0.49f;
        }
        //Debug.Log("==Round45==" + value);
        return (int)(value);
    }


}
