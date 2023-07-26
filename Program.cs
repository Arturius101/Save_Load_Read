using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

var currentDirectory = Directory.GetCurrentDirectory(); // возвращает полный путь
var storesDirectory = Path.Combine(currentDirectory, "stores");
var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir); // Создание папки
var salesFiles = FindFiles(storesDirectory);
var salesTotal = CalculateSalesTotal(salesFiles);

// чтобы в файле ничего не перезаписывалось и Добавление данных в файлы
File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}"); // Писать текст с новой строки: Environment.NewLine

// проверяет, если ли такая папка, во избежание дублей
bool doesDirectoryExist = Directory.Exists("newDir");
// создаёт новую папку, если её нет
Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "stores", "newDir"));

// Создаём новый файл с содержимым:
// File.WriteAllText(Path.Combine(salesTotalDir, "totals.txt"), "Hello World!");
// Поиск, например, в СПЕЦИАЛЬНОЙ папке "MyDocuments", если не знаешь точного места расположения
// string foundFiles = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();
    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        // поиск файлов с конкретным расширение
        var extension = Path.GetExtension(file);

        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;

    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);

        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);

        // Add the amount found in the Total field to the salesTotal variable
        salesTotal += data?.Total ?? 0;
    }

    return salesTotal;
}

record SalesData(double Total);


//Если необходимо работать с файлами других типов, а не ".json", соответствующие пакеты можно найти на сайте nuget.org.
//Например, можно использовать пакет CsvHelper для CSV-файлов.

// запускать приложение в терминале: dotnet run
