using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowyScript : MonoBehaviour {
    public Color[] colors;
    [HideInInspector]
    public bool fadeOut = false;
    private SpriteRenderer sr;
    private float rate;
    private float one = 1;
    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        int i = Random.Range(0, colors.Length);
        sr.color = new Color(colors[i].r, colors[i].g, colors[i].b, 1);
        rate = Random.Range(0.00075f, 0.015f);
        rate *= -1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!fadeOut)
        {
            one += rate;
            if (one <= 0 || one >= 1)
            {
                rate *= -1;
            }
            float currVal = Mathf.Lerp(1, 0.25f, one);

            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, currVal);
        }
        else
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a - 0.01f);
            if(sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
        
    }
}
