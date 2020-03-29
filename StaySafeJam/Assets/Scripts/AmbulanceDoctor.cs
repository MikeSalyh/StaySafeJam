using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AmbulanceDoctor : Doctor
{
  protected override void EnterBuilding(float duration)
  {
  }

  protected override void ExitBuilding(float duration)
  {
  }

  public override bool Appear(float time)
  {
    if (base.Appear(time))
    {
      transform.localPosition = new Vector2(hiddenX, transform.localPosition.y);
      transform.DOLocalMoveX(shownX, time * 1.25f).SetEase(Ease.Linear);
      return true;
    }
    return false;
  }

  override protected void DebugAppear()
  {
    Appear(4f);
  }
}
