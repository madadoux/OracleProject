using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

namespace Members
{
    public partial class Form2 : Form
    {



        string str = "data source=orcl; user id=scott; password=tiger;";
        OracleConnection conn; 

        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            var c = new OracleCommand();
            c.Connection = conn;
            c.CommandText = "insert into filmCategory values (catID.nextval , :1 )  returning categoryID into :2 ";
            c.Parameters.Add("name", textBox1.Text);
            c.Parameters.Add("categoryID", OracleDbType.Int32,ParameterDirection.Output);
            int r = -1;
            try
            {
                r = c.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (r != -1)
            {
                MessageBox.Show("NEW CATEGORY ADDED");
                Int64 res = Convert.ToInt64(c.Parameters["categoryID"].Value.ToString());
                comboBox1.Items.Add(res.ToString());
                comboBox1.Text = res.ToString();
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            conn = new OracleConnection(str);
            conn.Open();
            Init(); 
        }



        void Init()
        {
            OracleCommand cmd = new OracleCommand("select categoryID from FilmCategory ");
            cmd.Connection = conn;
            cmd.CommandType = CommandType.Text;
            var R = cmd.ExecuteReader();
            while (R.Read())
            {
                comboBox1.Items.Add(R["categoryID"].ToString());
                /*textBox1.Text = R.GetString(1).ToString(); 
                textBox1.*/
            }
            R.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {

            OracleCommand c = new OracleCommand();
            c.Connection = conn;
            c.CommandText =
            "update FilmCategory set categoryName=:1  where CategoryID=:2";

            c.Parameters.Add("n", textBox1.Text);
       

            c.Parameters.Add("ID", comboBox1.SelectedItem.ToString());
            int r = c.ExecuteNonQuery();
            if (r != -1)
                MessageBox.Show("Member data is modified");

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var c = new OracleCommand();
            c.Connection = conn;
            c.CommandText = "select  *  from FilmCategory  where CategoryID = :1 ";
            c.Parameters.Add("id", comboBox1.SelectedItem.ToString());


            var dr = c.ExecuteReader();
            if (dr.Read())
            {
                
                textBox1.Text = dr.GetString(1);

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // after the row is successfully deleted, you should clear the values of the controls on the form
            OracleCommand c = new OracleCommand();
            c.Connection = conn;
            c.CommandText = "Delete from FilmCategory where CategoryID=:1";
            c.Parameters.Add("id", comboBox1.SelectedItem.ToString());
            int r = c.ExecuteNonQuery();
            if (r != -1)
            {
                MessageBox.Show("Category deleted");
                int i = comboBox1.SelectedIndex;
                comboBox1.Items.RemoveAt(i);
                textBox1.Text = "";
            }
        }
    }
}
