using System.Collections.Generic;
using UnityEngine;

public class BattleOptions : MonoBehaviour {

    public int SelectedOption { get; private set; }
    
    public List<BattleOption> options;

    [SerializeField] private bool verticalSelection;
    [SerializeField] private bool horizontalSelection;

    public void SetSelectedOption(int index) {
        SelectedOption = index;
        for (int i = 0; i < options.Count; i++) {
            options[i].SetSelected(i == index);
        }
    }

    private void Update() {
        if (!gameObject.activeSelf) return; //todo is this necessary?

        var newSelection = SelectedOption;
        if (
            verticalSelection && Input.GetKeyDown(KeyCode.DownArrow) || 
            horizontalSelection && Input.GetKeyDown(KeyCode.RightArrow)
        ) {
            newSelection += 1;
        } else if (
            verticalSelection && Input.GetKeyDown(KeyCode.UpArrow) || 
            horizontalSelection && Input.GetKeyDown(KeyCode.LeftArrow)
        ) {
            newSelection -= 1;
        }

        Mathf.Clamp(newSelection, 0, options.Count - 1);
        
        SetSelectedOption(newSelection);
    }
}