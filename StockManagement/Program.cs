using StockManagement;

class Program
{
    static void Main(string[] args)
    {
        StockManager manager = new StockManager();
        
        string command;
        do
        {
            Console.WriteLine("\nMENU:\n    -ADD\n    -DEL\n    -IMPORT\n    -SEARCH\n    -PLOT\n    -SAVE\n    -LOAD\n    -QUIT");
            command = Console.ReadLine().ToUpper();
            switch (command)
            {
                case "ADD":
                    manager.AddStock();
                    break;
                case "DEL":
                    manager.DeleteStock();
                    break;
                case "IMPORT":
                    manager.ImportStock();
                    break;
                case "SEARCH":
                    manager.SearchStock();
                    break;
                case "PLOT":
                    manager.PlotStock();
                    break;
                case "SAVE":
                    manager.SaveHash();
                    break;
                case "LOAD":
                    manager.LoadHash();
                    break;
            }
            
        } while (command != "QUIT");
        return;
    }
}