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

  //Doctor delegates
  public delegate void GiveMaskDelegate(Doctor md);
  public GiveMaskDelegate OnGiveMask;
  public delegate void FailDelegate();
  public FailDelegate OnFail;

  // Start is called before the first frame update
  void Start()
  {
    state = State.Hidden;
    door.gameObject.SetActive(true);
    patienceSlider.GetComponent<CanvasGroup>().alpha = 0f;
    transform.localPosition = new Vector3(hiddenX, transform.localPosition.y);
  }


  public KeyCode debugKey;
  private void Update()
  {
    if (state == State.Ready)
    {
      patienceSlider.value = patienceRemaining / patienceMax;
    }

    //DEBUG
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

    patienceMax = time;
    patienceRemaining = time;

    state = State.Ready;
    ExitBuilding(0.5f);
    door.gameObject.SetActive(false);
    doctorImage.sprite = waitingSprite;
    StartCoroutine(AwaitMaskCoroutine());

    patienceSlider.GetComponent<CanvasGroup>().DOFade(1f, 0.5f);
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
      patienceSlider.GetComponent<CanvasGroup>().DOFade(0f, 0.25f);
      doctorImage.sprite = sadSprite;

      if (OnFail != null)
        OnFail();
    }

    yield return new WaitForSeconds(0.5f);
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
    patienceSlider.GetComponent<CanvasGroup>().DOFade(0f, 0.25f);
    doctorImage.sprite = happySprite;
    patienceRemaining = 0;

    if (OnGiveMask != null)
      OnGiveMask(this);
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
