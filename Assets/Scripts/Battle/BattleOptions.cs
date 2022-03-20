using System.Collections.Generic;
using UnityEngine;

public class BattleOptions : MonoBehaviour {
    
    public List<BattleOption> options;

    public void SetSelectedOption(int index) {
        for (int i = 0; i < options.Count; i++) {
            options[i].SetSelected(i == index);
        }
    }
}