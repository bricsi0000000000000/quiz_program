using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Quiz
{
    public partial class AnswerItem : UserControl
    {
        private readonly int id;
        public AnswerItem(int id)
        {
            InitializeComponent();

            this.id = id;
        }

        public AnswerItem(Answer answer)
        {
            InitializeComponent();

            id = answer.ID;
            AnswerTextBlock.Text = answer.Name;
            CheckCheckBox.IsChecked = answer.Right;
        }

        private void AnswerTextBlock_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((SettingsContent)TabManager.GetTab("Settings").Content).ChangeActualAnswerName(id, AnswerTextBlock.Text);
        }

        private void CheckCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ((SettingsContent)TabManager.GetTab("Settings").Content).ChangeActualAnswerIsChecked(id, (bool)CheckCheckBox.IsChecked);
        }
    }
}
