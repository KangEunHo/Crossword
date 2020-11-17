using UnityEngine;
using UnityEngine.UI;

namespace HealingJam
{
    [RequireComponent(typeof(Image))]
    public class ImageSwap : MonoBehaviour
    {
        public Sprite activatedSprite = null;
        public Sprite deadtivatedSprite = null;
        private Image image = null;
        public bool IsActive { get; private set; }

        private void Start()
        {
            IsActive = Image.sprite == activatedSprite ? true : false;
        }

        public Image Image
        {
            get
            {
                if (this.image == null)
                {
                    this.image = GetComponent<Image>();
                }
                return this.image;
            }
        }

        public void SetSprite(bool active, bool setNativeSize = false)
        {
            this.Image.sprite = active ? activatedSprite : deadtivatedSprite;

            if (setNativeSize)
                this.Image.SetNativeSize();

            IsActive = active;
        }
    }
}