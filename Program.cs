using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace Assignment7
{
    class Program
    {
        static string connectionString = "server=DESKTOP-C057B7R;database=LibraryDB;trusted_connection=true;";

        static void Main(string[] args)
        {
            // Step 2: Retrieve Data Into a DataSet
            DataSet libraryDataSet = RetrieveData();
            start:

                Console.WriteLine("Library Management System");
                Console.WriteLine("1. Display Book Inventory");
                Console.WriteLine("2. Add New Book");
                Console.WriteLine("3. Update Book Quantity");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        // Step 4: Display Book Inventory
                        DisplayBookInventory(libraryDataSet);
                        break;

                    case "2":
                        // Step 5: Add New Book
                        AddNewBook(libraryDataSet);
                        break;

                    case "3":
                        // Step 6: Update Book Quantity
                        UpdateBookQuantity(libraryDataSet);
                        break;

                    case "4":
                    return;

                    default:
                        Console.WriteLine("Invalid option. Please select again.");
                        break;
                }
            Console.WriteLine("Do you want to continue enter 1");
            string ch= Console.ReadLine();
                if (ch=="1")
                {
                goto start;
                }
            

            // Step 7: Apply Changes to Database
            ApplyChangesToDatabase(libraryDataSet);
        }

        static DataSet RetrieveData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Using SqlDataAdapter to retrieve data from the database
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Books", connection);
                DataSet dataSet = new DataSet();
                // Filling the DataSet with data from the Books table
                adapter.Fill(dataSet, "Books");
                return dataSet;
            }
        }

        static void DisplayBookInventory(DataSet dataSet)
        {
            DataTable booksTable = dataSet.Tables["Books"];
            Console.WriteLine("Book Inventory:");
            foreach (DataRow row in booksTable.Rows)
            {
                Console.WriteLine($"Title: {row["Title"]}, Author: {row["Author"]}, Genre: {row["Genre"]}, Quantity: {row["Quantity"]}");
            }
        }

        static void AddNewBook(DataSet dataSet)
        {
            DataTable booksTable = dataSet.Tables["Books"];
            DataRow newRow = booksTable.NewRow();

            Console.WriteLine("Add a New Book:");
            Console.Write("Enter BookId: ");
            newRow["BookId"] = int.Parse(Console.ReadLine());

            Console.Write("Enter Book Title: ");
            newRow["Title"] = Console.ReadLine();

            Console.Write("Enter Book Author: ");
            newRow["Author"] = Console.ReadLine();

            Console.Write("Enter Book Genre: ");
            newRow["Genre"] = Console.ReadLine();

            Console.Write("Enter Quantity: ");
            newRow["Quantity"] = int.Parse(Console.ReadLine());

            // Adding the new row to the DataTable
            booksTable.Rows.Add(newRow);
            Console.WriteLine("New Book added successfully.");
            Console.WriteLine();
        }

        static void UpdateBookQuantity(DataSet dataSet)
        {
            DataTable booksTable = dataSet.Tables["Books"];

            Console.WriteLine("Update Book Quantity:");
            Console.Write("Enter Book Title to update quantity: ");
            string bookTitle = Console.ReadLine();

            DataRow[] foundRows = booksTable.Select($"Title = '{bookTitle}'");
            if (foundRows.Length > 0)
            {
                Console.Write("Enter New Quantity: ");
                int newQuantity = int.Parse(Console.ReadLine());

                // Updating the Quantity column for the found row
                foundRows[0]["Quantity"] = newQuantity;
                Console.WriteLine("Quantity updated successfully.");
            }
            else
            {
                Console.WriteLine("Book not found.");
            }
            Console.WriteLine();
        }

        static void ApplyChangesToDatabase(DataSet dataSet)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Using SqlDataAdapter and SqlCommandBuilder to update database changes
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Books", connection);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                // Updating the database with changes from the DataSet
                adapter.Update(dataSet.Tables["Books"]);
            }
            Console.WriteLine("Changes applied to the database.");
        }
    }
}