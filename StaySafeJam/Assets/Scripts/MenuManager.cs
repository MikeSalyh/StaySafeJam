using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    MetagameManager.SwitchState(MetagameManager.GameState.Menu);   
  }

  public void HandleStartClicked()
  {
    MetagameManager.GoToGameplay();
  }
}
