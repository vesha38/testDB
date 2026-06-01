using System.Linq.Expressions;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;

namespace testDB;

//

public class Menu
{
    
    InputHelper input = new InputHelper();
    bool isLogged = false;
    public bool IsLogged { get { return isLogged; } }

    public void MainMenu(MySqlConnection connection)
    {

        string[] options = ["Exit", "View users", "Add user", "Remove user"];

        Console.Clear();
        for (int i = 0; i < options.Length; i++)
        {

            Console.WriteLine($"{i}. {options[i]}");

        }

        switch (input.GetInt("Choose an option"))
        {

            case 0:
                {

                    Environment.Exit(0);
                    break;

                }
            case 1:
                {

                    PrintUsers(connection);
                    break;

                }
            case 2:
                {

                    AddUser(connection);
                    break;

                }
            case 3:
                {

                    RemoveUser(connection);
                    break;

                }
            default:
                {

                    Console.WriteLine("No such option");
                    break;

                }

        }

    }

    public void Login(MySqlConnection connection)
    {

        Console.Clear();
        string pswd = input.GetString("Enter password");
        connection.ConnectionString = $"server=localhost;user=root;database=testdb;password={pswd}";

        connection.Open();
        Console.WriteLine("Login successfull");
        PressEnter();
        connection.Close();

        isLogged = true;

    }

    void PrintByRow(MySqlDataReader reader)
    {

        while (reader.Read())
        {

            for (int i = 0; i < reader.FieldCount; i++)
            {

                if ((i + 1) > reader.FieldCount)
                {

                    Console.Write(reader[i]);

                }
                else
                {

                    Console.Write($"{reader[i]} === ");

                }

            }

            Console.WriteLine();

        }

        reader.Close();

    }

    public void PrintUsers(MySqlConnection connection)
    {

        connection.Open();

        string sql = "SELECT * FROM users";

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        MySqlDataReader reader = cmd.ExecuteReader();

        //there used to be PrintByRow()

        PrintByRow(reader);

        connection.Close();
        Console.WriteLine("\nDone");
        PressEnter();

    }

    public void AddUser(MySqlConnection connection)
    {

        connection.Open();

        string userName = input.GetString("Enter user's name");
        string userAge = input.GetString("Enter user's age");

        string sql = "INSERT INTO users(userName, age) "
        + $"VALUES('{userName}', {userAge})";

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        cmd.ExecuteNonQuery();

        sql = $"SELECT * FROM users WHERE userName = '{userName}'";
        cmd.CommandText = sql;
        MySqlDataReader reader = cmd.ExecuteReader();

        Console.WriteLine("Added user:");
        PrintByRow(reader);

        connection.Close();
        PressEnter();

    }

    public void RemoveUser(MySqlConnection connection)
    {

        connection.Open();

        string userID = input.GetString("Enter user's ID");
        string sql = $"DELETE FROM users WHERE users.id = {userID}";

        MySqlCommand cmd = new MySqlCommand(sql, connection);
        cmd.ExecuteNonQuery();
        
        Console.WriteLine($"Successfully deleted user with ID {userID}");

        connection.Close();
        PressEnter();

    }

    public void PressEnter()
    {

        Console.WriteLine("Press ENTER to continue. . .");
        Console.ReadLine();

    }

}

class Program
{
    static void Main(string[] args)
    {

        MySqlConnection connection = new MySqlConnection();
        Menu menu = new Menu();

        while (true)
        {

            try
            {

                if (!menu.IsLogged)
                {

                    menu.Login(connection);

                }
                else
                {

                    menu.MainMenu(connection);

                }

            }
            catch (Exception ex)
            {

                string msg = "Error ";

                if (ex is MySqlException sqlEx)
                    Console.WriteLine(msg + sqlEx.Number + ": " + sqlEx.Message);
                else
                    Console.WriteLine(msg + ": " + ex.Message);

                menu.PressEnter();

            }

        }

    }
}
