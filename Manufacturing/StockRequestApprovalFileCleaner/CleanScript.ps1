$path = "C:\Git\SharePointDemo\Manufacturing\StockRequestApprovalFileCleaner\FileCleaner\FileCleaner\bin\Debug\this";
$time = (Get-Date).AddDays(-1);

Get-ChildItem $path | Where-Object {$_.CreationTime -lt $time} | Remove-Item;