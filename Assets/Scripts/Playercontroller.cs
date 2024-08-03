using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Playercontroller : MonoBehaviour
{
    public GameObject bulletprefabs;
    public float horizontalinput;
    public float speed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -5)
        {
            transform.position = new Vector3(-5, transform.position.y, transform.position.z);
        }
        if (transform.position.x > 5)
        {
            transform.position = new Vector3(5, transform.position.y, transform.position.z);
        }
        horizontalinput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * horizontalinput * Time.deltaTime * speed);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bulletprefabs, transform.position, bulletprefabs.transform.rotation);
        }
    }
}
