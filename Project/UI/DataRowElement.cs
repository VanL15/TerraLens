using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace TerraLens.Project.UI
{
    public class DataRowElement : UIElement
    {
        private Color backgroundColor = Color.Transparent;

        public DataRowElement(string name, object value)
        {
            Width.Set(0f, 1f);
            Height.Set(25f, 0f);
            PaddingLeft = 5f;
            PaddingRight = 5f;

            // Truncate name if above 20 characters
            if (name.Length > 25)
            {
                name = name.Substring(0, 25) + "...";
            }

            // Name Text - 40% width
            var nameText = new UIText(name, 0.4f, true);
            nameText.Width.Set(0f, 0.4f); // 40% of parent width
            nameText.Height.Set(0f, 1f);
            nameText.Left.Set(0f, 0f);
            nameText.Top.Set(6f, 0f);
            nameText.SetPadding(0f);
            nameText.HAlign = 0f;
            Append(nameText);

            // Value Text - 60% width
            var valueText = new UIText(value.ToString(), 0.4f, true);
            valueText.Width.Set(0f, 0.6f); // 60% of parent width
            valueText.Height.Set(0f, 1f);
            valueText.Left.Set(0f, 0.4f); // Positioned after nameText
            valueText.Top.Set(6f, 0f);
            valueText.SetPadding(0f);
            valueText.HAlign = 0f;
            Append(valueText);
        }

        public DataRowElement(string name, object value1, object value2)
        {
            Width.Set(0f, 1f);
            Height.Set(25f, 0f);
            PaddingLeft = 5f;
            PaddingRight = 5f;

            // Truncate name if above 20 characters
            if (name.Length > 30)
            {
                name = name.Substring(0, 30) + "...";
            }

            // Name Text - 40% width
            var nameText = new UIText(name, 0.4f, true);
            nameText.Width.Set(0f, 0.4f); // 40% of parent width
            nameText.Height.Set(0f, 1f);
            nameText.Left.Set(0f, 0f);
            nameText.Top.Set(6f, 0f);
            nameText.SetPadding(0f);
            nameText.HAlign = 0f;
            Append(nameText);

            // Value Text - 30% width
            var valueText1 = new UIText(value1.ToString(), 0.4f, true);
            valueText1.Width.Set(0f, 0.3f); // 30% of parent width
            valueText1.Height.Set(0f, 1f);
            valueText1.Left.Set(0f, 0.4f); // Positioned after nameText
            valueText1.Top.Set(6f, 0f);
            valueText1.SetPadding(0f);
            valueText1.HAlign = 0f;
            Append(valueText1);

            // Value Text - 30% width
            var valueText2 = new UIText(value2.ToString(), 0.4f, true);
            valueText2.Width.Set(0f, 0.3f); // 30% of parent width
            valueText2.Height.Set(0f, 1f);
            valueText2.Left.Set(0f, 0.7f); // Positioned after nameText
            valueText2.Top.Set(6f, 0f);
            valueText2.SetPadding(0f);
            valueText2.HAlign = 0f;
            Append(valueText2);
        }

        public Color BackgroundColor
        {
            get => backgroundColor;
            set
            {
                backgroundColor = value;
                Recalculate(); // Update layout if necessary
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (backgroundColor != Color.Transparent)
            {
                // Draw background
                CalculatedStyle dimensions = GetDimensions();
                spriteBatch.Draw(TextureAssets.MagicPixel.Value,
                                 new Rectangle((int)dimensions.X, (int)dimensions.Y, (int)dimensions.Width, (int)dimensions.Height),
                                 backgroundColor);
            }

            base.Draw(spriteBatch);
        }
    }
}
