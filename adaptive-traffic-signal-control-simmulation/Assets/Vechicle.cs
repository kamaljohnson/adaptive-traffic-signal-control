using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Right,
    Left,
    Forward,
    Back
}

public class Vechicle : MonoBehaviour
{
    public float Speed;
    public float RotationRadius;

    private Direction currentMovingDirection;

    private bool isMoving;
    private bool atJunction;

    void Start()
    {
        Setup();
    }

    void Update()
    {
        if(isMoving)
        {
            if(CheckJunction())
            {
                atJunction = true;
            }
            else
            {
                Move(currentMovingDirection);
            }
        }
        else
        {

        }
    }

    private void Setup()
    {
        currentMovingDirection = (Direction)Random.Range(0, sizeof(Direction) - 1);
        isMoving = true;
        atJunction = false;
    }

    private void Move(Direction direction)
    {
        switch (direction)
        {
            case Direction.Right:
                Rotate(direction);
                break;
            case Direction.Left:
                Rotate(direction);
                break;
            case Direction.Forward:
                transform.Translate(Vector3.forward * Speed * Time.deltaTime);
                break;
        }
    }

    private void Rotate(Direction direction)
    {
        var angle = direction == Direction.Right ? 90 : -90;
        transform.RotateAround(transform.position, transform.up, angle);
        currentMovingDirection = Direction.Forward;
    }

    private bool CheckJunction()
    {
        return false;
    }
}
