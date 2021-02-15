using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimRotation : MonoBehaviour
{
    [SerializeField] private float speed;
    
    void Update()
    {
        this.transform.Rotate(Vector3.forward, this.speed * Time.deltaTime);
    }
}
