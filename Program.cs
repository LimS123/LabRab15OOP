using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace LabRab15
{
    class Program
    {
        private static object lockObj = new object();
        static Mutex mutexObj = new Mutex();
        static void Main(string[] args)
        {
            //15.1
            Console.WriteLine("Процессы\n");
           foreach(Process process in Process.GetProcesses())
            {
                Console.WriteLine($"Идентификатор процесса - {process.Id}");
                Console.WriteLine($"Имя процесса - {process.ProcessName}");
                try {
                    Console.WriteLine($"Приоритет процесса - {process.PriorityClass}");
                    Console.WriteLine($"Время запуска процесса - {process.StartTime}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Приоритет процесса - {ex.Message}");
                    Console.WriteLine($"Время запуска процесса - {ex.Message}");
                }
                Console.WriteLine($"Состояние процесса - {process.Responding}");
                try
                {
                    Console.WriteLine($"Время работы процесса - {process.TotalProcessorTime}");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Время работы процесса - {ex.Message}");
                }
                Console.WriteLine("\n");
            }
            //15.2
            Console.WriteLine("Текущий домен\n");
            AppDomain thisAppDomain = AppDomain.CurrentDomain;
            Console.WriteLine($"Имя текущего домена - {thisAppDomain.FriendlyName}");
            Console.WriteLine($"Детали конфигурации - {thisAppDomain.SetupInformation.ApplicationBase},\n" +    //Место расположения домена
                $"{thisAppDomain.SetupInformation.ApplicationName},\n" +    //Имя домена
                $"{thisAppDomain.SetupInformation.ConfigurationFile},\n" +  //Файл конфигурации
                $"{thisAppDomain.SetupInformation.TargetFrameworkName}");   //Имя и версия фреймворка
            Console.WriteLine("Сборки, загруженные в данный момент:");
            foreach (Assembly a in thisAppDomain.GetAssemblies())
            {  
                Console.WriteLine(a);
            }

            AppDomain newDomain = AppDomain.CreateDomain("newDomain");
            newDomain.Load("Лабораторная работа 15");
            try
            {
                AppDomain.Unload(newDomain);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //15.3
            Thread newThread = new Thread(func);
            newThread.Name = "newTread";
            newThread.Start();
            Console.WriteLine($"Статус потока - {newThread.ThreadState}");
            Console.WriteLine($"Наименование потока - {newThread.Name}");
            Console.WriteLine($"Приоритет потока - {newThread.Priority}");
            Console.WriteLine($"Идентификатор потока - {newThread.ManagedThreadId}");
            newThread.Join();
            //15.4
            File.Delete(@"C:\Users\User\Documents\labs2K\LabRab15\bin\Debug\text.txt");
            Thread firstThread = new Thread(firstFunc);
            Thread secondThread = new Thread(secondFunc);
            secondThread.Priority = ThreadPriority.Highest;
            secondThread.Start();
            secondThread.Join();
            firstThread.Start();
            firstThread.Join();
            //15.5
            int number = 6;
            TimerCallback Timer = new TimerCallback(Count);
            Timer tm = new Timer(Timer, number, 0, 3000);
            Console.Read();
        }
        static void func()
        {
            Thread.Sleep(200);
            string address = @"C:\Users\User\Documents\labs2K\LabRab15\bin\Debug\file.txt";
            FileInfo fileInfo = new FileInfo(address);
            Console.WriteLine("Введите n");
            int n = Convert.ToInt16(Console.ReadLine());
                using (StreamWriter sw = new StreamWriter(address, false, Encoding.UTF8))
                {
                    for(int i = 0; i < n; i++)
                    {
                        sw.Write(i + " ");
                        Console.Write(i + " ");
                    }
                Console.WriteLine("\n");
                    sw.Close();
                }
        }
        static void firstFunc()
        {
            string address = @"C:\Users\User\Documents\labs2K\LabRab15\bin\Debug\text.txt";
            FileInfo fileInfo = new FileInfo(address);
            int i = 1;
            //lock (lockObj)
            //{
            
                while (i < 100)
                {
                //mutexObj.WaitOne();
                try
                    {
                        using (StreamWriter sw = File.AppendText(address))
                        {
                            sw.Write(i + " ");
                            sw.Close();
                        }
                    }
                    catch
                    {

                    }
                    Console.Write(i + " ");
                 //mutexObj.ReleaseMutex();
                    i += 2;
                //Thread.Sleep(10);
                }
            
            //}
        }
        static void secondFunc()
        {
            string address = @"C:\Users\User\Documents\labs2K\LabRab15\bin\Debug\text.txt";
            FileInfo fileInfo = new FileInfo(address);
            int i = 0;
            //lock (lockObj)
            //{
            
            while (i < 100)
                {
            //mutexObj.WaitOne();
                    try
                    {
                        using (StreamWriter sw = File.AppendText(address))
                        {
                            sw.Write(i + " ");
                            sw.Close();
                        }
                    }
                    catch
                    {

                    }
                    Console.Write(i + " ");
            //mutexObj.ReleaseMutex();
                    i += 2;
                    //Thread.Sleep(50);
                }
            
            //}
        }
        static void Count(object Object)
        {
            int n = (int)Object;
            Console.WriteLine("Выполнение цикла");
            for(int i = 0; i < n; i++)
            {
                Console.WriteLine("Сообщение " + i);
            }
            Console.WriteLine("\n    ");
        }
    }
}
