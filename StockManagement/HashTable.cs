namespace StockManagement;

public class HashTable
{
    //2 tabellen, damit wir nach name und kürzel suchen können
    private Stock[] kürzelTable; //key = kürzel
    private Stock[] nameTable; //key = name
    private int size;

    //lazy deletion
    private Stock deleted = new Stock();
    
    //arrays initialisieren
    public HashTable(int size)
    {
        this.size = size;
        kürzelTable = new Stock[size];
        nameTable = new Stock[size];
    }

    //hashfunktion
    private int Hash(string key)
    {
        long hashValue = 0;
        string lowerKey = key.ToLower(); //case-sensitive
        foreach (char k in lowerKey)
        {
            // * 31 (Primzahl hilft gegen Kollisionen) + ASCII
            hashValue = (hashValue * 31 + k) % size;
        }
        return (int)hashValue;
    
    }

    public void InsertInto(Stock[] table, string key, Stock stock)
    {
        //wo fangen wir an zu suchen?
        int hash = Hash(key);
        for (int i = 0; i < size; i++)
        {
            //quadratische sondierung
            int index = (int)((hash + (long)i * i) % size);
            
            //wenn der platz frei (null) oder als gelöscht markiert ist - aktie hier ablegen
            if (table[index] == null || table[index] == deleted)
            {
                table[index] = stock;
                return;
            }
        }
    }
    
    //insert in beide arrays (kürzel und name)
    public void Insert(Stock stock)
    {
        InsertInto(kürzelTable, stock.Kürzel, stock);
        InsertInto(nameTable, stock.Name, stock);
    }
    
    public Stock SearchIn(Stock[] table, string key)
    {
        //startindex berechnen
        int start = Hash(key);
        
        for (int i = 0; i < size; i++)
        {
            //quadratische sondierung
            int index = (int)((start + (long)i * i) % size);
            
            //wenn null, existiert key nicht
            if (table[index] == null) return null;
            
            //wenn der platz belegt ist (nicht deleted)
            if (table[index] != deleted)
            {
                string storedKey;
                if (table == kürzelTable)
                    storedKey = table[index].Kürzel;
                else
                    storedKey = table[index].Name;

                //vergleich durchführen - gross klein schreibung ignorieren
                if (storedKey.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    return table[index];
                }
            }
        }
        
        return null;
    }
    public Stock Search(string key)
    {
        //erst in kürzel-tabelle suchen, dann in name-tabelle
        Stock result = SearchIn(kürzelTable, key);
        if (result == null)
            result = SearchIn(nameTable, key);
        
        return result;
    }
    
    public void DeleteFrom(Stock[] table, string key)
    {
        //start berechnen
        int start = Hash(key);
        
        for (int i = 0; i < size; i++)
        {
            int index = (int)((start + (long)i * i) % size);
            if (table[index] == null) return; // nicht gefunden
            if (table[index] != deleted)
            {
                string storedKey;
                if (table == kürzelTable)
                    storedKey = table[index].Kürzel;
                else
                    storedKey = table[index].Name;

                //vergleich durchführen
                //gross klein schreibung ignorieren
                if (storedKey.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    table[index] = deleted; //lazy deletion - markierung setzen
                    Console.WriteLine("deleted");
                    
                    return;
                }
            }
        }
    }

    public void Delete(string key)
    {
        //stock finden
        Stock stock = Search(key);
        if (stock == null)
        {
            Console.WriteLine($"Aktie '{key}' nicht gefunden.");
            return;
        }

        //aus beiden tabellen löschen
        DeleteFrom(kürzelTable, stock.Kürzel);
        DeleteFrom(nameTable,   stock.Name);
        Console.WriteLine($"Aktie '{stock.Name}' ({stock.Kürzel}) gelöscht.");
    }

    //eine aktie ausgeben
    public void Print(Stock stock)
    {
        if (stock == null)
        {
            Console.WriteLine("Aktie nicht gefunden.");
            return;
        }

        Console.WriteLine($"\nName:   {stock.Name}");
        Console.WriteLine($"WKN:    {stock.WKN}");
        Console.WriteLine($"Kürzel: {stock.Kürzel}");

        //daten vom letzen tag ausgeben
        if (stock.StockData[0] != null)
        {
            StockData stockData = stock.StockData[0];
            Console.WriteLine($"--- Aktuellster Kurs ---");
            Console.WriteLine($"Date:   {stockData.Date}");
            Console.WriteLine($"Close:  ${stockData.Close:F2}");
            Console.WriteLine($"Volume: {stockData.Volume}");
            Console.WriteLine($"Open:   ${stockData.Open:F2}");
            Console.WriteLine($"High:   ${stockData.High:F2}");
            Console.WriteLine($"Low:    ${stockData.Low:F2}");
        }
        else
        {
            Console.WriteLine("Keine Kursdaten vorhanden.");
        }
    }

    //ASCII Grafik
    public void Plot(string key)
    {
        Stock stock = Search(key);
        if (stock == null)
        {
            Console.WriteLine("Stock not found");
            return;
        }
        
        //höchsten preis suchen um grafik zu skalieren
        double max = 0;
        foreach (var entry in stock.StockData) {
            if (entry != null && entry.Close > max) max = entry.Close;
        }
        
        //basis bei 85% für sichtbare veränderungen
        double basis = max * 0.85;
        Console.WriteLine($"\nChart für {stock.Name}");
        foreach (var entry in stock.StockData) {
            if (entry == null) continue;

            // differenz
            double differenz = entry.Close - basis;
            if (differenz < 0) differenz = 0;

            // balken skalieren
            int barLength = (int)(differenz / (max - basis) * 30) + 5;

            Console.Write(entry.Date + " | ");
            for (int i = 0; i < barLength; i++) Console.Write("█");
            Console.WriteLine($" ${entry.Close}");
        }
        
    }

    //in eine datei speichern
    public void Save(string fileName)
    {
        StreamWriter sw = new StreamWriter(fileName);
        for (int i = 0; i < size; i++)
        {
            if (kürzelTable[i] == null || kürzelTable[i] == deleted) continue;
            
                Stock stock = kürzelTable[i];
                //markierung für start einer aktie
                sw.WriteLine("Stock");
                sw.WriteLine(stock.Name);
                sw.WriteLine(stock.WKN);
                sw.WriteLine(stock.Kürzel);
                
                foreach (StockData entry in stock.StockData)
                {
                    if (entry != null)
                    {
                        //markierung für daten einer aktie
                        sw.WriteLine("StockData");
                        sw.WriteLine(entry.Date);
                        sw.WriteLine(entry.Close);
                        sw.WriteLine(entry.Volume);
                        sw.WriteLine(entry.Open);
                        sw.WriteLine(entry.High);
                        sw.WriteLine(entry.Low);
                    }
                }
                sw.WriteLine("End of Stock");   
        }
        sw.Close();
        Console.WriteLine("File saved");
    }

    public void Load(string fileName)
    {
        //checken ob die file existiert
        if (!File.Exists(fileName))
        {
            Console.WriteLine("File not found");
            return;
        }
        
        //tabelle leeren - duplikate beim neuladen vermeiden
        kürzelTable = new Stock[size];
        nameTable = new Stock[size];
        
        string[] lines = File.ReadAllLines(fileName);
        Stock curStock = null;
        int dataIndex = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            
            //neue aktie fängt an
            if (line == "Stock")
            {
                if (i + 3 >= lines.Length) break;
                
                curStock = new Stock();
                curStock.Name = lines[++i];
                curStock.WKN = lines[++i];
                curStock.Kürzel = lines[++i];
                dataIndex = 0;
            }
            else if (line == "StockData" && curStock != null)
            {
                StockData d = new StockData();
                d.Date = lines[++i];
                d.Close = double.Parse(lines[++i]);
                d.Volume = long.Parse(lines[++i]);
                d.Open = double.Parse(lines[++i]);
                d.High = double.Parse(lines[++i]);
                d.Low = double.Parse(lines[++i]);

                if (dataIndex < 30)
                {
                    curStock.StockData[dataIndex] = d;
                    dataIndex++;
                }
            }
            else if (line == "End of Stock")
            {
                if (curStock != null)
                {
                    Insert(curStock);
                    curStock = null; 
                }
                
            }
        }
        Console.WriteLine("File Loaded");
    }
}