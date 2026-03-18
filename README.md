# Stock Management System: Custom Hash Table Implementation
C# console application managing stock data using a custom-built Hash Table, featuring CSV parsing and ASCII charting.

This project was developed to gain a deep understanding of data structures. Instead of relying on C#'s built-in `Dictionary` or `HashTable` classes, we implemented a custom Hash Table from scratch to manage stock market data efficiently with an average time complexity of O(1).

## Technical Highlights

* **Custom Data Structure:** A fully functional, self-written Hash Table designed to store and manage up to 1000 stocks.
* **Double Hashing Support:** The system uses two separate internal arrays, allowing constant-time O(1) lookups by both the Stock Name and the Ticker.
* **Quadratic Probing:** Implemented mathematical collision resolution (index = (hash + i^2) % size) to prevent clustering and ensure even data distribution.
* **Optimized Table Size:** Uses a prime number (2003) for the array size to maintain an optimal load factor of ~50%, drastically minimizing collisions.
* **Lazy Deletion:** Uses a specific `DELETED` marker to handle item removal. This ensures that the quadratic probing search chains remain intact when elements are removed.
* **Data Parsing & File I/O:** Capable of parsing stock data from standard Nasdaq CSV files and serializing/saving the entire state of the Hash Table to a local text file for later use.

## Features & Usage

When running the application, the user is presented with an interface offering the following operations:

* `ADD`: Insert a new stock (Name, WKN, Ticker) into the Hash Table.
* `DEL`: Remove a stock from both tables using Lazy Deletion.
* `IMPORT`: Load the last 30 days of stock data (Date, Close, Volume, Open, High, Low) from a `.csv` file.
* `SEARCH`: Find a stock by Name or Ticker and display its most recent market data.
* `PLOT`: Generates a dynamic ASCII-art chart in the console, visualizing the closing prices of the last 30 days scaled relative to the stock's highest price.
* `SAVE` / `LOAD`: Serialize the current Hash Table state to a text file or reconstruct it from a saved file.

## Getting Started

**Requirements:**
* [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.

**Installation:**
1. Clone this repository: `https://github.com/mariella-pra/StockManagement.git`
2. Navigate to the project directory.
3. Run the application via the terminal: `dotnet run`
