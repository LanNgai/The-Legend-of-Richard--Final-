using System;
using UnityEngine;

public class PowerUpIndicatorScript : MonoBehaviour
{
    private GameObject player; 

    private void Start() {
        player = GameObject.FindWithTag("Player"); 
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        //update indicator position to be under player 
        transform.position = player.transform.position;// + -Vector3.up * 0.4f;

        //make the attached gameobject slowly rotate and pulsate in scale
        transform.Rotate(Vector3.up * Time.deltaTime * 30f);
        transform.localScale = Vector3.one * 5 * (Math.Clamp(Mathf.Sin(Time.time * 2f) + 1f, 1.8f, 2.5f));
    }
}
