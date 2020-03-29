using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinaleManager : MonoBehaviour
{
  public TextMeshProUGUI finalScoreText;

  // Start is called before the first frame update
  void Start()
  {
    MetagameManager.SwitchState(MetagameManager.GameState.Finale);
    finalScoreText.text = "Your Score:\n"+MetagameManager.score;
  }

  public void ReturnToMenu()
  {
    MetagameManager.GoToMenu();
  }
}
