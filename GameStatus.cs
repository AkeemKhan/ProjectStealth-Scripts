using UnityEngine;
using System.Collections;

public static class GameStatus {

    public enum GameState
    {
        Normal,
        Caution,
        Alert
    };

    public static GameState currentState;
}
