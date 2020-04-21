using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Astroline.Scripts.SceneControllers
{
    public class SceneController : MonoBehaviour
    {
        public Canvas CurrentCanvas { get; private set; }
        public float CanvasScale
        {
            get
            {
                if (CurrentCanvas != null)
                    return CurrentCanvas.scaleFactor;
                return 0f;
            }
            private set { }
        }
        public SceneController CurrentSceneController { get; private set; }

        private static SceneController _instance;
        public static SceneController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneController>();
                }

                return _instance;
            }
        }

        [SerializeField]
        private GameObject GameScreen;
        [SerializeField]
        private RectTransform PlayArea;

        private float _borderSize = 5f;

        public float BorderSize
        {
            get { return _borderSize / CanvasScale; }
            private set
            {
                _borderSize = value;
                AdjustBorderSize();
            }
        }

        public Vector2 PlayAreaSize { get { return GameScreen.GetComponent<RectTransform>().rect.size - Vector2.one * BorderSize * 2f; } }
        public Vector2 ScreenScaleOffset { get { return GameScreen.GetComponent<RectTransform>().pivot; } }

        private void AdjustBorderSize()
        {
            PlayArea.offsetMin = Vector2.one * BorderSize;
            PlayArea.offsetMax = -PlayArea.offsetMin;
        }

        private void Awake()
        {
            CurrentCanvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
            CurrentSceneController = GameObject.FindGameObjectWithTag("GameController").GetComponent<SceneController>();

#if UNITY_EDITOR || UNITY_STANDALONE
            AspectRatioFitter aspectRatioFitter = GameScreen.AddComponent<AspectRatioFitter>();
            aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            aspectRatioFitter.aspectRatio = 9f / 16f;
#endif
            BorderSize = 5f;
            Debug.Log($"[SceneController] CurrentCanvas: {CurrentCanvas.gameObject.name}, CurrentSceneController: {CurrentSceneController.gameObject.name}");
        }
    }
}
