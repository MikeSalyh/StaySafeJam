using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  [Header("Game Balancing Vars")]

  public int startingHP = 3;
  private int _remainingHP;
  public int HP
  {
    get { return _remainingHP; }
  }

  private int _score = 0;
  public int Score
  {
    get { return _score; }
  }

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

  // Update is called once per frame
  void Update()
  {
        
  }

  void HandleGiveMask(Doctor md)
  {
    _score += md.pointValue;
    Debug.Log("Gave a dr mask: " + md.pointValue);
  }

  void HandleFail()
  {
    _remainingHP--;
    if (_remainingHP == 0)
      FinishGameplay();
  }

  public void FinishGameplay()
  {
    MetagameManager.GoToFinale();
  }
}
