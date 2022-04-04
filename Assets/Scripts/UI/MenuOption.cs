using UnityEngine;
using UnityEngine.UI;

public class MenuOption : MonoBehaviour {

    [SerializeField] private Text textField;
    [SerializeField] private GameObject arrow;

    public void Init(string text) {
        textField.text = text;
    }
    
    public void SetSelected(bool selected) {
        arrow.SetActive(selected);
    }

}