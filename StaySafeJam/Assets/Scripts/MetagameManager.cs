using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MetagameManager : MonoBehaviour
{
  public AudioLowPassFilter musicFilter;
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
    Finale,
    Loading
  }

  public CanvasGroup dimPlane;


  public static int score = 0;

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
        DOTween.To(() => Instance.musicFilter.cutoffFrequency, x => Instance.musicFilter.cutoffFrequency = x, 1000f, 1f);
        break;
      case GameState.Gameplay:
        DOTween.To(() => Instance.musicFilter.cutoffFrequency, x => Instance.musicFilter.cutoffFrequency = x, 22000f, 1f);
        score = 0;
        break;
      case GameState.Finale:
        DOTween.To(() => Instance.musicFilter.cutoffFrequency, x => Instance.musicFilter.cutoffFrequency = x, 1000f, 1f);
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
    if (dimPlane.alpha > 0f)
      dimPlane.alpha = 0f;
  }

  public static void GoToGameplay()
  {
    if(CurrentState != GameState.Loading)
      Instance.StartCoroutine(Instance.GoToGameplayCoroutine());
  }

  private IEnumerator GoToGameplayCoroutine()
  {
    SwitchState(GameState.Loading);
    dimPlane.DOFade(1f, 0.5f);
    yield return new WaitForSeconds(0.5f);
    SceneManager.LoadScene("Gameplay");
    SwitchState(GameState.Gameplay);
    dimPlane.DOFade(0f, 0.5f);
  }

  public static void GoToMenu()
  {
    if (CurrentState != GameState.Loading)
      Instance.StartCoroutine(Instance.GoToMenuCoroutine());
  }

  private IEnumerator GoToMenuCoroutine()
  {
    SwitchState(GameState.Loading);
    dimPlane.DOFade(1f, 0.5f);
    yield return new WaitForSeconds(0.5f);
    SceneManager.LoadScene("Menu");
    SwitchState(GameState.Menu);
    dimPlane.DOFade(0f, 0.5f);
  }

  public static void GoToFinale()
  {
    if (CurrentState != GameState.Loading)
      Instance.StartCoroutine(Instance.GoToFinaleCoroutine());
  }

  private IEnumerator GoToFinaleCoroutine()
  {
    SwitchState(GameState.Loading);
    dimPlane.DOFade(1f, 0.5f);
    yield return new WaitForSeconds(0.5f);
    SceneManager.LoadScene("Finale");
    SwitchState(GameState.Finale);
    dimPlane.DOFade(0f, 0.5f);
  }
}
