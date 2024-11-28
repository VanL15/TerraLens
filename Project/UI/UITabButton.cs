using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerraLens.Project.UI
{
    public class UITabButton : UIPanel
    {
        private UIText text;

        public UITabButton(string textContent, float fontScale = 0.4f) : base()
        {
            // Set alignment and appearance
            HAlign = 0f;
            VAlign = 0f;
            Width.Set(160f, 0f); // Container width
            Height.Set(40f, 0f);
            BackgroundColor = new Color(63, 82, 151) * 0.7f;
            BorderColor = Color.Black;

            // Add text
            text = new UIText(textContent, fontScale, true);
            text.HAlign = 0.5f;
            text.VAlign = 0.5f;
            Append(text);

            // Hover effects
            OnMouseOver += (evt, element) => BackgroundColor = new Color(73, 94, 171);
            OnMouseOut += (evt, element) => BackgroundColor = new Color(63, 82, 151) * 0.7f;
        }
    }
}
