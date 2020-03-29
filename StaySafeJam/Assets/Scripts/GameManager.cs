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
    set { _remainingHP = value; }
  }

  public static bool active = true;
  public N95Mask n95;

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Alpha0))
    {
      active = !active;
      n95.active = active;
    }
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

  void HandleGiveMask(Doctor md)
  {
    MetagameManager.score += md.pointValue;
  }

  void HandleFail(Doctor md)
  {
    if (md.givesPenalty)
    {
      StartCoroutine(HandleFailCoroutine(md));
    }
  }

  private IEnumerator HandleFailCoroutine(Doctor md)
  {
    foreach (Doctor doc in GameObject.FindObjectsOfType<Doctor>())
    {
      if (doc != md)
      {
        doc.ForceHide();
      }
    }
    _remainingHP--;
    active = false;
    n95.active = false;
    yield return new WaitForSeconds(2f);
    if (_remainingHP == 0)
    {
      FinishGameplay();
    }
    else
    {
      active = true;
      n95.active = true;
    }
  }

  public void FinishGameplay()
  {
    MetagameManager.GoToFinale();
  }
}
