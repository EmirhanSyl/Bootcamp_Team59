using UnityEngine;

public class DragClick : MonoBehaviour
{
    Camera _cam;

    [SerializeField] RectTransform _selectionBoxVisual;

    Rect _boxSelection;

    Vector2 _startPosition;
    Vector2 _endPosition;
    private void Start()
    {
        _cam = Camera.main;
        _startPosition = Vector2.zero;
        _endPosition = Vector2.zero;
        DrawVisual();
    }

    private void Update()
    {
        // týklayýnca
        if (Input.GetMouseButtonDown(2))
        {
            _startPosition = Input.mousePosition;
            _boxSelection = new Rect();
        }

        // sürüklerken
        if (Input.GetMouseButton(2))
        {
            _endPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        if (Input.GetMouseButtonUp(2))
        {
            _startPosition = Vector2.zero;
            _endPosition = Vector2.zero;
            DrawVisual();
            UnitSelection();
        }
    }

    void DrawVisual()
    {
        Vector2 boxStart = _startPosition;
        Vector2 boxEnd = _endPosition;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        _selectionBoxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        _selectionBoxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        if (Input.mousePosition.x < _startPosition.x)
        {
            //yeþil seçme þeyi sola çekilince x'i deðiþtiriyor
            _boxSelection.xMin = Input.mousePosition.x;
            _boxSelection.xMax = _startPosition.x;
        }
        else
        {
            //bu da saga alýyor
            _boxSelection.xMin = _startPosition.x;
            _boxSelection.xMax = Input.mousePosition.x;
        }

        if (Input.mousePosition.y < _startPosition.y)
        {
            // asagi
            _boxSelection.yMin = Input.mousePosition.y;
            _boxSelection.yMax = _startPosition.y;
        }
        else
        {
            //yukari
            _boxSelection.yMin = _startPosition.y;
            _boxSelection.yMax = Input.mousePosition.y;
        }
    }
     
    void UnitSelection()
    {
        foreach (var unit in UnitSelections.Instance._unitList)
        {
            if (_boxSelection.Contains(Camera.main.WorldToScreenPoint(unit.transform.position)))
            {
                UnitSelections.Instance.DragClickSelect(unit);
            }
        }
    }
}
