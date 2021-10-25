using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
        String parsedWord;
        string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "Ñ", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        int fallos = 0;
        int letrasAcertadas = 0;

        public MainWindow()
        {
            InitializeComponent();
            AddLetterButtons();
            StartNewGame(true);
        }
        
        private void StartNewGame(Boolean firstGame)
        {
            
            if(firstGame)
            {
                SelectRandomSet();
            }
            if (!firstGame)
            {
                DisableComboBoxWarnings();
                fallos = 0;
                letrasAcertadas = 0;
                EnableButtons(true);
                LettersStackPanel.Children.Clear();
                SurrenderButton.IsEnabled = true;
            }
            SelectWord();
            EnableComboBoxWarnings();
        }

        private void SelectRandomSet()
        {
            Random seed = new Random();
            int randomNumber = seed.Next(0, SetsComboBox.Items.Count);

            ComboBoxItem item = (ComboBoxItem)SetsComboBox.Items[randomNumber];
            item.IsSelected = true;
        }

        private void SelectWord()
        {
            word = GetSelectedSetWord().ToUpper();
            parsedWord = NoAccents(word);

            Border bottomBorder = new Border();
            bottomBorder.BorderThickness = new Thickness(0, 0, 0, 1);

            for (int i = 0; i < word.Length; i++)
            {
                if(word[i].ToString() == " ") letrasAcertadas++;

                TextBlock newLetter = new TextBlock
                {
                    Text = word[i].ToString() == " " ? " " : "_",
                    Tag = parsedWord[i].ToString(),
                    Margin = new Thickness(5),
                    FontSize = 90,
                    Width = 100,
                    TextAlignment = TextAlignment.Center,
                };
                LettersStackPanel.Children.Add(newLetter);
            }

        }

        private String NoAccents(String word)
        {
            
            String parsedWord = new String(word.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray()).Normalize(NormalizationForm.FormC);

            return parsedWord;
        }

        private void MostrarLetras()
        {
            for (int i = 0; i < word.Length; i++)
            {
                TextBlock letterBlock = (TextBlock)LettersStackPanel.Children[i];
                letterBlock.Text = word[i].ToString();
            }
        }

        private String GetSelectedSetWord()
        {
            ArrayList words = new ArrayList();

            String language = ((ComboBoxItem)LanguagesComboBox.SelectedItem).Tag.ToString();
            String selectedSet = ((ComboBoxItem)SetsComboBox.SelectedItem).Tag.ToString();

            String category = selectedSet + "_" + language;
            switch (category)
            {
                case "eidolons_ES": case "eidolons_US":
                    words.Add("Lilit");
                    words.Add("Garuda");
                    words.Add("Abe no Seimei");
                    words.Add("Bastet");
                    words.Add("Summer Persephone");
                    break;
                case "masteries_ES":
                    words.Add("Fanatismo");
                    words.Add("Romper la Defensa");
                    words.Add("Vigor");
                    break;
                case "masteries_US":
                    words.Add("Fanatismo_US");
                    words.Add("Romper la Defensa_US");
                    words.Add("Vigor_US");
                    break;
                case "items_ES":
                    words.Add("Rodajas de Sandía");
                    words.Add("Katana de Gea");
                    words.Add("Poción de Reiniciación de Gemas Secretas Superior");
                    break;
                case "items_US":
                    words.Add("Rodajas de Sandía_US");
                    words.Add("Katana de Gea_US");
                    words.Add("Poción de Reiniciación de Gemas Secretas Superior_US");
                    break;
                case "titles_ES":
                    words.Add("Eres adorable pero yo soy inmune");
                    words.Add("Siempre Joven");
                    words.Add("Blanco Nieve");
                    break;
                case "titles_US":
                    words.Add("Eres adorable pero yo soy inmune_US");
                    words.Add("Siempre Joven_US");
                    words.Add("Blanco Nieve_US");
                    break;
                default:
                    break;
            }

            Random seed = new Random();
            int randomNumber = seed.Next(0, words.Count);

            return words[randomNumber].ToString();
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
                if(letter == parsedWord[i].ToString())
                {
                    TextBlock letterInTextBlock = (TextBlock)LettersStackPanel.Children[i];
                    letterInTextBlock.Text = word[i].ToString();
                    letrasAcertadas++;
                    isCorrect = true;
                }
            }

            if (isCorrect)
            {
                if(letrasAcertadas == word.Length)
                {
                    EndGame("¡Has ganado!");
                }
            }
            else
            {
                fallos++;
                hangmanImage.Source = new BitmapImage(new Uri(@"/assets/" + fallos + ".jpg", UriKind.Relative));
                if (fallos >= 7)
                {
                    EndGame("¡Has perdido! Mas suerte la próxima");
                }
            }
        }

        private void EndGame(String text)
        {
            EnableButtons(false);
            SurrenderButton.IsEnabled = false;
            MessageBox.Show(text, "¡Fin del juego!", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void Letter_KeyDown(object sender, KeyEventArgs e)
        {
            if (letters.Contains(e.Key.ToString().ToUpper()))
            {
                Boolean wasEnabled = DisableButton(e.Key.ToString().ToUpper());
                if(wasEnabled)
                {
                    CheckLetter(e.Key.ToString().ToUpper());
                }
            }
        }

        private Boolean DisableButton(String letter)
        {
            Boolean wasEnabled = false;
            foreach (Button button in LetterButtonsUniformGrid.Children)
            {
                if (button.Tag.ToString().ToUpper() == letter)
                {
                    wasEnabled = button.IsEnabled;
                    button.IsEnabled = false;
                }
            }

            return wasEnabled;
        }


        private void EnableButtons(Boolean enable)
        {
            foreach (Button button in LetterButtonsUniformGrid.Children)
            {
                button.IsEnabled = enable;
            }
        }

        private void Letter_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            button.IsEnabled = false;

            CheckLetter(button.Tag.ToString().ToUpper());
            
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame(false);
        }

        private void Surrender_Click(object sender, RoutedEventArgs e)
        {
            MostrarLetras();
            EndGame("Te has rendido...");
        }

        private void EnableComboBoxWarnings()
        {
            LanguagesComboBox.SelectionChanged += ComboBox_SelectionChanged;
            SetsComboBox.SelectionChanged += ComboBox_SelectionChanged;
        }

        private void DisableComboBoxWarnings()
        {
            LanguagesComboBox.SelectionChanged -= ComboBox_SelectionChanged;
            SetsComboBox.SelectionChanged -= ComboBox_SelectionChanged;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("El cambio se efectuará al comenzar una nueva partida", "¡Aviso!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
    
}
