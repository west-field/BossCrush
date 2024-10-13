using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private CSharpEventExample example;

    private void FixedUpdate()
    {
        if (example.IsMove())
        {
            this.transform.position += example.GetVelocity() * Time.deltaTime * 2.0f;
        }
    }
}
