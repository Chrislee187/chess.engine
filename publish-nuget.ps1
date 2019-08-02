param($version="", $apikey="")
$package = ".\src\chess.engine\bin\Debug\chess.engine." + $version + ".nupkg"
dotnet nuget push $package -k $apikey