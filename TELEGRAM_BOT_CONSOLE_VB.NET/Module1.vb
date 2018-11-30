Imports System.IO
Imports System.Text
Imports System.Timers
Imports Telegram.Bot
Imports Telegram.Bot.Args
Imports Telegram.Bot.Types
Imports Telegram.Bot.Types.Enums
Imports Telegram.Bot.Types.ReplyMarkups

Module Module1

    Dim bott As Telegram.Bot.TelegramBotClient
    Sub initbot()
        Dim YourBotTokenHere = "TU TOKEN AQUI"
        bott = New Telegram.Bot.TelegramBotClient(YourBotTokenHere)

        AddHandler bott.OnUpdate, AddressOf OnUpdate
        AddHandler bott.OnMessage, AddressOf OnMessageReceived
        AddHandler bott.OnMessageEdited, AddressOf OnMessageEdited
        AddHandler bott.OnCallbackQuery, AddressOf OnCallbackQuery
        AddHandler bott.OnInlineQuery, AddressOf BotOnInlineQueryReceived
        AddHandler bott.OnInlineResultChosen, AddressOf BotOnChosenInlineResultReceived
        AddHandler bott.OnReceiveError, AddressOf BotOnReceiveError

        If Not bott.IsReceiving Then
            bott.StartReceiving()
        End If

        Console.WriteLine("Bot Iniciado")

    End Sub

    Sub Main()
        'Fuente del codigo https://codejournalweb.wordpress.com/2018/02/15/vb-net-receive-telegram-messages/
        initbot()
        Console.ReadLine()
    End Sub


    Private Sub BotOnReceiveError(sender As Object, e As ReceiveErrorEventArgs)
        Console.WriteLine("BotOnReceiveError")
    End Sub

    Private Sub BotOnChosenInlineResultReceived(sender As Object, e As ChosenInlineResultEventArgs)
        Console.WriteLine("BotOnChosenInlineResultReceived")
    End Sub

    Private Sub BotOnInlineQueryReceived(sender As Object, e As InlineQueryEventArgs)
        Console.WriteLine("BotOnInlineQueryReceived")
    End Sub

    Private Sub OnCallbackQuery(sender As Object, e As CallbackQueryEventArgs)
        Console.WriteLine("OnCallbackQuery")
    End Sub

    Private Sub OnMessageEdited(sender As Object, e As MessageEventArgs)
        Console.WriteLine(e.Message.Text)
    End Sub

    Private Sub OnUpdate(sender As Object, e As UpdateEventArgs)
        Console.WriteLine("OnUpdate")
    End Sub

    Private Async Sub OnMessageReceived(sender As Object, e As MessageEventArgs)
        Dim te As String = e.Message.From.Id.ToString & " " & 'Aqui obtenemos los datos del mensaje para despues mostrarlos
                            e.Message.From.FirstName & " " &
                            e.Message.From.Id & " " &
                            e.Message.From.IsBot & " " &
                            e.Message.From.LastName & " " & e.Message.From.Username

        Dim ID As String = e.Message.From.Id.ToString ' aqui se obtiene el id de quien envia el mensaje

        If e.Message.Type.Equals(Types.Enums.MessageType.Text) Then
            Select Case e.Message.Text
                Case "/now" 'reponde con la fecha y hora
                    Await sendMessage(ID, "es:  " & Now.ToString)

                Case "/myid" 'te envia tu nombre y tu id
                    Await sendMessage(ID, e.Message.From.FirstName & " Codigo: " & e.Message.From.Id.ToString)

                Case "/botones"
                    Dim bt1 As InlineKeyboardButton = New InlineKeyboardButton With {
                        .Text = "Test 1",
                        .CallbackData = "bt1"
                        }
                    Dim bt2 As InlineKeyboardButton = New InlineKeyboardButton With {
                    .Text = "Test 2",
                    .CallbackData = "bt2"
                    }
                    Dim bt3 As InlineKeyboardButton = New InlineKeyboardButton With {
                    .Text = "Test 3",
                    .CallbackData = "bt3"
                    }
                    Dim bt4 As InlineKeyboardButton = New InlineKeyboardButton With {
                    .Text = "Test 4",
                    .CallbackData = "bt4"
                    }

                    'Cuando son 3 botones o menos usar esto
                    'Dim teclado As InlineKeyboardMarkup = New InlineKeyboardMarkup({bt2, bt3, bt4, bt5})

                    'Para varios botones usaremos esto.
                    Dim teclado As InlineKeyboardMarkup = New InlineKeyboardButton()() {
                    New InlineKeyboardButton() {bt1},
                    New InlineKeyboardButton() {bt1, bt2},
                    New InlineKeyboardButton() {bt1, bt2, bt3},
                    New InlineKeyboardButton() {bt1, bt2, bt3, bt4}
                    }
                    Try
                        Await bott.SendTextMessageAsync(ID, "Elije una accion " & e.Message.From.FirstName, replyMarkup:=teclado)
                    Catch ex As Exception
                        Console.WriteLine(ex.ToString)
                    End Try

            End Select
            log(e.Message.Text & " ==> " & te)
        Else
            Dim t = "Lo siento, no soporto este tipo de mensaje!"
            log(e.Message.Text & " ==> " & te)
            Await sendMessage(ID, t)
        End If
    End Sub

    Public Async Function sendMessage(ByVal destID As String, ByVal text As String) As Task
        Try
            Await bott.SendTextMessageAsync(destID, text, Telegram.Bot.Types.Enums.ParseMode.Markdown, False, False, 0, Nothing)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Function

    Private Sub log(txt As String)
        Dim s = Now.ToString & " -- " & txt & vbCrLf
        Console.WriteLine(s)
        Dim folder As String = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
        folder = If(folder.EndsWith("\"), folder, folder & "\") & "IsNotifier\Log\" 'Guarda el log en C:\ProgramData\IsNotifier\Log\
        If Not Directory.Exists(folder) Then
            Directory.CreateDirectory(folder)
        End If
        folder = folder & Now.Month.ToString.PadLeft(2, "0") & Now.Year & ".txt"
        IO.File.AppendAllText(folder, s)
    End Sub
End Module

