public class InputHelper
{
    
    public int GetInt(string ?message = null)
    {
        
        if (!string.IsNullOrWhiteSpace(message))
        {
            
            Console.WriteLine(message);

        }

        string ?rawInput = Console.ReadLine();

        while (true)
        {
            
            try
            {
                
                int.TryParse(rawInput, out int result);
                return result;

            }
            catch
            {
                
                Console.WriteLine("Error: incorrect input");

            }

        }

    }

    public string GetString(string ?message = null, bool ?allowNull = null)
    {
        
        if (!string.IsNullOrWhiteSpace(message))
        {
            
            Console.WriteLine(message);

        }

        string ?input = Console.ReadLine();

        if (allowNull == true)
        {
            
            return input;

        }

        while (true)
        {
            
            if (string.IsNullOrWhiteSpace(input))
            {
                
                Console.WriteLine("Error: there is no input");

            }
            else
            {
                
                return input;

            }

        }

    }

}