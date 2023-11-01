using System.Xml.Serialization;
using Newtonsoft.Json;
using System.IO;
using LisaConverter;
using System.Reflection.PortableExecutable;

internal class Program
{
    static public List<Paric> paricLIST = new List<Paric>(); // создали статичный лист из париков, чисто как в презентации
    static public string[] paricSTRING = new string[9]; // 9 - число строк в файле, можешь поменять
    static void Main(string[] args)
    {
        Console.WriteLine("Введите путь к файлу, который вы хотите открыть: " +
            "\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        string filePath = Console.ReadLine();

        if (File.Exists(filePath))
        {
            Console.Clear();
            Console.WriteLine("Сохранить файл в одном из 3 форматов (txt, json и xml) - F1. Закрыть программу - Escape " +
                "\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            // тут должен быть метод Load, ReadAllText работает только с тхт  файлами
            //string readText = File.ReadAllText(filePath);
            // Console.WriteLine(readText);
            LoadFile(filePath);
            // вывожу данные
            foreach (var item in paricSTRING)
            {
                Console.WriteLine(item);
            }
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.F1)
                {
                    Program program = new Program(); // даю доступ к методам
                    Console.Clear();
                    Console.WriteLine("Введите путь до файла (месте с расширением), куда нужно сохранить текст " +
                        "\n~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    filePath = Console.ReadLine(); // не нужна другая переменная для пути, переназначаем одну и ту же
                    program.SaveFile(filePath); // тут обратилась к методу через наш основной класс из-за доступа

                    //Console.WriteLine("Файл успешно сохранен."); 
                    // лучше это писать в методе загрузки
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }
            } while (true); // зачем цикл если программа так и так 1 раз проигрывается
        }
        else
        {
            Console.WriteLine("Ошибка при загрузке файла. Программа завершает работу.");
        }
    }

    public void StringConv()
    {
        // нужно преобразовать лист в массив для загрузки в тхт файл
        int i = 0;
        foreach (Paric item in paricLIST)
        {
            paricSTRING[i] = item.Parica;
            i++;
            paricSTRING[i] = item.Price.ToString();
            i++;
            paricSTRING[i] = item.Style;
            i++;
        }

    }

    public void SaveFile(string filePath)
    {

        string fileExtension = Path.GetExtension(filePath);

        if (string.IsNullOrEmpty(fileExtension))
        {
            Console.WriteLine("\nНеизвестный формат файла.");
            return;
        }

        switch (fileExtension)
        {

            case ".txt":
                File.WriteAllLines(filePath, paricSTRING);
                Console.WriteLine("Файл успешно сохранен в txt");
                break;
            //default:
            //    using (StreamWriter writer = new StreamWriter(filePath))
            //    {
            //        writer.WriteLine(Paric.ToString());
            //    }
            //    Console.WriteLine("Файл успешно сохранен в txt");
            //    break;

            case ".json":
                string json = JsonConvert.SerializeObject(paricLIST); 
                File.WriteAllText(filePath, json);
                Console.WriteLine("Файл успешно сохранен в Json");

                break;

            case ".xml":
                XmlSerializer xml = new XmlSerializer(typeof(List<Paric>)); // меняем на вариант из презентации с typeof
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    xml.Serialize(writer, paricLIST); //---
                }
                Console.WriteLine("Файл успешно сохранен в Xml");
                break;

        }

    }

    static object LoadFile(string filePath)
    {
        Program program = new Program();
        string fileExtension = Path.GetExtension(filePath);

        if (string.IsNullOrEmpty(fileExtension))
        {
            Console.WriteLine("Неизвестный формат файла.");
            return null;
        }

        try
        {
            string fileContent = File.ReadAllText(filePath); // не читает файлы других разрешений, только тхт И в любом случае зачем здесь эта строка

            switch (fileExtension)
            {
                // меняю везде "Paric" на лист с париками

                case ".json":
                    string json = File.ReadAllText(filePath);
                    paricLIST = JsonConvert.DeserializeObject<List<Paric>>(json); //--
                    program.StringConv();
                    return paricLIST; //---

                case ".xml":
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Paric>));
                    using (FileStream reader = new FileStream(filePath, FileMode.Open))
                    {
                        paricLIST = (List<Paric>)serializer.Deserialize(reader); //---
                        program.StringConv();
                        return paricLIST; //---
                    }
                case ".txt":
                    string text = File.ReadAllText(filePath); // здесь надо полностью переписать, чтобы на выходе был лист paricLIST
                    program.StringConv();
                    return text;

                default:
                    Console.WriteLine("Неизвестный формат файла.");
                    return null;

            }
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
            return null;
        }
    }
}
