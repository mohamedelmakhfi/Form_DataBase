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

namespace TP_DataBase
{
    public partial class Form1 : Form
    {

        string connectionString = "Data Source=MEDELMAKHFI\\SQLEXPRESS;Initial Catalog=GestionEtudiants;Integrated Security=True"; // Remplacez par votre propre chaîne de connexion

        public Form1()
        {
            InitializeComponent();
        }

        // Méthode pour obtenir la connexion à la base de données
        private SqlConnection GetSqlConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string nomFiliere = textBox1.Text;

                using (SqlConnection connection = GetSqlConnection())
                {
                    // Créez et exécutez la commande SQL pour insérer la filière
                    string insertQuery = "INSERT INTO Filiere (Nom_filiere) VALUES (@NomFiliere)";
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NomFiliere", nomFiliere);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("La filière a été ajoutée avec succès à la base de données.");
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de l'ajout de la filière.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

    }
}
