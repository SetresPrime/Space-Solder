using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireButton : MonoBehaviour
{
    bool _isFire;
    private void Update()
    {
        if (_isFire)
            Debug.Log("Fire!");
    }
    public void ButtonDown()
    {
        _isFire = true;
    }
    public void ButtonUp()
    {
        _isFire = false;
    }
}
