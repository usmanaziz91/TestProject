using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Assignment
{
    public partial class Main : Form
    {
        int operationCheck = 0; 
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dbDataSet.VideoCategory' table. You can move, or remove it, as needed.
            this.videoCategoryTableAdapter.Fill(this.dbDataSet.VideoCategory);
            // TODO: This line of code loads data into the 'dbDataSet.Video' table. You can move, or remove it, as needed.
            this.videoTableAdapter.Fill(this.dbDataSet.Video);
            // TODO: This line of code loads data into the 'dbDataSet.VideoCategory' table. You can move, or remove it, as needed.
           
            // TODO: This line of code loads data into the 'dbDataSet.Video' table. You can move, or remove it, as needed.
            this.videoTableAdapter.Fill(this.dbDataSet.Video);
            try
            {
                
                //label1.Text=LoggedInUser.Username;
                DAL.DataClasses1DataContext objdal=new DAL.DataClasses1DataContext();
               // var table = (from c in objdal.VideoCategories select c);
                comboBox1.DataSource = (from c in objdal.VideoCategories select c).ToList();
                comboBox1.DisplayMember = "CategoryName";
                comboBox1.ValueMember = "CategoryName";

                BindGrid();
                UpdateID();
               
            }
            catch 
            { 
            
            }
        }
        private void BindGrid()
        {
            try
            { 
                DAL.DataClasses1DataContext objdal=new DAL.DataClasses1DataContext();
                dataGridView1.DataSource = from c in objdal.Videos
                                           select c;  //hide navigation column
                dataGridView1.Columns[0].ReadOnly = true; //make the id column read only

                //add new button column to the DataGridView
                //This column displays a delete icon in each row
                DataGridViewLinkColumn delbut = new DataGridViewLinkColumn();
                delbut.Width = 20;
                delbut.Text = "Add/Update";
                delbut.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                delbut.UseColumnTextForLinkValue = true;
                dataGridView1.Columns.Add(delbut);


                DataGridViewLinkColumn Update = new DataGridViewLinkColumn();
                Update.Width = 20;
                Update.Text = "Delete";
                Update.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                Update.UseColumnTextForLinkValue = true;
                dataGridView1.Columns.Add(Update);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTitle.Text.Trim() != "" && txtURL.Text.Trim() != "")
                {
                    DAL.DataClasses1DataContext  objdal=new DAL.DataClasses1DataContext();


                    DAL.Video objvideo= new DAL.Video();

                    objvideo.VideoTitle = txtTitle.Text.Trim();
                    objvideo.VideoUrl = txtURL.Text.Trim();
                    objvideo.CategoryTitle = comboBox1.SelectedValue.ToString();
                    objvideo.UserID = LoggedInUser.UserID;

                    objdal.Videos.InsertOnSubmit(objvideo);
                    objdal.SubmitChanges();
                    if (objvideo.ID > 0)
                    {
                        MessageBox.Show("Record added successfully");
                        dataGridView1.DataSource = from c in objdal.Videos
                                                   select c;
                        UpdateID();
                        txtTitle.Text = txtURL.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Record not added");
                    }
                    

                }
            }
            catch
            {
            
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            { 
            
            }
        }
         
         

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            if (e.ColumnIndex == 5 && e.RowIndex >= 0 && operationCheck == 0 && id!=0) //delete icon button is clicked
            {
                 
                DialogResult result = MessageBox.Show("Do you want to delete this record?", "Confirmation", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    DeleteVideo(id); //delete the record from the Book table
                     
                }

            }
            else if (e.ColumnIndex == 4 && e.RowIndex >= 0 && id!=0)
            { 
                DialogResult result = MessageBox.Show("Do you want to update this record?", "Confirmation", MessageBoxButtons.OKCancel);
                if (result == DialogResult.OK)
                {
                    DAL.DataClasses1DataContext objdal = new DAL.DataClasses1DataContext();
                    DAL.Video objvideo = objdal.Videos.FirstOrDefault(x => x.ID == id);
                    if (objvideo != null)
                    {
                        objvideo.VideoTitle = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                        objvideo.VideoUrl = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                        objvideo.CategoryTitle = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                        
                        objdal.SubmitChanges();
                        //bind grid 
                        dataGridView1.DataSource = from c in objdal.Videos
                                                   select c;
                    }
                }
            }
            else if (e.ColumnIndex == 4 && e.RowIndex >= 0  && id==0)
            {

                DAL.DataClasses1DataContext objdal = new DAL.DataClasses1DataContext();
                DAL.Video objvideo = new DAL.Video();
                objvideo.UserID = LoggedInUser.UserID;
                objvideo.VideoTitle = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                objvideo.VideoUrl = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                objvideo.CategoryTitle = objvideo.VideoTitle = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();

                objdal.Videos.InsertOnSubmit(objvideo);
                objdal.SubmitChanges();
                //bind grid 
                dataGridView1.DataSource = from c in objdal.Videos
                                           select c;
                 

            }
            UpdateID();
            
        }

        private void DeleteVideo(int id)
        {
            try
            {
                DAL.DataClasses1DataContext objdal = new DAL.DataClasses1DataContext();
                DAL.Video objvideo = objdal.Videos.FirstOrDefault(x => x.ID == id);
                if (objvideo != null)
                {
                    objdal.Videos.DeleteOnSubmit(objvideo);
                    objdal.SubmitChanges();
                    //bind grid 
                    dataGridView1.DataSource = from c in objdal.Videos
                                               select c;
                }
            }
            catch
            { }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var login = (Login)Tag;
            login.Show();
            Close();
        }
        private void UpdateID()
        {
            try
            {
                DAL.DataClasses1DataContext objdal = new DAL.DataClasses1DataContext();
                int ID=0;
                try
                {
                    ID = (from c in objdal.Videos
                          select c).Max(x => x.ID);
                }
                catch
                {
                    ID = 1;
                }
                ID++;
                textBox1.Text = ID.ToString();
            }
            catch { }
        }
        

         
    }
}
