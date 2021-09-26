module CLI

open Repl
open Parser
open System.IO
open Interpreter
open CoreLib
open Runtime

let private runScript script =
    let initialState : ComputationState = (None, crateEnvWithCoreLibFunctions())
    match parseProgramm (script+"\n") with
    | Result.Ok presult -> 
        compute presult.Result initialState |> ignore; 1
    | Result.Error e -> 
        parserErrorPrinter e; 0

let run argv =
    let initialState : ComputationState = (None, crateEnvWithCoreLibFunctions())
    match argv with
    | [||] ->
        printfn "juri repl (juri version 0.1.0)"
        startRepl() |> ignore; 1
    | [|path|] ->
        if File.Exists(path) then
            printfn $"executing file: \"{path}\""
            runScript(File.ReadAllText(path))
        else
            printfn $"file not found: \"{path}\""; 0
    | _ -> printfn "too many arguments"; 0