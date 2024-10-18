using System.Net.Http.Json;
using MassageAppServer;

HttpClient client = new HttpClient();
string serverUrl = "https://localhost:7099/api/Message";
int lastSeenMessageCount = 0;

Console.Write("Enter your username: ");
string username = Console.ReadLine();

var receiveTask = Task.Run(() => ReceiveMessages());

await SendMessages(username);


async Task ReceiveMessages()
{
    while (true)
    {
        try
        {
            var messages = await client.GetFromJsonAsync<List<Message>>(serverUrl);

            if (messages != null && messages.Count > lastSeenMessageCount)
            {
                var newMessages = messages.GetRange(lastSeenMessageCount, messages.Count - lastSeenMessageCount);
                foreach (var msg in newMessages)
                {
                    Console.WriteLine($"{msg.Owner}: {msg.Text}");
                }
                lastSeenMessageCount = messages.Count;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error receiving messages: {ex.Message}");
        }

        await Task.Delay(1000);
    }
}

async Task SendMessages(string owner)
{
    while (true)
    {
        string text = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(text))
        {
            var message = new Message { Owner = owner, Text = text };
            try
            {
                var response = await client.PostAsJsonAsync(serverUrl, message);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error sending message.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }
    }
}