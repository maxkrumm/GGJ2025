using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Common
{
    public class MessageBox : MonoBehaviour
    {
        private static MessageBoxManager objectPool;

        private static MessageBoxManager ObjectPoolInstance
        {
            get
            {
                if (objectPool == null)
                {
                    var resOBJ = Resources.Load<GameObject>("Prefabs/Common/TutorialMessageBox");
                    if (resOBJ == null)
                    {
                        Debug.LogError("MessageBox prefab not found at Resources/Prefabs/Common/TutorialMessageBox");
                        return null;
                    }

                    var messageBox = resOBJ.GetComponent<MessageBox>();
                    if (messageBox == null)
                    {
                        Debug.LogError("Prefab does not contain a MessageBox component.");
                        return null;
                    }

                    objectPool = new MessageBoxManager(messageBox);
                }

                return objectPool;
            }
        }

        [SerializeField] private Text _text;
        public string Text { get => _text.text; set => _text.text = value; }
        private CanvasGroup _canvasGroup;
        public float duration = 1.0f; // フェード時間

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0;
        }

        public void Open()
        {
            FadeIn();
        }

        public void Close()
        {
            FadeOut();
        }


        public void FadeIn()
        {
            _canvasGroup.DOFade(1f, duration).SetEase(Ease.InOutQuad);
        }

        public void FadeOut()
        {
            _canvasGroup.DOFade(0f, duration)
                .SetEase(Ease.InOutQuad);
            //.OnComplete(() => ); // フェードアウト後に削除
        }
        public static MessageBox ShowText()
        {
            return ObjectPoolInstance.Rental();
        }

        public static MessageBox ShowText(Vector2 pos)
        {
            var msgBox = ObjectPoolInstance.Rental();
            ((RectTransform)msgBox.transform).anchoredPosition = pos;
            return msgBox;
        }
    }
}