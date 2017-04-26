open System

module Logic =
    open Telegram.Bot
    let filter (x: Types.Update) = x.Message.Sticker != null

module Messengers =
    open Telegram.Bot
    let toString x =
        use out = new System.IO.StringWriter()
        Newtonsoft.Json.JsonSerializer.CreateDefault().Serialize(out, x)
        out.ToString()
    let getNewBotMessages token =
        let bot = TelegramBotClient(token)
        bot.OnUpdate 
        |> Event.map (fun x -> x.Update)
        |> Event.filter Logic.filter
        |> Event.add (fun x -> 
            printfn "update = %O" (toString x.Message.Sticker))
        |> ignore
        bot.StartReceiving()

[<EntryPoint>]
let main argv =
    printfn "start"
    Messengers.getNewBotMessages argv.[0]
    Threading.Thread.Sleep(5_000)
    printfn "end"
    0