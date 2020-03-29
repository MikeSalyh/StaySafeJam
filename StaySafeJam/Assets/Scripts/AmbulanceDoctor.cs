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
      transform.DOLocalMoveX(shownX * 0.9f, time * 1.25f).SetEase(Ease.Linear);
      return true;
    }
    return false;
  }

  override protected void DebugAppear()
  {
    Appear(4f);
  }

  public override void ForceHide()
  {
    base.ForceHide();
    transform.DOKill();
    transform.localPosition = new Vector2(hiddenX, transform.localPosition.y);
  }

  override protected IEnumerator GoInside()
  {
    transform.DOKill();
    yield return new WaitForSeconds(0.05f);
    transform.DOLocalMoveX(shownX, 1f).SetEase(Ease.InQuint);
    yield return new WaitForSeconds(1f);
    state = State.Hidden;
  }

  protected override IEnumerator HandleMissBlinking()
  {
    transform.DOKill();
    transform.localPosition = new Vector2(shownX * 0.9f, transform.localPosition.y);
    for (int i = 0; i < 8; i++)
    {
      GetComponent<Image>().enabled = !GetComponent<Image>().enabled;
      yield return new WaitForSeconds(0.2f);
    }
    transform.localPosition = new Vector2(shownX, transform.localPosition.y);
  }
}
