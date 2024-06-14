using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace SoftWareProject
{
    public partial class Form1 : Form
    {
        OracleDataAdapter adapter;
        OracleCommandBuilder commandBuilder;
        DataSet ds;
        string ordb = "Data Source=ORCL;User ID=scott;Password=tiger";
        OracleConnection conn;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Open Connection
            conn = new OracleConnection(ordb);
            conn.Open();
            //Commend
            OracleCommand cmd = new OracleCommand();
            //Connection
            cmd.Connection = conn;
            //Text
            cmd.CommandText = "Select * from Magazines";
            //Type
            cmd.CommandType = CommandType.Text;
            //Execute
            OracleDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                comboBox1.Items.Add(rdr[0]);
            }
            rdr.Close();
        }

        //search
        private void button1_Click(object sender, EventArgs e)
        {
            string conStr = "Data Source=ORCL;User Id=scott;Password=tiger";
           

                string searchValue = textBox1.Text.Trim();
                string sql = @"
                SELECT Title
                FROM Magazines M
                JOIN Publisher P ON M.PublisherID = P.UserID
                WHERE P.Username = :searchValue";

                adapter = new OracleDataAdapter(sql, conStr);
                adapter.SelectCommand.Parameters.Add("searchValue", OracleDbType.Varchar2).Value = searchValue;

                ds = new DataSet();
                adapter.Fill(ds);

                dataGridView1.DataSource = ds.Tables[0];
            
        }
        //load info
        private void button3_Click(object sender, EventArgs e)
        {
            string constr = "Data Source=ORCL;User Id=scott;Password=tiger";
            string cmdr = "select * from Guest";
            adapter = new OracleDataAdapter(cmdr, constr);
            ds= new DataSet();
            adapter.Fill(ds);

            dataGridView2.DataSource = ds.Tables[0];

        }
        //save
        private void button2_Click(object sender, EventArgs e)
        {
            commandBuilder=new OracleCommandBuilder(adapter);
            adapter.Update(ds.Tables[0]);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "";
            cmd.CommandText = "Insert into Magazines values (:MagazineID,:PublisherID,:Title,:Content,:Published)";
            cmd.Parameters.Add("MagazineID", comboBox1.Text);
            cmd.Parameters.Add("PublisherID", textBox2.Text);
            cmd.Parameters.Add("Title", textBox3.Text);
            cmd.Parameters.Add("Content", textBox4.Text);
            cmd.Parameters.Add("Published", textBox5.Text);
            cmd.CommandType = CommandType.Text;

            int r = cmd.ExecuteNonQuery();
            if (r != -1)
            {
                comboBox1.Items.Add(comboBox1.Text);
                MessageBox.Show("Row is inserted successfully");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            OracleCommand c = new OracleCommand();
            c.Connection = conn;
            c.CommandText = "Select * from Magazines where MagazineID = :id";
            c.Parameters.Add("id", comboBox1.SelectedItem.ToString());
            c.CommandType = CommandType.Text;
            OracleDataReader dr = c.ExecuteReader();
            while (dr.Read())
            {
                textBox2.Text = dr[0].ToString();
                textBox3.Text = dr[1].ToString();
                textBox4.Text = dr[2].ToString();
                textBox5.Text = dr[3].ToString();

            }
            dr.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Dispose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "GettingMagazinesID";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("PID", textBox2.Text);
            cmd.Parameters.Add("MID",OracleDbType.RefCursor,ParameterDirection.Output);
            OracleDataReader dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                comboBox2.Items.Add(dr[0]);
            }
            dr.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int maxRate, newRate;
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn;
            cmd.CommandText = "GetPublisherID";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("PID", OracleDbType.Int32, ParameterDirection.Output);
            cmd.ExecuteNonQuery();
            try
            {
                maxRate = Convert.ToInt32(cmd.Parameters["PID"].Value.ToString());
                newRate = maxRate + 1;
            }
            catch
            {
                newRate = 1;
            }
            textBox2.Text = newRate.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
