using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmBeat : MonoBehaviour
{
    float t;
    public Vector3 startPosition;
    public Vector3 target;
    public float timeToReachTarget;
    private Stack<GameObject> stack;

    void Start()
    {
       // startPosition = target = transform.position;
    }

    void Update()
    {
        t += Time.deltaTime / timeToReachTarget;
        if (t <= 1)
        {
            transform.localPosition = Vector3.Lerp(startPosition, target, t);
        }else
        {
            transform.localPosition = Vector3.Lerp(target, target + Vector3.down * 10, (t - 1));

            if (t > 1.5)
            {
                KillButton();
            }
        }


    }

    //private void OnBecameInvisible()
    //{
    //    if (t > 1)
    //        KillButton();
    //}

    public void Setup(Vector3 destination, float time, Stack<GameObject> stack)
    {
        t = 0;
        startPosition = transform.position;
        timeToReachTarget = time;
        target = destination;
        Debug.Log(target);
        this.stack = stack;

    }

    public float KillButton()
    {
        stack.Pop();
        Destroy(gameObject);
        return 10f;
    }


    
}



