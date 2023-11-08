using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TP_DataBase
{
    public partial class Form3 : Form
    {

        string connectionString = "Data Source=MEDELMAKHFI\\SQLEXPRESS;Initial Catalog=GestionEtudiants;Integrated Security=True"; // Remplacez par votre propre chaîne de connexion

        public Form3()
        {
            InitializeComponent();
        }

        private SqlConnection GetSqlConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string compteClient1 = textBox1.Text;
                string compteClient2 = textBox2.Text;

                decimal soldeClient1 = GetSolde(compteClient1);
                decimal soldeClient2 = GetSolde(compteClient2);

                textBox3.Text = soldeClient1.ToString();
                textBox4.Text = soldeClient2.ToString();
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Erreur de format : Vérifiez que les numéros de compte sont des nombres valides.");
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erreur SQL : " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string compteClient1 = textBox1.Text;
            string compteClient2 = textBox2.Text;
            decimal montant = decimal.Parse(textBox5.Text);

            decimal soldeClient1 = GetSolde(compteClient1);

            if (soldeClient1 >= montant)
            {
                using (SqlConnection connection = GetSqlConnection())
                {
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Debit the account of the sender.
                        using (SqlCommand debitCommand = new SqlCommand("UPDATE Compte SET Solde = Solde - @Montant WHERE NumCompte = @CompteDebiteur", connection, transaction))
                        {
                            debitCommand.Parameters.AddWithValue("@Montant", montant);
                            debitCommand.Parameters.AddWithValue("@CompteDebiteur", compteClient1);
                            debitCommand.ExecuteNonQuery();
                        }

                        // Credit the account of the receiver.
                        using (SqlCommand creditCommand = new SqlCommand("UPDATE Compte SET Solde = Solde + @Montant WHERE NumCompte = @CompteCrediteur", connection, transaction))
                        {
                            creditCommand.Parameters.AddWithValue("@Montant", montant);
                            creditCommand.Parameters.AddWithValue("@CompteCrediteur", compteClient2);
                            creditCommand.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        // Display updated balances after a successful transaction.
                        decimal soldeClient1AfterTransaction = GetSolde(compteClient1);
                        decimal soldeClient2AfterTransaction = GetSolde(compteClient2);

                        textBox3.Text = soldeClient1AfterTransaction.ToString();
                        textBox4.Text = soldeClient2AfterTransaction.ToString();

                        MessageBox.Show("Virement réussi.");

                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Échec du virement. Veuillez vérifier les comptes et les soldes.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Solde insuffisant pour effectuer le virement.");
            }


        }

        private decimal GetSolde(string compte)
        {
            using (SqlConnection connection = GetSqlConnection())
            {
                string query = "SELECT Solde FROM Compte WHERE NumCompte = @Compte";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Compte", compte);
                    object solde = command.ExecuteScalar();

                    if (solde != null)
                    {
                        return (decimal)solde;
                    }
                    else
                    {
                        throw new Exception("Le numéro de compte n'existe pas.");
                    }
                }
            }
        }
    }
}
