﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project01_ATMT
{
    public partial class Menu : Form
    {
        public Menu(string LoginEmail)
        {
            InitializeComponent();
            loadInfor(LoginEmail);
            SetAllTextBoxToReadOnly();
            LoadListData();
        }
        public void SetAllTextBoxToReadOnly()
        {
            t_Email.ReadOnly = true;
            t_Password.ReadOnly = true;
            t_Phone.ReadOnly = true;
            t_FullName.ReadOnly = true;
            t_dob.ReadOnly = true;
            t_Address.ReadOnly = true;
        }
        public void EnableEditAllTextBox()
        {
            t_Password.ReadOnly = false;
            t_Phone.ReadOnly = false;
            t_FullName.ReadOnly = false;
            t_dob.ReadOnly = false;
            t_Address.ReadOnly = false;
        }
        public void ClearAllTextBox()
        {
            t_Email.Clear();
            t_Password.Clear();
            t_Phone.Clear();
            t_FullName.Clear();
            t_dob.Clear();
            t_Address.Clear();
        }

        public void loadInfor(string loginEmail)
        {
            DataTable data = DAO_Account.get_Instance().getUserInfo(loginEmail);
            this.EnableEditAllTextBox();
            this.t_Email.Text = data.Rows[0][0].ToString();
            this.t_Password.Text = "Hidden";
            this.t_Phone.Text = data.Rows[0][2].ToString();
            this.t_FullName.Text = data.Rows[0][3].ToString();
            this.t_dob.Text = data.Rows[0][4].ToString();
            this.t_Address.Text = data.Rows[0][5].ToString();
            SetAllTextBoxToReadOnly();
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            EnableEditAllTextBox();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(t_Email.Text) || String.IsNullOrEmpty(t_Password.Text) || String.IsNullOrEmpty(t_Phone.Text) || String.IsNullOrEmpty(t_FullName.Text) || String.IsNullOrEmpty(t_dob.Text) || String.IsNullOrEmpty(t_Address.Text))
            {
                MessageBox.Show("Text box cannot be left blank");
                return;
            }

            DTO_Account acc = new DTO_Account();
            acc.Email = t_Email.Text.ToString();
            acc.Password = SHA256.Hash(Encoding.ASCII.GetBytes(t_Password.Text));
            acc.Phone = t_Phone.Text.ToString();
            acc.Fullname = t_FullName.Text.ToString();
            acc.dob = t_dob.Text.ToString();
            acc.Address = t_Address.Text.ToString();

            DAO_Account.get_Instance().EditAccount(acc);
            MessageBox.Show("Successfully edited information!");
            this.t_Password.Text = "Hidden*";

            SetAllTextBoxToReadOnly();
        }
        private DataTable ListToDataTable(List<String> col, List<DTO_Account> listOfUser) { 
            DataTable dt = new DataTable();
            foreach(String s in col)
            {
                dt.Columns.Add(s);
            }

            if (listOfUser.Count == 0)
            {
                return dt;
            }
            foreach (DTO_Account acc in listOfUser)
            {
                dt.Rows.Add(new object[] { acc.Email, acc.Phone, acc.Fullname });
            }

            return dt;
        }
        public void LoadListData()
        {
            List<DTO_Account> listOfUser;
            listOfUser = DAO_Account.get_Instance().LoadUserList();
            DataTable dt =  ListToDataTable(new List<String> { "Email", "Phone", "Fullname" }, listOfUser);

            this.dgv_userlist.DataSource = dt;
            this.dgv_userlist.Update();
            this.dgv_userlist.Refresh();
            //TextBox Event Full Row Selected Click
        }
        public void reloadListDataTable() {
            this.dgv_userlist.DataSource = null;
            LoadListData();
        }
    }
}
