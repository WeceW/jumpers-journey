using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Crawler : MonoBehaviour
{
    public float speed;
    public float delay;
    public bool stopOnScreen;

    bool crawling;
    float time;

    void Start()
    {
        crawling = true;
        time = 0f;
    }

    void Update()
    {
        if (!crawling || time < delay) {
            time += Time.deltaTime;
            return;
        }

        this.transform.Translate(Vector3.up * Time.deltaTime * speed);

        if (stopOnScreen && gameObject.transform.position.y > 0.1) {
            crawling = false;
        }
    }
}
