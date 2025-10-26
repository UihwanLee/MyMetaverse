using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.transform.GetComponent<PlayerController>();
        if(player != null)
        {

            Destroy(this.gameObject);
        }
    }
}
