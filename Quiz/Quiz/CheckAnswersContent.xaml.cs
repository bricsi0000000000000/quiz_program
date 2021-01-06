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
    /// <summary>
    /// Interaction logic for CheckAnswersContent.xaml
    /// </summary>
    public partial class CheckAnswersContent : UserControl
    {
        public CheckAnswersContent()
        {
            InitializeComponent();
        }

        public void UpdateQuestions()
        {
            QuestionsStackPanel.Children.Clear();

            foreach (var wrongQuestionID in QuestionManager.WrongAnsweredQuestions)
            {
                QuestionsStackPanel.Children.Add(new CheckQuestionItem(QuestionManager.GetWrongQuestion(wrongQuestionID.ID),
                                                                       QuestionManager.GetQuestion(wrongQuestionID.ID)));
            }
        }

        public void ClearQuestionsStackPanelChildren()
        {
            QuestionsStackPanel.Children.Clear();
        }
    }
}
