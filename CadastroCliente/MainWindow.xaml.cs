using System;
using System.IO;
using System.Collections.Generic;
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
using System.Data;

namespace CadastroCliente
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dtNasc.Text = DateTime.Today.ToShortDateString();
            LoadGrid();
        }

        private void Cadastrar(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtNome.Text == "" || txtSobrenome.Text == "" || txtCPF.Text == "" || dtNasc.Text == "" || txtCEP.Text == "" ||
                    txtEndereco.Text == "" || txtNumero.Text == "" || txtComplemento.Text == "" || txtCidade.Text == "" || txtEstado.Text == "")
                {
                    MessageBox.Show("Todos os campos devem ser preenchidos!");
                    return;
                }

                StringBuilder sbReg = new StringBuilder();
                sbReg.Append(txtNome.Text);
                sbReg.Append(";");
                sbReg.Append(txtSobrenome.Text);
                sbReg.Append(";");
                sbReg.Append(txtCPF.Text);
                sbReg.Append(";");
                sbReg.Append(dtNasc.Text);
                sbReg.Append(";");
                sbReg.Append(txtCEP.Text);
                sbReg.Append(";");
                sbReg.Append(txtEndereco.Text);
                sbReg.Append(";");
                sbReg.Append(txtNumero.Text);
                sbReg.Append(";");
                sbReg.Append(txtComplemento.Text);
                sbReg.Append(";");
                sbReg.Append(txtCidade.Text);
                sbReg.Append(";");
                sbReg.Append(txtEstado.Text);
                sbReg.AppendLine();
                File.AppendAllText("cadastros.csv", sbReg.ToString());

                MessageBox.Show("Cadastro Realizado com Sucesso!");
                LoadGrid();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao cadastrar: " + ex.Message);
                return;
            }

        }

        public void LoadGrid()
        {
            try
            {
                if (!File.Exists("cadastros.csv"))
                {
                    return;
                }

                string Registros = File.ReadAllText("cadastros.csv");

                DataTable tb = new DataTable();
                tb.Columns.Add("Nome");
                tb.Columns.Add("CPF");
                tb.Columns.Add("Nascimento");
                tb.Columns.Add("Endereco");

                using (System.IO.StringReader reader = new System.IO.StringReader(Registros))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] Regs = line.Split(';');
                        string Nome = Regs[0] + " " + Regs[1];
                        string CPF = Regs[2];
                        string Nasc = Regs[3];
                        string Ende = Regs[5] + ", " + Regs[6] + "/" + Regs[7] + " - " + Regs[8] + "/" + Regs[9] + " - " + Regs[4];
                        tb.Rows.Add(Nome, CPF, Nasc, Ende);
                    }
                }
                gridCli.ItemsSource = tb.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao carregar grid: " + ex.Message);
                return;
            }
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static readonly Regex _regex = new Regex("[^0-9]+");
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
    }
}
