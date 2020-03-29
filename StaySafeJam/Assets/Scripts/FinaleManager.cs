using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinaleManager : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    MetagameManager.SwitchState(MetagameManager.GameState.Finale);

  }

  public void ReturnToMenu()
  {
    MetagameManager.GoToMenu();
  }
}
