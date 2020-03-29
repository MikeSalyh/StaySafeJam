using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
  [Header("Game Balancing Vars")]

  public int startingHP = 3;
  private int _remainingHP;
  public int HP
  {
    get { return _remainingHP; }
  }

  //Delegates
  public delegate void StartDragDelegate();
  public StartDragDelegate OnStartDrag;
  public delegate void StopDragDelegate();
  public StopDragDelegate OnStopDrag;
  private bool _isDragging = false;

  // Start is called before the first frame update
  void Start()
  {
    MetagameManager.SwitchState(MetagameManager.GameState.Gameplay);

    _remainingHP = startingHP;

    foreach (Doctor md in GameObject.FindObjectsOfType<Doctor>())
    {
      md.OnGiveMask += HandleGiveMask;
      md.OnFail += HandleFail;
    }
  }

  void HandleGiveMask(Doctor md)
  {
    MetagameManager.score += md.pointValue;
  }

  void HandleFail(Doctor md)
  {
    if (md.givesPenalty)
    {
      _remainingHP--;
      if (_remainingHP == 0)
        FinishGameplay();
    }
  }

  public void FinishGameplay()
  {
    MetagameManager.GoToFinale();
  }
}
