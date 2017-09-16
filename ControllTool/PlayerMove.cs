using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    const float speed = 2;

	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.W))
        {
            MoveW();
        }

        if (Input.GetKey(KeyCode.A))
        {
            MoveA();
        }

        if (Input.GetKey(KeyCode.D))
        {
            MoveD();
        }

        if (Input.GetKey(KeyCode.S))
        {
            MoveS();
        }
    }

    public void MoveW()
    {
        Move(Vector3.forward);
    }

    public void MoveA()
    {
        Move(Vector3.left);
    }

    public void MoveD()
    {
        Move(Vector3.left * -1);
    }

    public void MoveS()
    {
        Move(Vector3.forward * -1);
    }

    //在XZ平面内自由移动 
    public void Move(Vector3 v3_dir,float n_speed = speed)
    {
        transform.position += v3_dir * Time.deltaTime * n_speed;
    }

}
