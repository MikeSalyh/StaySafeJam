using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
  public Image doctorImage;

  public float hiddenX, shownX;

  private float patienceMax;
  private float patienceRemaining;
  public Slider patienceSlider;

  public int pointValue = 100;
  public bool givesPenalty = true;

  //Doctor delegates
  public delegate void GiveMaskDelegate(Doctor md);
  public GiveMaskDelegate OnGiveMask;
  public delegate void FailDelegate(Doctor md);
  public FailDelegate OnFail;

  // Start is called before the first frame update
  void Start()
  {
    state = State.Hidden;
    if(door != null)
      door.gameObject.SetActive(true);

    if (patienceSlider != null)
      patienceSlider.GetComponent<CanvasGroup>().alpha = 0f;
    EnterBuilding(0f);
  }


  public KeyCode debugKey;
  private void Update()
  {
    if (state == State.Ready)
    {
      if(patienceSlider != null)
        patienceSlider.value = patienceRemaining / patienceMax;
    }

    //DEBUG
    if (Input.GetKeyDown(debugKey))
    {
      if (Input.GetKey(KeyCode.LeftShift))
        GiveMask();
      else
        DebugAppear();
    }
  }

  virtual protected void DebugAppear()
  {
    Appear(2f);
  }

  virtual public bool Appear(float time)
  {
    if (state != State.Hidden)
      return false;

    patienceMax = time;
    patienceRemaining = time;

    state = State.Ready;
    ExitBuilding(0.5f);
    if (door != null)
      door.gameObject.SetActive(false);
    doctorImage.sprite = waitingSprite;
    StartCoroutine(AwaitMaskCoroutine());

    if (patienceSlider != null)
      patienceSlider.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
    return true;
  }

  IEnumerator AwaitMaskCoroutine()
  {
    while (patienceRemaining > 0f)
    {
      patienceRemaining -= 0.05f;
      yield return new WaitForSeconds(0.05f);
    }

    //The player has failed to give this doctor a mask
    if (state == State.Ready)
    {
      state = State.Sad;
      if (patienceSlider != null)
        patienceSlider.GetComponent<CanvasGroup>().DOFade(0f, 0.25f);
      doctorImage.sprite = sadSprite;

      if (OnFail != null)
        OnFail(this);
    }

    yield return new WaitForSeconds(0.5f);
    EnterBuilding(0.5f);
    yield return new WaitForSeconds(0.5f);
    state = State.Hidden;

    if (door != null)
      door.gameObject.SetActive(true);
  }

  public void GiveMask()
  {
    if (state != State.Ready)
      return;

    state = State.Happy;

    if (patienceSlider != null)
      patienceSlider.GetComponent<CanvasGroup>().DOFade(0f, 0.25f);
    doctorImage.sprite = happySprite;
    patienceRemaining = 0;

    if (OnGiveMask != null)
      OnGiveMask(this);
  }

  protected virtual void EnterBuilding(float duration)
  {
    doctorImage.transform.DOLocalMoveX(hiddenX, duration);
  }

  protected virtual void ExitBuilding(float duration)
  {
    doctorImage.transform.DOLocalMoveX(shownX, duration);
  }
}
