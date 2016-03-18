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
    public partial class Form1 : Form
    {
        // "data source=orcl; user id=scott; password=tiger;"


        string str = "data source=orcl; user id=scott; password=tiger;";
        OracleConnection conn ; 

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new OracleConnection(str) ;
            conn.Open();
            Init(); 

        }


        void Init()
        {
            OracleCommand cmd = new OracleCommand("select memberid from Members ");
            cmd.Connection = conn; 
            cmd.CommandType = CommandType.Text; 
            var R = cmd.ExecuteReader();
            while (R.Read())
            {
                comboBox1.Items.Add(R["memberID"].ToString());
                /*textBox1.Text = R.GetString(1).ToString(); 
                textBox1.*/
            }
            R.Close(); 

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Dispose(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {


            var c = new OracleCommand();
            c.Connection = conn;
            c.CommandText = "insert into members values (memId.nextval , :1 , :2 ,:3 ,:4)  returning memberid into :5 ";
            c.Parameters.Add("name", textBox1.Text); 
            c.Parameters.Add("join" , Convert.ToDateTime(dateTimePicker2.Text)); 
            c.Parameters.Add("birth" , Convert.ToDateTime(dateTimePicker1.Text)) ;
            c.Parameters.Add("ph", txtphone.Text);
            c.Parameters.Add("id", OracleDbType.Int64, ParameterDirection.ReturnValue); 

            int r=-1;
            try
            {
                 r = c.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); 
            }
            if (r != -1)
            { MessageBox.Show("NEW MEMBER ADDED");
            Int64 res = Convert.ToInt64(c.Parameters["id"].Value.ToString());
            comboBox1.Items.Add(res.ToString());
            comboBox1.Text = res.ToString(); 
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OracleCommand c = new OracleCommand();
            c.Connection = conn;
            c.CommandText =
            "update members set memname=:1, joindate=:2, DOB=:3, Phone=:4 where memberID=:5";

            c.Parameters.Add("n",  textBox1.Text);
            c.Parameters.Add("J", Convert.ToDateTime(dateTimePicker2.Text));
            c.Parameters.Add("bd", Convert.ToDateTime(dateTimePicker1.Text));
            c.Parameters.Add("P", txtphone.Text);

            c.Parameters.Add("ID", comboBox1 .SelectedItem.ToString());
            int r = c.ExecuteNonQuery();
            if (r != -1)
                MessageBox.Show("Member data is modified");

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var c = new OracleCommand();
            c.Connection = conn;
            c.CommandText = "select  *  from members  where memberid = :1 ";
            c.Parameters.Add("id", comboBox1.SelectedItem.ToString());


            var dr = c.ExecuteReader();
            if (dr.Read())
            {

                textBox1.Text = dr.GetString(1);
                dateTimePicker2.Text = dr.GetDateTime(2).ToString();
                dateTimePicker1.Text = dr.GetDateTime(3).ToString();
                if (!(dr.IsDBNull(4)))
                    txtphone.Text = dr[4].ToString(); 

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // after the row is successfully deleted, you should clear the values of the controls on the form
            OracleCommand c = new OracleCommand();
            c.Connection = conn;
            c.CommandText = "Delete from Members where MemberID=:1";
            c.Parameters.Add("id", comboBox1.SelectedItem.ToString());
            int r = c.ExecuteNonQuery();
            if (r != -1)
            {
                MessageBox.Show("Member deleted");
                int i = comboBox1.SelectedIndex;
                comboBox1.Items.RemoveAt(i);
                textBox1.Text = "";
                dateTimePicker2 .Text = DateTime.Now.ToString();
                dateTimePicker1.Text = DateTime.Now.ToString();
                txtphone.Text = "";
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            this.Hide();
            f.Show(); 
        }
    }
}
