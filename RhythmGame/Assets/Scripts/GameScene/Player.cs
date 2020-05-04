using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 targetPos;
    private bool xMove = false;
    private bool yMove = false;

    public int speed = 1;

    public float waitTimer = 1;
    private float timer = 0;
    private bool isCollision = false;

    void FixedUpdate()
    {
        if (!(xMove || yMove))
        {
            float xAxis = Input.GetAxisRaw("Horizontal") * 2;
            float yAxis = Input.GetAxisRaw("Vertical") * 2;
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
            {
                targetPos = new Vector2(transform.position.x, transform.position.y + yAxis);
                yMove = true;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                targetPos = new Vector2(transform.position.x + xAxis, transform.position.y);
                xMove = true;
            }
        }
        else
        {
            if (xMove)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                if (transform.position.Equals(targetPos))
                {
                    if (!isCollision)
                    {
                        if (targetPos.x > 0)
                        {
                            FindObjectOfType<GameManager>().JudgementCheck((int)Direction.RIGHT);
                        }
                        else
                        {
                            FindObjectOfType<GameManager>().JudgementCheck((int)Direction.LEFT);
                        }
                    }
                    isCollision = true;

                    timer += Time.deltaTime;
                    if (timer >= waitTimer)
                    {
                        ResetPos();
                        xMove = false;
                    }
                }
            }
            else if (yMove)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

                if (transform.position.Equals(targetPos))
                {
                    if (!isCollision)
                    {
                        if (targetPos.y > 0)
                        {
                            FindObjectOfType<GameManager>().JudgementCheck((int)Direction.UP);
                        }
                        else
                        {
                            FindObjectOfType<GameManager>().JudgementCheck((int)Direction.DOWN);
                        }
                    }
                    isCollision = true;

                    timer += Time.deltaTime;
                    if (timer >= waitTimer)
                    {
                        ResetPos();
                        yMove = false;
                    }
                }
            }
        }
    }
    void ResetPos()
    {
        timer = 0;
        isCollision = false;
        targetPos = Vector2.zero;
        transform.position = Vector2.zero;
    }
}
