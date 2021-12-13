using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableTile : MonoBehaviour
{
    public GameObject playerObject;

    [Header("Tile Settings")]
    [Header("Target position, starting position of the platform will be the first index")]
    public List<Transform> targetPositions;
    [Tooltip("How fast the tile moves")]
    public float tileSpeed;
    [Tooltip("How long it will stay still before moving")]
    public float delayTime;

    bool moveTile = false;
    float elapsedTime;
    int positionIndex = 0;
    Vector3 nextPos;
    bool backTravel = false;

    // Because targetPositions always update when the object is moving. So I need to store a local copy of it.
    List<Vector2> localTargetPositions = new List<Vector2>();
   

    // Start is called before the first frame update
    void Start()
    {
        // Set it to the first position in the list
        transform.position = targetPositions[positionIndex].position;
        positionIndex++;
        nextPos = targetPositions[positionIndex].position;
        

        //localTargetPositions
        //Debug.Log("Before_0" + localTargetPositions[0].position);
        //Debug.Log("Before_1" + localTargetPositions[1].position);
        for (int i = 0; i < targetPositions.Count; ++i)
        {
            Debug.Log(targetPositions[i].position);
            localTargetPositions.Add((Vector2)targetPositions[i].position);
        }

        //Debug.Log(targetPositions[positionIndex].position);

        // Set it to the very first position
        // if (targetPosition.Count > 0)
        //    currentTargetPosition = targetPosition[positionIndex].position;
        //else
        //    currentTargetPosition = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= delayTime)
        {
            moveTile = true;
            // Trigger the move
            elapsedTime = 0;
        }
    }

    private void FixedUpdate()
    {
        if (moveTile == true)
        {
            if (positionIndex < localTargetPositions.Count)
                //Debug.Log(targetPositions[positionIndex].position);
                transform.position = Vector3.MoveTowards(transform.position, nextPos, tileSpeed * Time.fixedDeltaTime);
               //transform.position = Vector2.Lerp(transform.position, currentTargetPosition, tileSpeed * Time.fixedDeltaTime);
               
            // Might have issues where it will not reach the same position
            if ((Vector2)transform.position == (Vector2)nextPos)
            {
                // Set the next position
                GetNextPosition();

                moveTile = false;
            }
        }
    }

    void GetNextPosition()
    {
        //Debug.Log("After_0" + localTargetPositions[0].position);
        //Debug.Log("After_1" + localTargetPositions[1].position);
        // Check if it will overshoot the list
        if (positionIndex + 1 < localTargetPositions.Count && backTravel == false)
        {
            // If its okay, keep increasing to the next target
            positionIndex++;
            nextPos = localTargetPositions[positionIndex];
        }
        // If it cannot go any higher, means it is at the end of the list
        // Hence, check if it is the only position, if it isnt then go back down the list
        else if (positionIndex > 0)
        {
            positionIndex--;
            nextPos = localTargetPositions[positionIndex];
            backTravel = true;
        }
        else
        {
            backTravel = false;
            positionIndex++;
            nextPos = localTargetPositions[positionIndex];
            // Debug.LogError("It shouldn't be here");
        }

    }
}
