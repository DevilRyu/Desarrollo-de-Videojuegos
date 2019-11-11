using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    public float tickDeltaTime;
    public float fastTickDeltaTime;
    public float speedFactor;
    public TetrisBoard Board;

    private Transform trans;
    private float accDeltaTime;

    // Start is called before the first frame update

    void Awake()
    {
        accDeltaTime = 0f;   
    }

    void Start()
    {
        trans = this.transform;    
    }

    (bool,bool) ApplyConstraints(Vector3 rollbackPos, Quaternion rollbackRot)
    {
        var (isOutX,isOutY) = CheckConstraints();
        if (isOutX|| isOutY)
        {
            trans.position = rollbackPos;
            trans.rotation = rollbackRot;
        }

        return (isOutX,isOutY);

    }

    (bool,bool) CheckConstraints()
    {
        var isOutX = false;
        var isOutY = false;
        for (int i = 0; i < trans.childCount; i++)
        {
            var childTrans = trans.GetChild(i);
            if(childTrans.position.x <0 || childTrans.position.x > 11)
            {
                isOutX = true;
                break;
            }
            if (childTrans.position.y < 0)
            {
                isOutY = true;
                break;
            }
        }
        return (isOutX,isOutY);
    }

    // Update is called once per frame
    void Update()
    {
        {//Rotation
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                var oldPos = trans.position;
                var oldRot = trans.rotation;
                trans.rotation *= Quaternion.Euler(0, 0, 90);
                ApplyConstraints(oldPos,oldRot);
                //trans.Rotate(Vector3.forward,90);
                //trans.Rotare(new Vector3(0,0,90),Space.Self);
            }
        }

        {//X-axis movement
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                var oldPos = trans.position;
                var newPos = trans.position;
                newPos.x -= 1;
                trans.position = newPos;
                ApplyConstraints(oldPos,transform.rotation);
                
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                var oldPos = trans.position;
                var newPos = trans.position;
                newPos.x += 1;
                trans.position = newPos;
                ApplyConstraints(oldPos,transform.rotation);
            }
        }

        { //Y-axis movement
            accDeltaTime += Time.deltaTime;
            var actualMovementTick = tickDeltaTime;

            if (Input.GetKey(KeyCode.DownArrow))
            {
                actualMovementTick = fastTickDeltaTime;
                //actualMovementTick = speedFactor * tickDeltaTime;
            }
            if (accDeltaTime > actualMovementTick)
            {
                {//Move Down

                    var oldPos = trans.position;
                    var oldRot = trans.rotation;

                    var newPos = trans.position;
                    newPos.y -= 1;
                    trans.position = newPos;

                    var(isOutX,isOutY)=ApplyConstraints(oldPos,oldRot);

                    var done = false;
                    if (done)
                    {
                        this.enabled=false;
                        Board.InstantiateTetrisBlock();
                    }
                }

                accDeltaTime = 0;
            }
        }

    }
}
