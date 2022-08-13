using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Coin")) {
            Debug.Log("Coin trapped in magnetic field");

            //pull object to this object
        }
    }
}
