using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public event Action CloseMenu;

    private MenuOptions options;

    private List<KeyValuePair<string, Action>> menu = new List<KeyValuePair<string, Action>>();

    private void Start() {
        options = GetComponentInChildren<MenuOptions>();
        menu.Add(new KeyValuePair<string, Action>("Moves", OpenMoves));
        menu.Add(new KeyValuePair<string, Action>("Exit", CloseMenu));
        options.Init(menu.ConvertAll(m => m.Key));
    }
    
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            CloseMenu();
        } else if (Input.GetKey(KeyCode.Return)) {
            menu[options.SelectedOption].Value();
        }
    }

    private void OpenMoves() {
        
    }
}
