using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class TestMoveScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject cart = GameObject.Find("Cart");
        TestMoveScript script = cart.GetComponent<TestMoveScript>();
        cart.name = "Cart Changed";
    }

    // Update is called once per frame
    void Update()
    {
            transform.position += transform.forward * 1f * Time.deltaTime;
    }

}
