using System;
using System.Collections;
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

namespace Ahorcado
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        String word;
        string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "Ñ", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        int fallos = 0;
        int letrasAcertadas = 0;

        public MainWindow()
        {
            InitializeComponent();
            AddLetterButtons();
            SelectWord();

        }

        private void SelectWord()
        {
            word = GetSelectedSetWord(LanguagesComboBox.SelectedItem.ToString().ToLower()).ToUpper();

            Border bottomBorder = new Border();
            bottomBorder.BorderThickness = new Thickness(0, 0, 0, 1);

            for (int i = 0; i < word.Length; i++)
            {
                if(word[i].ToString() == " ") letrasAcertadas++;

                TextBlock newLetter = new TextBlock
                {
                    Text = word[i].ToString() == " " ? " " : "_",
                    Tag = word[i].ToString(),
                    Margin = new Thickness(5),
                    FontSize = 90,
                    TextAlignment = TextAlignment.Center,
                    
                };

                LettersStackPanel.Children.Add(newLetter);
            }
            

        }

        private String GetSelectedSetWord(String language)
        {
            ArrayList wordsES = new ArrayList();

            String selectedSet = SetsComboBox.Text.ToLower();
            switch (selectedSet)
            {
                case "habilidades":
                    wordsES.Add("Habilidad A");
                    wordsES.Add("Habilidad B");
                    wordsES.Add("Habilidad C");
                    break;
                case "pasivas":
                    wordsES.Add("Pasiva1");
                    wordsES.Add("Pasiva2");
                    wordsES.Add("Pasiva3");
                    break;
                default:
                    break;
            }

            Random seed = new Random();
            int randomNumber = seed.Next(0, wordsES.Count);

            return wordsES[randomNumber].ToString();
        }

        private void AddLetterButtons()
        {
            int columns = LetterButtonsUniformGrid.Columns;
            int rows = LetterButtonsUniformGrid.Rows;

            int contador = 0;
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Button letter = new Button
                    {
                        Content = letters[contador],
                        Tag = letters[contador],
                        Margin = new Thickness(5),
                    };
                    letter.Click += Letter_Click;
                    LetterButtonsUniformGrid.Children.Add(letter);

                    contador++;
                }
            }
        }

        private void CheckLetter(String letter)
        {
            Boolean isCorrect = false;
            for (int i = 0; i < word.Length; i++)
            {
                if(letter == word[i].ToString())
                {
                    TextBlock letterInTextBlock = (TextBlock)LettersStackPanel.Children[i];
                    letterInTextBlock.Text = letter;
                    letrasAcertadas++;
                    isCorrect = true;
                }
            }

            if (isCorrect)
            {
                if(letrasAcertadas == word.Length)
                {
                    EndGame(true);
                }
            }
            else
            {
                fallos++;
                hangmanImage.Source = new BitmapImage(new Uri(@"/assets/" + fallos + ".jpg", UriKind.Relative));
                if (fallos >= 7)
                {
                    EndGame(false);
                }
            }
        }

        private void EndGame(Boolean victory)
        {
            String text = victory ? "Has ganado" : "Has perdido";
            MessageBox.Show(text);
        }

        private void Letter_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            button.IsEnabled = false;

            CheckLetter(button.Tag.ToString().ToUpper());
            
        }
    }
    
}
