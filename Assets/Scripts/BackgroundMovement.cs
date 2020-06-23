using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    Vector3 startPos;
    Vector3 endPos;

    [SerializeField]
    float speed;

    float startTime;
    float journeyLength;

    [SerializeField]
    Vector3 offset;

    void Start()
    {
        startPos = this.transform.position;

        endPos = startPos - offset;

        startTime = Time.time;
        journeyLength = Vector3.Distance(startPos, endPos);
    }

    // Update is called once per frame
    void Update()
    {
        // Distance moved equals elapsed time times speed..
        float distCovered = (Time.time - startTime) * speed;

        // Fraction of journey completed equals current distance divided by total distance.
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);

        if(distCovered >= journeyLength)
        {
            ResetPos();
        }
    }

    void ResetPos()
    {
        startTime = Time.time;
        this.transform.position = startPos;
    }
}
