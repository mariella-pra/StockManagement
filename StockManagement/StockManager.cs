namespace StockManagement;

public class StockManager
{
    HashTable hashTable;

    public StockManager()
    {
        hashTable = new HashTable(2003);
    }
    
    //neues stock hinzufügen
    public void AddStock()
    {
        Stock stock = new Stock();
        Console.WriteLine("\n(ADD) Name: ");
        stock.Name = Console.ReadLine();
        Console.WriteLine("\n(ADD) WKN: ");
        stock.WKN = Console.ReadLine();
        Console.WriteLine("\n(ADD) Kürzel: ");
        stock.Kürzel = Console.ReadLine();
        
        hashTable.Insert(stock);
    }
    
    //stock löschen
    public void DeleteStock()
    {
        Console.WriteLine("\n(DEL) Kürzel oder Name eingeben");
        string choice = Console.ReadLine().ToLower();
        hashTable.Delete(choice);
    }

    //stock suchen
    public void SearchStock()
    {
        Console.Write("\n(SEARCH) Kürzel oder Name eingeben: ");
        string key = Console.ReadLine();
        Stock result = hashTable.Search(key);
        hashTable.Print(result);
    }
    //stock importiern
    public void ImportStock()
    {
        Console.WriteLine("\n(IMPORT) Kürzel: ");
        string kürzel = Console.ReadLine();
        
        Console.WriteLine("\n(IMPORT) File Name: ");
        string fileName = Console.ReadLine();
        
        Stock stock = hashTable.Search(kürzel);
        
        if (stock != null)
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("File not found");
                return;
            }
            string[] lines = File.ReadAllLines(fileName);
            
            int index = 0;
            for(int i = 1; i < lines.Length && index < 30; i++)
            {
                string[] columns = lines[i].Split(',');
                StockData stockData = new StockData();
                stockData.Date = columns[0];
                stockData.Close  = double.Parse(columns[1].TrimStart('$').Replace(".", ","));
                stockData.Volume = long.Parse(columns[2]);
                stockData.Open   = double.Parse(columns[3].TrimStart('$').Replace(".", ","));
                stockData.High   = double.Parse(columns[4].TrimStart('$').Replace(".", ","));
                stockData.Low    = double.Parse(columns[5].TrimStart('$').Replace(".", ","));
            
                stock.StockData[index] = stockData;
                index++;
            }
            Console.WriteLine("CSV Data imported");
        }
        else
            Console.WriteLine("Stock not found");
    }
    
    //stock ausgeben
    public void PlotStock()
    {
        Console.Write("\n(PLOT) Kürzel oder Name eingeben: ");
        string input = Console.ReadLine();
        hashTable.Plot(input);
    }
    
    //stock als file speichern
    public void SaveHash()
    {
        Console.WriteLine("\n(SAVE) Filename eingeben");
        string fileName = Console.ReadLine();
        hashTable.Save(fileName);
    }
    
    //stock aus file laden
    public void LoadHash()
    {
        Console.WriteLine("\n(LOAD) Filename eingeben");
        string fileName = Console.ReadLine();
        hashTable.Load(fileName);
    }
}