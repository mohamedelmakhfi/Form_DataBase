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
    public partial class Form2 : Form
    {

        string connectionString = "Data Source=MEDELMAKHFI\\SQLEXPRESS;Initial Catalog=GestionEtudiants;Integrated Security=True"; // Remplacez par votre propre chaîne de connexion

        public Form2()
        {
            InitializeComponent();
        }

        private SqlConnection GetSqlConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCNE = comboBox1.SelectedItem.ToString();
            

            using (SqlConnection connection = GetSqlConnection()) 
            {
                // Créez la commande SQL pour récupérer les informations de l'étudiant
                string query = "SELECT Nom, Prenom, Filiere.Nom_filiere " +
                               "FROM Etudiant " +
                               "INNER JOIN Filiere ON Etudiant.Id_filiere = Filiere.Id_filiere " +
                               "WHERE Etudiant.Cne = @CNE";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CNE", selectedCNE);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Récupérez les valeurs des colonnes
                            string nom = reader["Nom"].ToString();
                            string prenom = reader["Prenom"].ToString();
                            string filiere = reader["Nom_filiere"].ToString();

                            // Affichez les informations dans les TextBox correspondants
                            textBox1.Text = nom;
                            textBox2.Text = prenom;
                            textBox3.Text = filiere;
                        }
                        else
                        {
                            // L'étudiant avec le CNE sélectionné n'a pas été trouvé
                            // Effacez les TextBox
                            textBox1.Text = string.Empty;
                            textBox2.Text = string.Empty;
                            textBox3.Text = string.Empty;
                        }
                    }
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = GetSqlConnection()) 
            {
                string query = "SELECT Cne FROM Etudiant";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader["Cne"].ToString());
                        }
                    }
                }
            }
        }
    }
}
