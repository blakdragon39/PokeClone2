using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogText : MonoBehaviour {

    [SerializeField] private int textSpeed;
    
    private Text dialog;
    
    private void Start() {
        dialog = GetComponent<Text>();
    }

    public void SetText(string text) {
        dialog.text = text;
    }

    public IEnumerator TypeText(string text) {
        dialog.text = "";
        
        foreach (var c in text.ToCharArray()) {
            dialog.text += c;
            yield return new WaitForSeconds(1f / textSpeed);
        }
    }
}
