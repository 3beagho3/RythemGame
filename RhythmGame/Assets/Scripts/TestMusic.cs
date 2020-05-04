using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class TestMusic : MonoBehaviour
{
    public Text tempText;
    public int waitTime = 3;

    private bool isMusicStart = false;

    void Start()
    {
        Invoke("MusicStart" , 3);
    }
    void MusicStart()
    {
        transform.GetComponent<AudioSource>().Play();
        isMusicStart = true;
    }
    void Update()
    {
        if (!isMusicStart) return;

        if (Input.GetMouseButtonDown(0))
        {
            tempText.text += (Time.time - waitTime).ToString("N3");
            tempText.text += ",";

            int tempRand = Random.Range(0, 4);
            if (tempRand.Equals(0))
                tempText.text += Direction.UP;
            else if (tempRand.Equals(1))
                tempText.text += Direction.RIGHT;
            else if (tempRand.Equals(2))
                tempText.text += Direction.DOWN;
            else if (tempRand.Equals(3))   
                tempText.text += Direction.LEFT;

            tempText.text += "\n";
        }
    }
}
