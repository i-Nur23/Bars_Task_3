using System;
using System.Threading;
namespace Program;

internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Введите текст запроса. Для выхода введите /q.");
        var command = Console.ReadLine();

        while (command != "/q")
        {
            string [] args = { };
            int i = 0;
            Console.WriteLine("Введите аргументы. Введите /void если аргументов нет");
            var stringArg = Console.ReadLine();
            while (stringArg != "/void")
            {
                Array.Resize(ref args, i+1);
                args[i] = stringArg;
                Console.WriteLine("Введите следующий аргумент. /void - больше аргументов нет");
                stringArg = Console.ReadLine();
                i++;
            }
            
            Thread write = new Thread((() => Writer(command,args)));
            write.IsBackground = true;
            write.Start();
            
            Console.WriteLine("Введите текст запроса. Для выхода введите /q.");
            command = Console.ReadLine();
        }
        Console.WriteLine("Завершение работы");
    }

    static void Writer(string message, string[] args)
    {
        var result = new DummyRequestHandler().HandleRequest(message,args);
        Console.WriteLine($"Сообщение с идентификатром {Guid.NewGuid().ToString("D")} {result}");
    }
}

public interface IRequestHandler
{
    string HandleRequest(string message, string[] arguments);
}

public class DummyRequestHandler : IRequestHandler
{
    public string HandleRequest(string message, string[] arguments)
    {
        // Притворяемся, что делаем что то.
        Thread.Sleep(10_000);
        try
        {
            if (message.Contains("упади"))
            {
                throw new Exception("Я упал, как сам просил");
            }
            return String.Concat("получил ответ: ",Guid.NewGuid().ToString("D"));
        }
        catch (Exception ex)
        {
            return String.Concat("упал с ошибкой: ", ex.Message);
        }
    }
}
