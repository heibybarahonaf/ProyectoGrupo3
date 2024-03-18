using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

    public Rigidbody platformRigidbody;
    public Transform[] platformPositions;
    public float platformSpeed;

    public int actualPosition = 0;
    public int nextPosition = 1;

    // public bool rotateOn = true;
    public bool moveToTheNext = true;
    public float waitTime = 3;

    void FixedUpdate()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        if (moveToTheNext)
        {
            StopCoroutine(WaitForMove(0));
            platformRigidbody.MovePosition(Vector3.MoveTowards(platformRigidbody.position, platformPositions[nextPosition].position, platformSpeed * Time.deltaTime));

            // Quaternion targetRotation = Quaternion.LookRotation(platformPositions[nextPosition].position - platformRigidbody.position, Vector3.up);
            // platformRigidbody.MoveRotation(Quaternion.RotateTowards(platformRigidbody.rotation, targetRotation, platformSpeed * Time.fixedDeltaTime));
        }

        if (Vector3.Distance(platformRigidbody.position, platformPositions[nextPosition].position) <= 0)
        {
            StartCoroutine(WaitForMove(waitTime));
            actualPosition = nextPosition;
            nextPosition++;

            if (nextPosition > platformPositions.Length - 1)
                nextPosition = 0;
        }
    }

    private IEnumerator WaitForMove(float time)
    {
        moveToTheNext = false;
        yield return new WaitForSeconds(time);
        moveToTheNext = true;
    }

}
