﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;

namespace PracaDomowa.SQLITE
{
    internal class CarRepository
    {
        private readonly string connectionString;

        public CarRepository(string connectionString)
        {
            this.connectionString = connectionString;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Open();

                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Cars (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Make TEXT NOT NULL,
                    Model TEXT NOT NULL,
                    Year INTEGER NOT NULL,
                    Color TEXT
                )";
                dbConnection.Execute(createTableQuery);
            }
        }

        public void CreateCar(Car car)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("INSERT INTO Cars (Make, Model, Year, Color) VALUES (@Make, @Model, @Year, @Color)", car);
            }
        }

        public IEnumerable<Car> ReadCars()
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Open();
                return dbConnection.Query<Car>("SELECT * FROM Cars");
            }
        }

        public void UpdateCar(int id, Car updatedCar)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("UPDATE Cars SET Make = @Make, Model = @Model, Year = @Year, Color = @Color WHERE Id = @Id", new { Id = id, Make = updatedCar.Make, Model = updatedCar.Model, Year = updatedCar.Year, Color = updatedCar.Color });
            }
        }

        public void DeleteCar(int id)
        {
            using (IDbConnection dbConnection = new SQLiteConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("DELETE FROM Cars WHERE Id = @Id", new { Id = id });
            }
        }
    }
}
