using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetScript : MonoBehaviour
{
    PlayerScript playerScript;
    private void Awake()
    {
        playerScript = gameObject.GetComponentInParent<PlayerScript>();
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
            playerScript.grounded = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
            playerScript.grounded = false;
    }
    private void Update()
    {
        //Debug.Log(playerScript.grounded);
    }
}
