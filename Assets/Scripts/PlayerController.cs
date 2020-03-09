using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Astroline.Scripts
{
    public class PlayerController : MonoBehaviour
    , IDragHandler, IBeginDragHandler
    {
        private float canvasScale;
        private RectTransform rectTransform;
        private Vector2 characterCenterPoint;

        // Start is called before the first frame update
        void Start()
        {
            canvasScale = gameObject.GetComponentInParent<Canvas>().scaleFactor;
            rectTransform = GetComponent<RectTransform>();
            characterCenterPoint = rectTransform.sizeDelta / 2;

            // Debug setup to start at the middle of the screen
            var position = new Vector3(Screen.width, Screen.height, 0) / (2 * canvasScale);
            rectTransform.anchoredPosition = position;
        }

        // Update is called once per frame
        void Update()
        {
        }

        #region OtherImplementation
        //void OnMouseDown()
        //{
        //    screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        //    offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        //    //Debug.Log($"[PlayerController] offset: {offset}");
        //}

        //void OnMouseDrag()
        //{
        //    Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        //    curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        //    isDragging = true;
        //}

        //void OnMouseUp()
        //{
        //    isDragging = false;
        //}
        #endregion

        public void OnBeginDrag(PointerEventData eventData)
        {
            MoveCharacter(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            MoveCharacter(eventData.position);
        }

        private void MoveCharacter(Vector2 pointerPosition)
        {
            rectTransform.anchoredPosition = (pointerPosition / canvasScale) + characterCenterPoint;
        }
    }
}
