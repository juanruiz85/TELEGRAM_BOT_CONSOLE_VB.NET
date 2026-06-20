Imports System.IO
Imports System.Threading
Imports System.Threading.Tasks
Imports Telegram.Bot
Imports Telegram.Bot.Types
Imports Telegram.Bot.Types.Enums
Imports Telegram.Bot.Types.ReplyMarkups

Module Module1

    Private bott As TelegramBotClient

    Sub Main()
        Dim token = Environment.GetEnvironmentVariable("TELEGRAM_BOT_TOKEN")
        If String.IsNullOrWhiteSpace(token) Then
            token = "TU TOKEN AQUI"
        End If

        bott = New TelegramBotClient(token)

        Console.WriteLine("Bot Iniciado")

        Dim cts = New CancellationTokenSource()
        Dim receiveTask = ProcessUpdatesAsync(cts.Token)

        Console.WriteLine("Presiona ENTER para detener el bot...")
        Console.ReadLine()

        cts.Cancel()
        Try
            receiveTask.GetAwaiter().GetResult()
        Catch ex As OperationCanceledException
            Console.WriteLine("Recepción detenida.")
        End Try
    End Sub

    Private Async Function ProcessUpdatesAsync(cancellationToken As CancellationToken) As Task
        Dim offset As Integer? = Nothing

        While Not cancellationToken.IsCancellationRequested
            Dim updates = Await bott.GetUpdatesAsync(offset, limit:=Nothing, timeout:=30, allowedUpdates:=Nothing, cancellationToken:=cancellationToken)
            If updates Is Nothing OrElse updates.Count = 0 Then
                Continue While
            End If

            For Each update In updates
                Try
                    Await ProcessUpdateAsync(update)
                Catch ex As Exception
                    Console.WriteLine($"Error procesando update: {ex}")
                End Try

                offset = update.Id + 1
            Next
        End While
    End Function

    Private Async Function ProcessUpdateAsync(update As Update) As Task
        If update.Message IsNot Nothing Then
            Await HandleMessageAsync(update.Message)
            Return
        End If

        If update.CallbackQuery IsNot Nothing Then
            Console.WriteLine("CallbackQuery recibido")
            Await sendMessage(update.CallbackQuery.Message.Chat.Id.ToString(), "Callback recibido")
            Return
        End If

        If update.InlineQuery IsNot Nothing Then
            Console.WriteLine("InlineQuery recibido")
            Return
        End If

        If update.ChosenInlineResult IsNot Nothing Then
            Console.WriteLine("ChosenInlineResult recibido")
            Return
        End If
    End Function

    Private Async Function HandleMessageAsync(message As Message) As Task
        Dim messageText As String = If(String.IsNullOrWhiteSpace(message.Text), "<no text>", message.Text)
        Dim chatId As String = message.Chat.Id.ToString()

        If message.Type = MessageType.Text Then
            Select Case messageText.Trim().ToLowerInvariant()
                Case "/now"
                    Await sendMessage(chatId, $"es: {Now}")
                Case "/myid"
                    Await sendMessage(chatId, $"{message.From.FirstName} Codigo: {message.From.Id}")
                Case "/botones"
                    Dim bt1 As InlineKeyboardButton = InlineKeyboardButton.WithCallbackData("Test 1", "bt1")
                    Dim bt2 As InlineKeyboardButton = InlineKeyboardButton.WithCallbackData("Test 2", "bt2")
                    Dim bt3 As InlineKeyboardButton = InlineKeyboardButton.WithCallbackData("Test 3", "bt3")
                    Dim bt4 As InlineKeyboardButton = InlineKeyboardButton.WithCallbackData("Test 4", "bt4")

                    Dim teclado As InlineKeyboardMarkup = New InlineKeyboardButton()() {
                        New InlineKeyboardButton() {bt1},
                        New InlineKeyboardButton() {bt1, bt2},
                        New InlineKeyboardButton() {bt1, bt2, bt3},
                        New InlineKeyboardButton() {bt1, bt2, bt3, bt4}
                    }

                    Try
                        Await bott.SendTextMessageAsync(chatId, $"Elije una accion {message.From.FirstName}", replyMarkup:=teclado)
                    Catch ex As Exception
                        Console.WriteLine(ex.ToString())
                    End Try
                Case Else
                    Await sendMessage(chatId, "Comando no reconocido. Usa /now, /myid o /botones")
            End Select

            log($"{messageText} ==> {message.From.Id} {message.From.FirstName} {message.From.LastName} {message.From.Username}")
        Else
            Dim t = "Lo siento, no soporto este tipo de mensaje!"
            log($"{messageText} ==> {message.From.Id} {message.From.FirstName} {message.From.LastName} {message.From.Username}")
            Await sendMessage(chatId, t)
        End If
    End Function

    Public Async Function sendMessage(destID As String, text As String) As Task
        Try
            Await bott.SendTextMessageAsync(destID, text, parseMode:=ParseMode.Markdown)
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try
    End Function

    Private Sub log(txt As String)
        Dim s = $"{Now} -- {txt}" & vbCrLf
        Console.WriteLine(s)
        Dim folder As String = Path.Combine(AppContext.BaseDirectory, "Logs")
        If Not Directory.Exists(folder) Then
            Directory.CreateDirectory(folder)
        End If
        Dim fileName As String = Now.ToString("MMyyyy") & ".txt"
        Dim filePath As String = Path.Combine(folder, fileName)
        System.IO.File.AppendAllText(filePath, s)
    End Sub
End Module
