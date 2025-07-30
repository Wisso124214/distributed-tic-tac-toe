Start-Process powershell -ArgumentList "-NoExit -WindowStyle minimized -Command & { cd 'C:\Users\p\Documents\_URU\Decimo_primer_trimestre\Distribuidos\.net\distributed tic tac toe'; dotnet run --project .\Server\Server.csproj }"

Start-Process powershell -ArgumentList "-NoExit -WindowStyle minimized -Command & { cd 'C:\Users\p\Documents\_URU\Decimo_primer_trimestre\Distribuidos\.net\distributed tic tac toe'; dotnet run --project .\PCClient\PCClient.csproj }"

Start-Process powershell -ArgumentList "-NoExit -Command & { cd 'C:\Users\p\Documents\_URU\Decimo_primer_trimestre\Distribuidos\.net\distributed tic tac toe'; dotnet run --project .\UserClient\UserClient.csproj }"


#Start-Process powershell -ArgumentList "-NoExit -WindowStyle minimized -Command & { cd 'C:\Users\p\Documents\_URU\Decimo_primer_trimestre\Distribuidos\.net\distributed tic tac toe'; dotnet run --project .\Server\Server.csproj }"
#Start-Process powershell -ArgumentList "-NoExit -WindowStyle minimized -Command & { cd 'C:\Users\p\Documents\_URU\Decimo_primer_trimestre\Distribuidos\.net\distributed tic tac toe'; dotnet run --project .\PCClient\PCClient.csproj }"
#Start-Process powershell -ArgumentList "-NoExit -Command & { cd 'C:\Users\p\Documents\_URU\Decimo_primer_trimestre\Distribuidos\.net\distributed tic tac toe'; dotnet run --project .\UserClient\UserClient.csproj }"
#-WindowStyle Hidden