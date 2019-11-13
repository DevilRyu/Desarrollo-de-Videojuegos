using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * To Do
 * - Block instantiation
 * - Blocks collision
 * - Line clearance
 * - Losing condition
 */
public class TetrisBlock : MonoBehaviour
{
    public float tickDeltaTime;
    public float fastTickDeltaTime;
    public TetrisBoard Board;
    public bool CanInstantiate = true;

    private Transform trans;
    private float accDeltaTime;

    void Awake()
    {
        accDeltaTime = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        trans = this.transform;
    }

    (bool, bool, bool) CheckConstraints()
    {
        var isOutX = false;
        var isOutY = false;
        var isGridNotAvailable = false;
        for (int i = 0; i < trans.childCount; i++)
        {
            var childTrans = trans.GetChild(i);
            int x = Mathf.RoundToInt(childTrans.position.x);
            int y = Mathf.RoundToInt(childTrans.position.y);
            if (x < 0
                || x > 9)
            {
                isOutX = true;
                break;
            }
            if (y < 0)
            {
                isOutY = true;
                break;
            }
            var row = y;
            var col = x;
            if (!Board.IsCellAvailable(row, col))
            {
                isGridNotAvailable = true;
                break;
            }
        }
        return (isOutX, isOutY, isGridNotAvailable);
    }

    (bool, bool, bool) ApplyConstraints(Vector3 rollbackPos,
                                        Quaternion rollbackRot)
    {
        var (isOutX, isOutY, isGridNotAvailable) = CheckConstraints();
        if (isOutX
            || isOutY
            || isGridNotAvailable)
        {
            trans.position = rollbackPos;
            trans.rotation = rollbackRot;
        }
        return (isOutX, isOutY, isGridNotAvailable);
    }

    // Update is called once per frame
    void Update()
    {
        { // Rotation
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                var oldPos = trans.position;
                var oldRot = trans.rotation;

                trans.rotation *= Quaternion.Euler(0, 0, 90);
                //trans.Rotate(Vector3.forward, 90);
                //trans.Rotate(new Vector3(0, 0, 90), Space.Self);

                ApplyConstraints(oldPos, oldRot);
            }
        }

        { // X-axis movement
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            { // Move left
                var oldPos = trans.position;
                var oldRot = trans.rotation;

                var newPos = trans.position;
                newPos.x -= 1;
                trans.position = newPos;

                ApplyConstraints(oldPos, oldRot);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            { // Move Right
                var oldPos = trans.position;
                var oldRot = trans.rotation;

                var newPos = trans.position;
                newPos.x += 1;
                trans.position = newPos;

                ApplyConstraints(oldPos, oldRot);
            }
        }

        { // Y-axis movement
            accDeltaTime += Time.deltaTime;

            //var actualMovementTick = tickDeltaTime;
            //if (Input.GetKey(KeyCode.LeftShift)
            //    || Input.GetKey(KeyCode.RightShift))
            //{
            //    actualMovementTick = fastTickDeltaTime;
            //    //actualMovementTick = speedFactor * tickDeltaTime;
            //}

            var fastSpeedPressed = Input.GetKey(KeyCode.DownArrow);
            var actualMovementTick = fastSpeedPressed ? fastTickDeltaTime : tickDeltaTime;

            if (accDeltaTime > actualMovementTick)
            {
                { // Move down
                    var oldPos = trans.position;
                    var oldRot = trans.rotation;

                    var newPos = trans.position;
                    newPos.y -= 1;
                    trans.position = newPos;

                    var (isOutX, isOutY, isGridNotAvailable) = ApplyConstraints(oldPos, oldRot);

                    var done = isOutY || isGridNotAvailable;
                    if (done)
                    {
                        this.enabled = false;
                        //this.gameObject.SetActive(false);

                        if (CanInstantiate)
                        {
                            Board.InstantiateTetrisBlock();
                            CanInstantiate = false;
                        }
                        Board.AddBlockToGrid(this);
                        Board.ApplyClearanceRule();
                    }
                }
                accDeltaTime = 0;
            }
        }
    }
}