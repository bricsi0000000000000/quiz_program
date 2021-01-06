using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Quiz
{
    public static class TabManager
    {
        private readonly static List<TabItem> menuItems = new List<TabItem>();

        public static TabItem GetTab(string name) => menuItems.Find(x => x.Header.Equals(name));

        public static void InitMenuTabs(TabControl tabControl, ref Snackbar errorSnackbar)
        {
            AddTab("Questions", new QuestionsContent(ref errorSnackbar), "questionsMenuTab", tabControl);
            AddTab("Settings", new SettingsContent(ref errorSnackbar), "settingsMenuTab", tabControl, selected: true);
            AddTab("Wrong answers", new CheckAnswersContent(), "checkAnswersMenuTab", tabControl);
        }

        private static void AddTab(string header, object content, string name, TabControl tabControl, bool selected = false)
        {
            TabItem tab = new TabItem
            {
                Header = header,
                Content = content,
                Name = name,
                IsSelected = selected
            };

            menuItems.Add(tab);
            tabControl.Items.Add(tab);
        }
    }
}
