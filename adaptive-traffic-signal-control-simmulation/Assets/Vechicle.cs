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

    private float movementSnap = 1f;

    private List<bool> junctionPaths = new List<bool>
    {
        false,  //Direciton.Right
        false,  //Direction.Left
        false,  //Direction.Forward
    };

    private Vector3 destinationVector;

    private Direction currentMovingDirection;

    private bool isMoving;
    private bool atJunction;
    private bool directionChanged;
    private bool movementSnapped;

    void Start()
    {
        Setup();
    }

    void Update()
    {
        if(isMoving)
        {
            if(movementSnapped)
            {
                if(atJunction && !directionChanged)
                {
                    RotateRandom();
                }
                movementSnapped = false;
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
        currentMovingDirection = Direction.Forward;
        isMoving = true;
        atJunction = false;
        destinationVector = transform.position + transform.forward * movementSnap;
        directionChanged = false;
    }

    private void Move(Direction direction)
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, destinationVector) <= 0.01f)
        {
            movementSnapped = true;
            Debug.Log("Vehicle Snapped to grid");
            transform.position = destinationVector;
            destinationVector += transform.forward * movementSnap;
        }
    }

    private void RotateRandom()
    {
        var changeDirection = Direction.Forward;
        var angle = 0;

        while (true)
        {
            changeDirection = (Direction)Random.Range(0, sizeof(Direction) - 1);
            if(junctionPaths[(int)changeDirection])
            {
                break;
            }
        }

        switch (changeDirection)
        {
            case Direction.Right:
                angle = 90;
                break;
            case Direction.Left:
                angle = -90;
                break;
        }

        transform.RotateAround(transform.position, transform.up, angle);
        currentMovingDirection = Direction.Forward;
        directionChanged = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Junction")
        {
            directionChanged = false;
            atJunction = true;
            junctionPaths[(int)Direction.Right] = true;
            junctionPaths[(int)Direction.Left] = true;
            junctionPaths[(int)Direction.Forward] = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Junction")
        {
            atJunction = false;
        }
    }
}