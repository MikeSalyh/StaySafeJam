using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetagameManager : MonoBehaviour
{
  private static MetagameManager _instance;
  public static MetagameManager Instance
  {
    get
    {
      if (_instance == null)
        _instance = Instantiate(Resources.Load("MetagameManager") as GameObject).GetComponent<MetagameManager>();
      return _instance;
    }
  }

  public enum GameState
  {
    Init,
    Menu,
    Gameplay,
    Finale
  }

  [SerializeField]
  private GameState _currentState;
  public static GameState CurrentState
  {
    get { return Instance._currentState; }
    set { SwitchState(value); }
  }

  public static void SwitchState(GameState value)
  {
    if (Instance._currentState == value)  //If it's already in this state, do nothing.
      return;

    Instance._currentState = value;
    switch (value)
    {
      case GameState.Menu:
        break;
      case GameState.Gameplay:
        break;
      case GameState.Finale:
        break;
    }
  }

  // Start is called before the first frame update
  void Start()
  {
    if(_instance != this)
    {
      Destroy(this.gameObject);
      return;
    }
    _instance = this;
      DontDestroyOnLoad(this);
  }

  public static void GoToGameplay()
  {
    SceneManager.LoadScene("Gameplay");
    SwitchState(GameState.Gameplay);
  }

  public static void GoToMenu()
  {
    SceneManager.LoadScene("Menu");
    SwitchState(GameState.Menu);
  }

  public static void GoToFinale()
  {
    SceneManager.LoadScene("Finale");
    SwitchState(GameState.Finale);
  }
}
