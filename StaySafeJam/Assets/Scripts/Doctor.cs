using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Doctor : MonoBehaviour
{
  public enum State
  {
    Hidden,
    Ready,
    Happy,
    Sad
  }

  public State state;
  public Sprite waitingSprite, happySprite, sadSprite;
  public GameObject door;
  private float timeRemaining;
  public Image doctorImage;

  public float hiddenX, shownX;

  // Start is called before the first frame update
  void Start()
  {
    state = State.Hidden;
    door.gameObject.SetActive(true);
    transform.localPosition = new Vector3(hiddenX, transform.localPosition.y);
  }


  public KeyCode debugKey;
  private void Update()
  {
    if (Input.GetKeyDown(debugKey))
    {
      if (Input.GetKey(KeyCode.LeftShift))
        GiveMask();
      else
        Appear(2f);
    }
  }

  public void Appear(float time)
  {
    if (state != State.Hidden)
      return;

    door.gameObject.SetActive(false);
    timeRemaining = time;
    state = State.Ready;
    ExitBuilding(0.5f);
    doctorImage.sprite = waitingSprite;
    StartCoroutine(AwaitMaskCoroutine());
  }

  IEnumerator AwaitMaskCoroutine()
  {
    while (timeRemaining > 0f)
    {
      timeRemaining -= 0.05f;
      yield return new WaitForSeconds(0.05f);
    }

    //The player has failed to give this doctor a mask
    if (state == State.Ready)
    {
      state = State.Sad;
      doctorImage.sprite = sadSprite;
    }

    yield return new WaitForSeconds(1f);
    EnterBuilding(0.5f);
    yield return new WaitForSeconds(0.5f);
    state = State.Hidden;
    door.gameObject.SetActive(true);
  }

  public void GiveMask()
  {
    if (state != State.Ready)
      return;

    state = State.Happy;
    doctorImage.sprite = happySprite;
    timeRemaining = 0;
  }

  void EnterBuilding(float duration)
  {
    doctorImage.transform.DOLocalMoveX(hiddenX, duration);
  }

  void ExitBuilding(float duration)
  {
    doctorImage.transform.DOLocalMoveX(shownX, duration);
  }
}
