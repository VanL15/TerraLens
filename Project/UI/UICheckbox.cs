//using Terraria.UI;
//using Terraria.GameContent.UI.Elements;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Terraria;
//using Terraria.GameContent;

//namespace TerraLens.Project.UI
//{
//    public class UICheckbox : UIElement
//    {
//        private bool isChecked = false;
//        public bool IsChecked
//        {
//            get => isChecked;
//            set
//            {
//                isChecked = value;
//                OnChange?.Invoke(isChecked, this);
//                Recalculate();
//            }
//        }

//        public delegate void OnChangeDelegate(bool isChecked, UICheckbox checkbox);
//        public event OnChangeDelegate OnChange;

//        public UICheckbox()
//        {
//            Width.Set(20f, 0f);
//            Height.Set(20f, 0f);
//            // Assign click event
//            OnLeftClick += ToggleChecked;
//        }

//        private void ToggleChecked(UIMouseEvent evt, UIElement listeningElement)
//        {
//            IsChecked = !IsChecked;
//        }

//        protected override void DrawSelf(SpriteBatch spriteBatch)
//        {
//            base.DrawSelf(spriteBatch);
//            CalculatedStyle dimensions = GetDimensions();
//            Texture2D texture = TextureAssets.MagicPixel.Value; // Use a 1x1 white pixel texture

//            // Draw checkbox background
//            spriteBatch.Draw(texture, dimensions.ToRectangle(), Color.Gray);

//            // Draw checkmark if checked
//            if (IsChecked)
//            {
//                spriteBatch.Draw(texture, new Rectangle((int)dimensions.X + 4, (int)dimensions.Y + 4, 12, 12), Color.White);
//            }
//        }
//    }
//}
