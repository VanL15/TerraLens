using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TerraLens.Project.UI
{
    public class UITabbedPanel : UIElement
    {
        private UIElement tabButtonsContainer;
        private UIElement tabContentContainer;
        private Dictionary<string, UIElement> tabs;
        private UIElement currentContent;

        public UITabbedPanel()
        {
            tabs = new Dictionary<string, UIElement>();

            // Tab buttons container on the left
            tabButtonsContainer = new UIElement();
            tabButtonsContainer.Width.Set(160f, 0f); // Fixed width for tabs
            tabButtonsContainer.Height.Set(0f, 1f);
            tabButtonsContainer.Top.Set(0f, 0f);
            tabButtonsContainer.Left.Set(0f, 0f);
            Append(tabButtonsContainer);

            // Tab content container on the right
            tabContentContainer = new UIElement();
            tabContentContainer.Width.Set(-160f, 1f); // Take the rest of the width
            tabContentContainer.Height.Set(0f, 1f);
            tabContentContainer.Top.Set(0f, 0f);
            tabContentContainer.Left.Set(160f, 0f); // Positioned to the right of tab buttons
            Append(tabContentContainer);
        }

        public void AddTab(string tabName, System.Func<UIElement> contentProvider)
        {
            // Create tab button
            var tabButton = new UITabButton(tabName);
            tabButton.Width.Set(160f, 0f); // Container width
            tabButton.Height.Set(40f, 0f);
            tabButton.HAlign = 0f;
            tabButton.VAlign = 0f;
            tabButton.Top.Set(tabs.Count * 50f + 10f, 0f); // Stack tabs vertically with spacing

            // Handle left click to switch tabs
            tabButton.OnLeftClick += (evt, element) => SwitchTab(tabName);
            tabButtonsContainer.Append(tabButton);

            // Store tab content
            var content = contentProvider();
            tabs[tabName] = content;

            // Display first tab by default
            if (tabs.Count == 1)
            {
                SwitchTab(tabName);
            }
        }

        private void SwitchTab(string tabName)
        {
            if (currentContent != null)
            {
                tabContentContainer.RemoveChild(currentContent);
            }

            currentContent = tabs[tabName];
            tabContentContainer.Append(currentContent);
        }
    }
}
