using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace TerraLens.Project.UI;

public class TileInteractionElement : UIElement
{
    private Color backgroundColor = Color.Transparent;

    public TileInteractionElement(string name, object value)
    {
        Width.Set(0, 1f);
        Height.Set(25f, 0f);
        PaddingLeft = 5f;
        PaddingRight = 5f;

        // Name Text
        var nameText = new UIText(name, 0.4f, true);
        nameText.Width.Set(300f, 0f);
        nameText.Left.Set(0f, 0f);
        nameText.Top.Set(0f, 0f);
        Append(nameText);

        // Value Text
        var valueText = new UIText(value.ToString(), 0.4f, true);
        valueText.Width.Set(200f, 0f);
        valueText.Left.Set(310f, 0f); // Position next to name
        valueText.Top.Set(0f, 0f);
        Append(valueText);
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
