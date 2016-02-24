﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        #region EndpointConfiguration
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
        endpointConfiguration.UseTransport<SqlServerTransport>()
            .ConnectionString(@"Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True");
        endpointConfiguration.EndpointName("Samples.SqlServer.NativeIntegration");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        #endregion

        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        try
        {
            Console.WriteLine("Press enter to send a message");
            Console.WriteLine("Press any key to exit");

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter)
                {
                    return;
                }
                PlaceOrder();
            }
        }
        finally
        {
            await endpoint.Stop();
        }
    }

    static void PlaceOrder()
    {
        #region MessagePayload

        string message = @"{
                               $type: 'PlaceOrder',
                               OrderId: 'Order from ADO.net sender'
                            }";

        #endregion

        #region SendingUsingAdoNet

        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=samples;Integrated Security=True";
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string insertSql = @"INSERT INTO [Samples.SqlServer.NativeIntegration] ([Id],[Recoverable],[Headers],[Body]) VALUES (@Id,@Recoverable,@Headers,@Body)";
            using (SqlCommand command = new SqlCommand(insertSql, connection))
            {
                command.CommandType = CommandType.Text;

                command.Parameters.Add("Id", SqlDbType.UniqueIdentifier).Value = Guid.NewGuid();
                command.Parameters.Add("Headers", SqlDbType.VarChar).Value = "";
                command.Parameters.Add("Body", SqlDbType.VarBinary).Value = Encoding.UTF8.GetBytes(message);
                command.Parameters.Add("Recoverable", SqlDbType.Bit).Value = true;

                command.ExecuteNonQuery();
            }
        }

        #endregion
    }
}
