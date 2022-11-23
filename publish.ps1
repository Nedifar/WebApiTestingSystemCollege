if (!([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) { Start-Process powershell.exe "-NoProfile -ExecutionPolicy Bypass -File `"$PSCommandPath`"" -Verb RunAs; exit }
Invoke-Command -ComputerName 192.168.147.51 -Credential 192.168.147.51\su -ScriptBlock {Stop-Website -Name f}
dotnet publish -p:PublishProfile=FTPProfile
Invoke-Command -ComputerName 192.168.147.51 -Credential 192.168.147.51\su -ScriptBlock {Start-Website -Name f}
$shell = New-Object -ComObject Wscript.Shell
$shell.popup("Ура все получилось",0,"Результат" , 64)