Start-Process powershell -ArgumentList "-NoExit -WindowStyle minimized -Command & { cd 'C:\Users\Pc\Documents\Wisso\URU\tenth_firts_quarter\Distribuidos\.net\distributed-tic-tac-toe'; dotnet run --project .\Server\Server.csproj }"

Start-Process powershell -ArgumentList "-NoExit -Command & { cd 'C:\Users\Pc\Documents\Wisso\URU\tenth_firts_quarter\Distribuidos\.net\distributed-tic-tac-toe'; dotnet run --project .\UserClient\UserClient.csproj }"

Start-Process powershell -ArgumentList "-NoExit -Command & { cd 'C:\Users\Pc\Documents\Wisso\URU\tenth_firts_quarter\Distribuidos\.net\distributed-tic-tac-toe'; dotnet run --project .\UserClient\UserClient.csproj }"

#-WindowStyle Hidden