using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        int maxMistakes = 7;
        int mistakes = 0;
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
                mistakes = 0;
                numeroVidasTextBlock.Text = (maxMistakes - mistakes).ToString();
                hangmanImage.ImageSource = new BitmapImage(new Uri(@"../../assets/" + mistakes + ".jpg", UriKind.Relative));
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
            parsedWord = DeleteAccents(word);

            Border bottomBorder = new Border();
            bottomBorder.BorderThickness = new Thickness(0, 0, 0, 1);

            for (int i = 0; i < word.Length; i++)
            {
                String letraAAñadir = "_";
                if (!Char.IsLetter(parsedWord[i]))
                {
                    letraAAñadir = word[i].ToString();
                    letrasAcertadas++;
                }

                TextBlock newLetter = new TextBlock
                {
                    Text = letraAAñadir,
                    Tag = parsedWord[i].ToString(),
                    Style = (Style)this.Resources["LetterTextBlock"]
                };
                LettersStackPanel.Children.Add(newLetter);
            }

        }

        private String DeleteAccents(String word)
        {
            return string.Concat(Regex.Replace(word, @"(?i)[\p{L}-[ña-z]]+", m => m.Value.Normalize(NormalizationForm.FormD))
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));
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
                    words.Add("Hiperion");
                    words.Add("Halloween Zashi");
                    words.Add("Otohime");
                    words.Add("Summer Persephone");
                    words.Add("Salome");
                    words.Add("Inaba");
                    words.Add("Genbu");
                    words.Add("Urd");
                    words.Add("Aoandon");
                    words.Add("Qingniao");
                    words.Add("Sakuya-hime");
                    words.Add("Tyr");
                    break;
                case "abilities_ES":
                    words.Add("Asesinato");
                    words.Add("Bombardeo");
                    words.Add("Mariposas Fulminantes");
                    words.Add("Golondrina Negra");
                    words.Add("Golpe de Trueno Divino");
                    words.Add("Golpe Desmesurado");
                    words.Add("Electro Funk");
                    words.Add("Disparo Dimensional");
                    words.Add("Liberación de la Marca");
                    words.Add("Estrella Almíbar");
                    words.Add("Destello de Ignis");
                    break;
                case "abilities_US":
                    words.Add("Fireball");
                    words.Add("Decoy");
                    words.Add("Implosion");
                    words.Add("Terrifying Roar");
                    words.Add("Spur");
                    words.Add("Frantic Lotus");
                    words.Add("Frozen Tomb");
                    words.Add("Phase Shift Laser");
                    words.Add("Ground Slasher");
                    break;
                case "items_ES":
                    words.Add("Rodajas de Sandía");
                    words.Add("Katana de Gea");
                    words.Add("Pargo Rápido");
                    words.Add("Monopatín Rayo Cósmico");
                    words.Add("Fulgor - Dragón Divino de Ojos Dorados");
                    words.Add("Flotador Playero Moderno");
                    words.Add("Gato Portafortuna");
                    words.Add("Danza de Artemisa");
                    words.Add("Emblema de Pesadilla - Eternidad");
                    words.Add("Polvo Rosa Dorado");
                    words.Add("Dragoñeco Engañoso");
                    break;
                case "items_US":
                    words.Add("Sturdy Axe");
                    words.Add("Titan Warrior Battle Boots");
                    words.Add("Purified Obsidian Bracelet");
                    words.Add("Beebis the Ostrich");
                    words.Add("Pumpkin Phantom");
                    words.Add("Viridescent Flaming Phoenix Wings");
                    words.Add("Fearless Staff");
                    words.Add("Fated Dark Tarot Card");
                    words.Add("Yellow Bird Hatchling");
                    words.Add("Insidius");
                    words.Add("Soulsucker Ore");
                    break;
                case "titles_ES":
                    words.Add("Eres adorable pero yo soy inmune");
                    words.Add("Siempre Joven");
                    words.Add("Blanco Nieve");
                    words.Add("Confidente de Skandia");
                    words.Add("Vinatero Sustituto");
                    words.Add("Caracolero");
                    words.Add("Me pelo de frío");
                    words.Add("Servicio de atención al cliente");
                    words.Add("Fresco como una lechuga");
                    words.Add("Ahora sólo tomo jugo de tomate");
                    words.Add("UwU");
                    words.Add("LEGEN - espera - DARIO");
                    break;
                case "titles_US":
                    words.Add("Avatar");
                    words.Add("Puzzle Pro");
                    words.Add("Poor Winner");
                    words.Add("Wind Walker");
                    words.Add("Miraculous");
                    words.Add("Iron Man");
                    words.Add("Womanizer");
                    words.Add("Toxic Beauty");
                    words.Add("Regicide");
                    words.Add("Does this Air Make Me Look Fat?");
                    words.Add("YOLO");
                    break;
                case "monsters_ES":
                    words.Add("<Felio joven> Paparr Ryan");
                    words.Add("<Duque no muerto> Tarand");
                    words.Add("<Señor de la Guerra Hanba> Gorka");
                    words.Add("<Antiguo enviado de Gea> Ninus el maldito");
                    words.Add("<Segador de pesadilla> Scheel");
                    words.Add("<Duque alado> Avidas");
                    words.Add("<Anhelo de destino> Lucius");
                    words.Add("<Reflejo Oscuro> Enviado de Gea");
                    words.Add("<Corderita> Beeeeelén");
                    words.Add("<Reina Demoníaca del Ultramundo> Delfi");
                    break;
                case "monsters_US":
                    words.Add("<Desert Stalwart> Zangis");
                    words.Add("<Parallax Prince> Gareth");
                    words.Add("<Split Personality> Rabisu");
                    words.Add("<Soul Shatterer> Keres");
                    words.Add("<Cult Leader> Zaunna");
                    words.Add("<Zombie Gatekeeper> Malodnak");
                    words.Add("<Glimmering Princess> Fia");
                    words.Add("<Blood Spider Queen> Tavana");
                    words.Add("<Transformer Bouncer> Galio");
                    words.Add("<Tiny Hornwolf> Fenrir");
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
                        Style = (Style)this.Resources["LetterButton"]
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
                mistakes++;
                hangmanImage.ImageSource = new BitmapImage(new Uri(@"../../assets/" + mistakes + ".jpg", UriKind.Relative));
                numeroVidasTextBlock.Text = (maxMistakes - mistakes).ToString();

                if (mistakes >= maxMistakes)
                {
                    EndGame("¡Has perdido! Mas suerte la próxima");
                }
            }
        }

        private void EndGame(String text)
        {
            EnableButtons(false);
            MostrarLetras();
            SurrenderButton.IsEnabled = false;
            MessageBox.Show(text, "¡Fin del juego!", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private void Letter_KeyDown(object sender, KeyEventArgs e)
        {    
            String letter = e.Key.ToString().ToUpper();
            if(letter == "OEM3")
            {
                letter = "Ñ";
            }
            if (letters.Contains(letter) || letter == "OEM3")
            {
                Boolean wasEnabled = DisableButton(letter);
                if(wasEnabled)
                {
                    CheckLetter(letter);
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
                    button.Visibility = Visibility.Hidden;
                }
            }

            return wasEnabled;
        }


        private void EnableButtons(Boolean enable)
        {
            foreach (Button button in LetterButtonsUniformGrid.Children)
            {
                button.IsEnabled = enable;
                button.Visibility = enable ? Visibility.Visible : Visibility.Hidden;
            }
        }

        private void Letter_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            button.IsEnabled = false;
            button.Visibility = Visibility.Hidden;

            CheckLetter(button.Tag.ToString().ToUpper());
            
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame(false);
        }

        private void Surrender_Click(object sender, RoutedEventArgs e)
        {
            mistakes = 7;
            hangmanImage.ImageSource = new BitmapImage(new Uri(@"../../assets/" + mistakes + ".jpg", UriKind.Relative));
            numeroVidasTextBlock.Text = (maxMistakes - mistakes).ToString();
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
