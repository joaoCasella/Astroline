using Astroline.Scripts.SceneControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Astroline.Scripts.Controller
{
    public class PlayerController : MonoBehaviour
#if !UNITY_EDITOR && UNITY_ANDROID
    , IDragHandler, IBeginDragHandler
#endif
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        private Vector2 screenStartPoint;
        private Vector2 screenEndPoint;
#endif

        private Vector2 screenSizeReference;
        private float canvasScale;
        private Vector2 scaledScreenSize;
        private RectTransform playerRectTransform;
        private Vector2 characterCenterPoint;

        void Start()
        {
            canvasScale = SceneController.Instance.CanvasScale;
            scaledScreenSize = new Vector2(Screen.width, Screen.height) / canvasScale;

            playerRectTransform = GetComponent<RectTransform>();
            characterCenterPoint = playerRectTransform.rect.size * playerRectTransform.localScale / 2f;

#if UNITY_EDITOR || UNITY_STANDALONE
            screenSizeReference = SceneController.Instance.PlayAreaSize;
            screenStartPoint = Vector2.Scale(scaledScreenSize - screenSizeReference, SceneController.Instance.ScreenScaleOffset);
            screenEndPoint = screenSizeReference - characterCenterPoint;
#elif UNITY_ANDROID
            screenSizeReference = scaledScreenSize;
            // Debug setup to start at the middle of the screen
            var position = screenSizeReference / 2f;
            rectTransform.anchoredPosition = position;
#endif
        }

        void Update()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            DragCharacter();
#endif
        }

#if UNITY_EDITOR || UNITY_STANDALONE
        void DragCharacter()
        {
            // Gets the mouse position in screen space
            Vector2 mousePosition = Input.mousePosition / canvasScale;

            // Subtracts the point where the container starts and sets as the "new origin"
            mousePosition -= screenStartPoint;

            // Limits the movement to the boundaries of the game container, respecting the character size
            mousePosition.x = Mathf.Clamp(mousePosition.x, characterCenterPoint.x, screenEndPoint.x);
            mousePosition.y = Mathf.Clamp(mousePosition.y, characterCenterPoint.y, screenEndPoint.y);

            // Sets the new player position based on the anchor (lower left corner to correspond to the 
            // mouse position reference
            playerRectTransform.anchoredPosition = mousePosition;
        }

#elif UNITY_ANDROID
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

        //public void OnBeginDrag(PointerEventData eventData)
        //{
        //    MoveCharacter(eventData.position);
        //}

        public void OnDrag(PointerEventData eventData)
        {
            MoveCharacter(eventData.position);
        }

        private void MoveCharacter(Vector2 pointerPosition)
        {
            rectTransform.anchoredPosition = (pointerPosition / canvasScale) + characterCenterPoint;
        }
    }
#endif
    }
}