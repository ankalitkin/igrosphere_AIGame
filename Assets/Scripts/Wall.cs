﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.GetComponent<Mob>() == null)
            return;
        Vector3 direction = Vector3.Reflect(other.transform.forward, other.GetContact(0).normal);
        direction.y = 0;
        other.transform.forward = direction;
    }
}