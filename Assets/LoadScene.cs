using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public int buildIndex = 1;
    public float loadDelay = 15f;
    private float countdown = 0f;
    // Start is called before the first frame update
    void Start()
    {
        countdown = loadDelay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0) {
            SceneManager.LoadScene(buildIndex);
        }
    }
}
