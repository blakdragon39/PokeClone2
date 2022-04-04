using System.Collections.Generic;
using UnityEngine;

public class MenuOptions : MonoBehaviour {

    public List<MenuOption> Options => options;
    public int SelectedOption { get; private set; }
    
    [SerializeField] private bool verticalSelection;
    [SerializeField] private bool horizontalSelection;
    
    [SerializeField] private List<MenuOption> options;
    [SerializeField] private GameObject optionPrefab;

    public void Init(List<string> newOptions) { //todo map of strings to actions?
        options = new List<MenuOption>();

        newOptions.ForEach(newOption => {
            var prefab = Instantiate(optionPrefab);
            var option = prefab.GetComponent<MenuOption>();
            
            option.Init(newOption);
            prefab.SetActive(true);
            prefab.transform.SetParent(transform, false);

            options.Add(option);
        });

        SetSelectedOption(0);
    }

    public void SetSelectedOption(int index) {
        SelectedOption = index;
        for (int i = 0; i < options.Count; i++) {
            options[i].SetSelected(i == index);
        }
    }

    private void Update() {
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