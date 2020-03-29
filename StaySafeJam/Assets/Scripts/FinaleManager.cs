using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinaleManager : MonoBehaviour
{
  public TextMeshProUGUI finalScoreText;
  private AudioSource src;

  // Start is called before the first frame update
  void Start()
  {
    MetagameManager.SwitchState(MetagameManager.GameState.Finale);
    finalScoreText.text = "Your Score\n<size=2em>" + string.Format("{0:n0}", MetagameManager.score) + "</size>";
    src = GetComponent<AudioSource>();
    Invoke("DoSFX", 0.5f);
  }

  private void DoSFX()
  {
    src.Play();
  }

  public void ReturnToMenu()
  {
    CancelInvoke();
    MetagameManager.GoToMenu();
  }
}
