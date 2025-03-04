using FortunePlayingCards.Scripts.Component;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public string FaceName { get; set; }
    public int FaceValue { get; set; }
    public string FaceType { get; set; }

    [SerializeField]
    private MeshRenderer cardFrontRenderer;

    private Sprite cardFront;
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private Vector2 _originalLocalPointerPosition;

    
    private Transform _targetTransform;
    private float _positionDamp = .2f;
    private float _rotationDamp = .2f;
    private Vector3 _smoothVelocity;
    private Vector4 _smoothRotationVelocity;

    private Vector3 _dragOffset;
    
    public CardSlot ParentCardSlot { get; set; }
    public CardSlotEleven ParentCardSlotEleven { get; set; }

    public Transform TargetTransform
    {
        get
        {
            if (_targetTransform == null)
            {
                var targetObj = new GameObject($"{name}Target");
                _targetTransform = targetObj.transform;
                _targetTransform.position = transform.position;
                _targetTransform.forward = transform.forward;
            }
            return _targetTransform;
        }
    }
    
    private void Update()
    {
        SmoothToTargetPositionRotation();
    }

    public void SetDamp(float newPositionDamp, float newRotationDamp)
    {
        _positionDamp = newPositionDamp;
        _rotationDamp = newRotationDamp;
    }

    private void SmoothToTargetPositionRotation()
    {
        if (TargetTransform.position != transform.position || TargetTransform.eulerAngles != transform.eulerAngles)
        {
            SmoothToPointAndDirection(TargetTransform.position, _positionDamp, TargetTransform.rotation, _rotationDamp);
        }
    }

    private void SmoothToPointAndDirection(Vector3 point, float moveSmooth, Quaternion rotation, float rotSmooth)
    {
        transform.position = Vector3.SmoothDamp(transform.position, point, ref _smoothVelocity, moveSmooth);

        Quaternion newRotation = new Quaternion();
        newRotation.x = Mathf.SmoothDamp(transform.rotation.x, rotation.x, ref _smoothRotationVelocity.x, rotSmooth);
        newRotation.y = Mathf.SmoothDamp(transform.rotation.y, rotation.y, ref _smoothRotationVelocity.y, rotSmooth);
        newRotation.z = Mathf.SmoothDamp(transform.rotation.z, rotation.z, ref _smoothRotationVelocity.z, rotSmooth);
        newRotation.w = Mathf.SmoothDamp(transform.rotation.w, rotation.w, ref _smoothRotationVelocity.w, rotSmooth);
        transform.rotation = newRotation;

        TestVisibility();
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = FindObjectOfType<Canvas>();
    }

    private void TestVisibility()
    {
        float angle = Vector3.Angle(Camera.main.transform.forward, transform.forward);
        if (angle < 90)
        {
            FrontBecameVisible();
        }
        else
        {
            FrontBecameHidden();
        }
    }

    private void FrontBecameVisible()
    {
        cardFrontRenderer.enabled = true;
    }

    private void FrontBecameHidden()
    {
        cardFrontRenderer.enabled = false;
    }

    public void SetCardFrontSprite(Sprite frontSprite)
    {
        cardFront = frontSprite;
        Texture2D texture = frontSprite.texture;

        Material material = new Material(Shader.Find("Standard"));
        material.mainTexture = texture;
        cardFrontRenderer.material = material;

        cardFrontRenderer.enabled = true;
    }

        public void DestroyTargetObject()
    {
        Destroy(TargetTransform.gameObject);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.GetComponent<RectTransform>(),
            eventData.position,
            _canvas.worldCamera,
            out _originalLocalPointerPosition
        );
    } 

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvas.GetComponent<RectTransform>(),
            eventData.position,
            _canvas.worldCamera,
            out Vector2 localPointerPosition))
        {
            Vector2 offset = localPointerPosition - _originalLocalPointerPosition;
            _rectTransform.localPosition += (Vector3)offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var dealer = FindObjectOfType<DealerEleven>();
        if (dealer != null && dealer._currentCardSlot != null)
        {
            dealer._currentCardSlot.AddCard(this);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Optional highlight logic
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Optional un-highlight logic
    }

}