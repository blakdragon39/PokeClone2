using UnityEngine;

public class BattleOption : MonoBehaviour {
    [SerializeField] private GameObject arrow;

    public void SetSelected(bool selected) {
        arrow.SetActive(selected);
    }
}