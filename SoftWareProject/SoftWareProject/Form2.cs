using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Data.Common;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SoftWareProject
{
    public partial class Form2 : Form
    {
        OracleDataAdapter adapter2;
        OracleCommandBuilder commandBuilder2;
        DataSet ds2;
        string ordb2 = "Data Source=ORCL;User ID=scott;Password=tiger";
        OracleConnection conn2;

        public Form2()
        {
            InitializeComponent();
        }
        //search
        private void button1_Click(object sender, EventArgs e)
        {
            string conStr = "Data Source=ORCL;User Id=scott;Password=tiger";

            string searchValue = textBox1.Text.Trim();
            string sql = @"
                SELECT Review 
                FROM Ratings  R
                JOIN Guest G ON R.UserID  = G.UserID
                WHERE G.Username = :searchValue";

            adapter2 = new OracleDataAdapter(sql, conStr);
            adapter2.SelectCommand.Parameters.Add("searchValue", OracleDbType.Varchar2).Value = searchValue;

            ds2 = new DataSet();
            adapter2.Fill(ds2);

            dataGridView1.DataSource = ds2.Tables[0];
        }
        //update
        private void button3_Click(object sender, EventArgs e)
        {
            string constr = "Data Source=ORCL;User Id=scott;Password=tiger";
            string cmdr = "select * from Publisher";
            adapter2 = new OracleDataAdapter(cmdr, constr);
            ds2 = new DataSet();
            adapter2.Fill(ds2);

            dataGridView2.DataSource = ds2.Tables[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //save

            commandBuilder2 = new OracleCommandBuilder(adapter2);
            adapter2.Update(ds2.Tables[0]);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            conn2 = new OracleConnection(ordb2);
            conn2.Open();
            OracleCommand c = new OracleCommand();
            c.Connection = conn2;
            c.CommandText = "Select UserID from Publisher ";
            c.CommandType = CommandType.Text;
            OracleDataReader r = c.ExecuteReader();
            while(r.Read())
            {
                comboBox1.Items.Add(r["UserID"]);
            }
            r.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn2;
            cmd.CommandText = "Insert into Publisher values (:UserID,:Username,:Email,:Password)";
            cmd.Parameters.Add("UserID", comboBox1.Text);
            cmd.Parameters.Add("Username", textBox2.Text);
            cmd.Parameters.Add("Email", textBox3.Text);
            cmd.Parameters.Add("Password", textBox4.Text);
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
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn2;
            cmd.CommandText = "Select * from Publisher where UserID=:ID";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("ID",comboBox1.SelectedItem.ToString());
            OracleDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                textBox2.Text = reader[0].ToString();
                textBox3.Text = reader[1].ToString();
                textBox4.Text = reader[2].ToString();
            }
            reader.Close();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn2.Dispose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn2;
            cmd.CommandText = "GettingMagazinesID";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("PID", textBox6.Text);
            cmd.Parameters.Add("MID", OracleDbType.RefCursor, ParameterDirection.Output);
            OracleDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                comboBox2.Items.Add(dr[0]);
            }
            dr.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int maxRate, newRate;
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = conn2;
            cmd.CommandText = "GetUserID";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("UseID", OracleDbType.Int32,ParameterDirection.Output);
            cmd.ExecuteNonQuery();
            try
            {
                maxRate = Convert.ToInt32(cmd.Parameters["UseID"].Value.ToString());
                newRate = maxRate + 1;
            }
            catch 
            {
                newRate = 1;
            }
            textBox5.Text = newRate.ToString();
        }
    }
}
