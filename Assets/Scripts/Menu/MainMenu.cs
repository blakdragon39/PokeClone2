using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public event Action CloseMenu;
    
    public void HandleUpdate() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            CloseMenu();
        }
    }
}
