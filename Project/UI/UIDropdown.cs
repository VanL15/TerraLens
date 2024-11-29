//using System;
//using System.Collections.Generic;
//using Terraria.UI;
//using Terraria.GameContent.UI.Elements;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System.Linq;

//namespace TerraLens.Project.UI
//{
//    public class UIDropdown : UIElement
//    {
//        private UIPanel mainPanel;
//        private UIText selectedText;
//        private UIList optionsList;
//        private UIScrollbar scrollbar;
//        private bool isOpen = false;
//        private List<string> options;
//        private string selectedOption;

//        public delegate void OnOptionSelectedDelegate(string selected);
//        public event OnOptionSelectedDelegate OnOptionSelected;

//        public UIDropdown(List<string> options, string defaultOption = "All")
//        {
//            this.options = options;
//            selectedOption = defaultOption;

//            // Main panel
//            mainPanel = new UIPanel();
//            mainPanel.Width.Set(150f, 0f);
//            mainPanel.Height.Set(30f, 0f);
//            mainPanel.BackgroundColor = new Color(73, 94, 171) * 0.7f;
//            mainPanel.BorderColor = Color.Black;
//            Append(mainPanel);

//            // Selected text
//            selectedText = new UIText(selectedOption, 0.6f, true);
//            selectedText.Left.Set(5f, 0f);
//            selectedText.VAlign = 0.5f;
//            mainPanel.Append(selectedText);

//            // Toggle button (simple indicator)
//            var toggleIndicator = new UIText("▼", 0.6f, true);
//            toggleIndicator.HAlign = 1f;
//            toggleIndicator.VAlign = 0.5f;
//            mainPanel.Append(toggleIndicator);

//            // Click event to toggle dropdown
//            mainPanel.OnLeftClick += ToggleDropdown;

//            // Options list
//            optionsList = new UIList();
//            optionsList.Width.Set(150f, 0f);
//            optionsList.Height.Set(100f, 0f);
//            optionsList.Top.Set(30f, 0f);
//            optionsList.Left.Set(0f, 0f);
//            // Initially hidden by not drawing it
//            Append(optionsList);

//            // Scrollbar for options
//            scrollbar = new UIScrollbar();
//            scrollbar.SetView(25f, 100f);
//            scrollbar.Height.Set(100f, 0f);
//            scrollbar.Top.Set(30f, 0f);
//            scrollbar.Left.Set(-20f, 1f);
//            optionsList.SetScrollbar(scrollbar);
//            Append(scrollbar);

//            // Populate options
//            foreach (var option in options)
//            {
//                var optionText = new UIText(option, 0.5f, true);
//                optionText.Width.Set(0f, 1f);
//                optionText.Height.Set(25f, 0f);
//                optionText.OnLeftClick += (evt, element) =>
//                {
//                    selectedOption = option;
//                    selectedText.SetText(option);
//                    isOpen = false;
//                    // Hide the optionsList by not drawing it
//                    OnOptionSelected?.Invoke(option);
//                };
//                optionsList.Add(optionText);
//            }
//        }

//        private void ToggleDropdown(UIMouseEvent evt, UIElement listeningElement)
//        {
//            isOpen = !isOpen;
//        }

//        // Method to update options (if needed)
//        public void UpdateOptions(List<string> newOptions, string newSelectedOption = "All")
//        {
//            options.Clear();
//            options.AddRange(newOptions);
//            selectedOption = newSelectedOption;
//            selectedText.SetText(selectedOption);

//            optionsList.Clear();

//            foreach (var option in options)
//            {
//                var optionText = new UIText(option, 0.5f, true);
//                optionText.Width.Set(0f, 1f);
//                optionText.Height.Set(25f, 0f);
//                optionText.OnLeftClick += (evt, element) =>
//                {
//                    selectedOption = option;
//                    selectedText.SetText(option);
//                    isOpen = false;
//                    OnOptionSelected?.Invoke(option);
//                };
//                optionsList.Add(optionText);
//            }
//        }

//        // Optional: Method to set selected option programmatically
//        public void SetSelectedOption(string option)
//        {
//            if (options.Contains(option))
//            {
//                selectedOption = option;
//                selectedText.SetText(option);
//                OnOptionSelected?.Invoke(option);
//            }
//        }

//        // Optional: Method to get the current selected option
//        public string GetSelectedOption()
//        {
//            return selectedOption;
//        }

//        // Override DrawSelf to conditionally draw optionsList
//        protected override void DrawSelf(SpriteBatch spriteBatch)
//        {
//            base.DrawSelf(spriteBatch);

//            if (isOpen)
//            {
//                // Draw the optionsList
//                optionsList.Draw(spriteBatch);
//            }
//        }
//    }
//}
