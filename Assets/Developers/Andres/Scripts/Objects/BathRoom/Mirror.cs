using System.Collections;
using TMPro;
using UnityEngine;


public class Mirror : MonoBehaviour, IInteractable
{
    [Header("Fog Settings")]
    [SerializeField] private float _TransitionSpeed = 1f;
    [SerializeField] private float _Cooldown = 2f;
    [SerializeField] private float _MaxAlphaValue = 2f;
    [SerializeField] private TMP_Text _Text;

    private EventSystemBathroom _EventSystemBathroom;
    private InteractionDetector _InteractionDetector;
    private bool _canInteract;

    private void Start()
    {
        _EventSystemBathroom = EventSystemBathroom.instance;

        _EventSystemBathroom.onTurnOnShower += () => StartCoroutine(AppearText());

        _EventSystemBathroom.onLockDoor += () => ChangeText("Pick scissors");
        _EventSystemBathroom.onTakeScissors += () => ChangeText("Interact with mirror");
        _EventSystemBathroom.onCutHair += () => ChangeText("");

        _InteractionDetector = FindFirstObjectByType<InteractionDetector>(FindObjectsInactive.Include);
    }


    private void OnEnable() {
	    EventSystemController.Instance.onItemPicked += OnItemPicked;
	    EventSystemController.Instance.onItemDropped += OnItemDropped;
    }


    private void OnDisable() {
	    EventSystemController.Instance.onItemPicked -= OnItemPicked;
	    EventSystemController.Instance.onItemDropped -= OnItemDropped;
    }


    private void OnItemPicked(GameObject obj) {
	    if (!obj.TryGetComponent(out Holdable holdable)) return;
	    
	    if (holdable.HoldDefinition is SocketHoldDefinitionSO)
		    _canInteract = true;
    }
    
    
    private void OnItemDropped(GameObject obj) {
	    if (!obj.TryGetComponent(out Holdable holdable)) return;
	    
	    if (holdable.HoldDefinition is SocketHoldDefinitionSO)
		    _canInteract = false;
    }


    public bool CanInteract(HoldController holdController) {
	    return _canInteract;
    }

    public CrosshairType GetCrosshairType(HoldController holdController) {
	    return _canInteract ? CrosshairType.Interactable : CrosshairType.Default;
    }

    public void Interact()
    {
        _EventSystemBathroom.CutHair();
        Debug.Log("Start Cutscene");
    }

    private void ChangeText(string text)
    {
        _Text.alpha = 0;
        _Text.text = text;

        StartCoroutine(AppearText());
    }

    private IEnumerator AppearText()
    {
        yield return new WaitForSeconds(_Cooldown);

        float timeElapsed = 0f;

        while (timeElapsed < _TransitionSpeed)
        {
            float t = timeElapsed / _TransitionSpeed;
            t = Mathf.SmoothStep(0f, 1f, t);

            _Text.alpha = Mathf.Lerp(0, _MaxAlphaValue, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
