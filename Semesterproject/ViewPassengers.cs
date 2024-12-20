﻿using MongoDB.Bson;
using MongoDB.Driver;
using Semesterproject.Models;
using Semesterproject.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Semesterproject
{
    public partial class ViewPassengers : Form
    {
        private readonly IMongoCollection<Passengers> _passengersCollection;
        public ViewPassengers()
        {
            InitializeComponent();

            // Initialize the MongoDB service
            var mongoService = new MongoDBservices();
            _passengersCollection = mongoService.GetPassengersCollection();
        }

        private void LoadPassengers()
        {
            try
            {
                var passengers = _passengersCollection.Find(_ => true).ToList();
                guna2DataGridView1.DataSource = passengers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ViewPassengers_Load(object sender, EventArgs e)
        {
            LoadPassengers();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddPassengers addP = new AddPassengers();
            addP.Show();
            this.Hide();
        }

        private void lbl_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txt_PassId.Text == "")
            {
                MessageBox.Show("Enter PassengerId To Delete");
            }
            else
            {
                var filter = Builders<Passengers>.Filter
                .Eq("PassId", txt_PassId.Text);

                var result = _passengersCollection.Find(filter).FirstOrDefault();

                if (result != null)
                {
                    var delete = _passengersCollection.DeleteOne(filter);

                    if (delete.DeletedCount > 0)
                    {
                        MessageBox.Show("Passenger Deleted SuccessFully..");
                        var passengers = _passengersCollection.Find(_ => true).ToList();
                        guna2DataGridView1.DataSource = passengers;
                    }
                    else
                    {
                        MessageBox.Show("Passenger Deletion Failed..");
                    }
                }
                else
                {
                    MessageBox.Show("Passenger Is Not found!!");
                }
            }
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_PassId.Text = guna2DataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            txt_PassName.Text = guna2DataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            txt_PassAdress.Text = guna2DataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            txt_PassNation.Text = guna2DataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            txt_passport.Text = guna2DataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            txt_Gender.Text = guna2DataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            txt_Phone.Text = guna2DataGridView1.SelectedRows[0].Cells[6].Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            txt_PassId.Text = "";
            txt_PassName.Text = "";
            txt_PassAdress.Text = "";
            txt_PassNation.Text = "";
            txt_passport.Text = "";
            txt_Gender.Text = "";
            txt_Phone.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (txt_Gender.Text == "" || txt_Phone.Text == "" || txt_PassAdress.Text == "" || txt_PassId.Text == "" || txt_PassNation.Text == "" || txt_PassName.Text == "" || txt_passport.Text == "")
            {
                MessageBox.Show("Missing Information !!");
            }
            else
            {
                var filter = Builders<Passengers>.Filter.Eq("PassId", txt_PassId.Text);
                var update = Builders<Passengers>.Update
                .Set("PassId", txt_PassId.Text)
                .Set("PassAddress", txt_PassAdress.Text)
                .Set("PassNationality", txt_PassNation.Text)
                .Set("PassGender", txt_Gender.Text)
                .Set("PassName", txt_PassName.Text)
                .Set("PassPhone", txt_Phone.Text)
                .Set("Passport", txt_passport.Text);

                _passengersCollection.UpdateOne(filter, update);

                MessageBox.Show("Updated Successfully..");
                var passengers = _passengersCollection.Find(_ => true).ToList();
                guna2DataGridView1.DataSource = passengers;
            }
        }
    }
}