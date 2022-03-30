using UnityEngine;

public enum GameState {
    Overworld,
    Battle
}

public class GameController : MonoBehaviour {

    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BattleSystem battleSystem;

    private GameState state;

    private void Start() {
        playerController.OnEncounter += StartBattle;
        battleSystem.OnBattleEnded += EndBattle;
    }
    
    private void Update() {
        switch (state) {
            case GameState.Overworld:
                playerController.HandleUpdate();
                break;
            case GameState.Battle:
                battleSystem.HandleUpdate();
                break;
        }
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
