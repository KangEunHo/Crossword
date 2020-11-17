using UnityEngine;

namespace HealingJam
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteSwap : MonoBehaviour
    {
        public Sprite activatedSprite = null;
        public Sprite deadtivatedSprite = null;
        private SpriteRenderer spriteRenderer = null;
        public bool IsActive { get; private set; }

        private void Start()
        {
            IsActive = spriteRenderer == activatedSprite ? true : false;
        }

        public SpriteRenderer GetRenderer
        {
            get
            {
                if (this.spriteRenderer == null)
                {
                    this.spriteRenderer = GetComponent<SpriteRenderer>();
                }
                return this.spriteRenderer;
            }
        }

        public void SetSprite(bool active)
        {
            GetRenderer.sprite = active ? activatedSprite : deadtivatedSprite;

            IsActive = active;
        }
    }
}