using UnityEngine;

public class MenuOption : MonoBehaviour {
    [SerializeField] private GameObject arrow;

    public void SetSelected(bool selected) {
        arrow.SetActive(selected);
    }
}