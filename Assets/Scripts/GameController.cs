using UnityEngine;

public enum GameState {
    Overworld,
    Menu,
    Battle
}

public class GameController : MonoBehaviour {

    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private BattleSystem battleSystem;

    private GameState state;

    private void Start() {
        playerController.OnEncounter += StartBattle;
        playerController.OpenMenu += OpenMenu;
        mainMenu.CloseMenu += CloseMenu;
        battleSystem.OnBattleEnded += EndBattle;
    }
    
    private void Update() {
        switch (state) {
            case GameState.Overworld:
                playerController.HandleUpdate(); //todo Update() is only called if an object is active. Deactivate the correct objects?
                break;
            case GameState.Menu:
                mainMenu.HandleUpdate();
                break;
            case GameState.Battle:
                battleSystem.HandleUpdate();
                break;
        }
    }

    private void OpenMenu() {
        state = GameState.Menu;
        
        mainCamera.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

    private void CloseMenu() {
        state = GameState.Overworld;
        mainCamera.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(false);
    }

    private void StartBattle() {
        state = GameState.Battle;
        
        mainCamera.gameObject.SetActive(false);
        battleSystem.gameObject.SetActive(true);

        var enemies = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomEnemyParty();

        battleSystem.StartBattle(enemies);
    }

    private void EndBattle(bool playerWon) {
        //todo check results
        state = GameState.Overworld;
        
        mainCamera.gameObject.SetActive(true);
        battleSystem.gameObject.SetActive(false);
    }
}
