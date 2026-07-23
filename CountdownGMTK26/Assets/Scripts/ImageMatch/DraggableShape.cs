using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableShape :
    MonoBehaviour,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [SerializeField] private Image image;

    public RectTransform RectTransform { get; private set; }

    private RectTransform movementRegion;
    private Canvas rootCanvas;
    private bool canDrag;

    private Vector2 pointerOffset;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();

        if (image == null)
            image = GetComponent<Image>();

        rootCanvas = GetComponentInParent<Canvas>();

        if (rootCanvas != null)
            rootCanvas = rootCanvas.rootCanvas;
    }

    public void SetMovementRegion(RectTransform region)
    {
        movementRegion = region;
    }

    public void SetDraggable(bool value)
    {
        canDrag = value;
    }

    public void SetColor(Color color)
    {
        if (image != null)
            image.color = color;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDrag || movementRegion == null)
            return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                movementRegion,
                eventData.position,
                GetEventCamera(eventData),
                out Vector2 localPointerPosition))
        {
            pointerOffset =
                RectTransform.anchoredPosition - localPointerPosition;
        }

        // Move this shape in front of the others while dragging.
        RectTransform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag || movementRegion == null)
            return;

        bool foundPosition =
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                movementRegion,
                eventData.position,
                GetEventCamera(eventData),
                out Vector2 localPointerPosition
            );

        if (!foundPosition)
            return;

        Vector2 desiredPosition =
            localPointerPosition + pointerOffset;

        RectTransform.anchoredPosition =
            ClampPositionToRegion(desiredPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Nothing is required here yet, but this gives us a place
        // to add snapping or sound effects later.
    }

    private Vector2 ClampPositionToRegion(Vector2 position)
    {
        Rect regionRect = movementRegion.rect;

        float halfWidth = RectTransform.rect.width * 0.5f;
        float halfHeight = RectTransform.rect.height * 0.5f;

        position.x = Mathf.Clamp(
            position.x,
            regionRect.xMin + halfWidth,
            regionRect.xMax - halfWidth
        );

        position.y = Mathf.Clamp(
            position.y,
            regionRect.yMin + halfHeight,
            regionRect.yMax - halfHeight
        );

        return position;
    }

    private Camera GetEventCamera(PointerEventData eventData)
    {
        if (rootCanvas == null)
            return eventData.pressEventCamera;

        if (rootCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            return null;

        return rootCanvas.worldCamera;
    }
}